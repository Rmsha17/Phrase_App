using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.Interfaces;
using Phrase_App.Core.Models;
using System;

namespace Phrase_App.Infrastructure.Services
{
    public class OverlaySettingService : IOverlaySettingService
    {
        private readonly PhraseDbContext _context;
        private readonly IWebHostEnvironment _env;

        public OverlaySettingService(PhraseDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<bool> SaveOverlaySettingsAsync(Guid? userId, OverlaySettingsDto dto)
        {
            if (userId == null) return false;

            var settings = await _context.OverlaySettings.FirstOrDefaultAsync(s => s.UserId == userId);

            if (settings == null)
            {
                settings = new OverlaySetting { UserId = userId.Value, Id = Guid.NewGuid() };
                _context.OverlaySettings.Add(settings);
            }

            // Map basic settings
            settings.FontSize = dto.FontSize;
            settings.FontColor = dto.FontColor;
            settings.FontFamily = dto.FontFamily;
            settings.Opacity = dto.Opacity;
            settings.BackgroundType = dto.BackgroundType;
            settings.AnimationType = dto.AnimationType;
            settings.IntervalMinutes = dto.IntervalMinutes;
            settings.Position = dto.Position;
            settings.VibrationEnabled = dto.VibrationEnabled;
            settings.SoundEffect = dto.SoundEffect;
            settings.DisplayMode = dto.DisplayMode;
            settings.ShowAuthor = dto.ShowAuthor;       // NEW
            settings.SnapToGrid = dto.SnapToGrid;       // NEW
            settings.UpdatedAt = DateTime.UtcNow;

            // Handle Background Logic
            if (dto.BackgroundType == "Image")
            {
                if (dto.ImageFile != null && dto.ImageFile.Length > 0)
                {
                    DeletePreviousImage(settings);
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.ImageFile.FileName)}";
                    var folderPath = Path.Combine(_env.WebRootPath, "overlays");
                    var filePath = Path.Combine(folderPath, fileName);
                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.ImageFile.CopyToAsync(stream);
                    }
                    settings.BackgroundValue = $"/overlays/{fileName}";
                }
            }
            else if (dto.BackgroundType == "None")       // NEW — No background
            {
                DeletePreviousImage(settings);
                settings.BackgroundValue = "0";          // Store "0" for None
            }
            else
            {
                DeletePreviousImage(settings);
                settings.BackgroundValue = dto.BackgroundValue;
            }

            return await _context.SaveChangesAsync() > 0;
        }


        private void DeletePreviousImage(OverlaySetting settings)
        {
            //  DELETE PREVIOUS IMAGE IF IT EXISTS
            if (!string.IsNullOrEmpty(settings.BackgroundValue) && settings.BackgroundValue.StartsWith("/overlays"))
            {
                // Combine WebRootPath with the stored relative path to get the full disk path
                var oldFilePath = Path.Combine(_env.WebRootPath, settings.BackgroundValue.TrimStart('/'));

                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }
        }

        public async Task<OverlaySetting> GetOverlaySettingsAsync(Guid? userId)
        {
            return await _context.OverlaySettings.FirstOrDefaultAsync(s => s.UserId == userId);
        }
    }
}
