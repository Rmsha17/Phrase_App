using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.Models;

namespace Phrase_App.Core.Interfaces
{
    public interface IOverlaySettingService
    {
        Task<OverlaySetting> GetOverlaySettingsAsync(Guid? userId);
        Task<bool> SaveOverlaySettingsAsync(Guid? userId, OverlaySettingsDto dto);
    }
}
