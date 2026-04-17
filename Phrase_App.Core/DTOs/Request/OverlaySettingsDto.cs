using Microsoft.AspNetCore.Http;
namespace Phrase_App.Core.DTOs.Request
{
    public class OverlaySettingsDto
    {
        public double FontSize { get; set; }
        public string FontColor { get; set; }
        public string FontFamily { get; set; }
        public double Opacity { get; set; }
        public string BackgroundType { get; set; }
        public string? BackgroundValue { get; set; }
        public string AnimationType { get; set; }
        public double IntervalMinutes { get; set; } 
        public string Position { get; set; } = "Center";
        public bool VibrationEnabled { get; set; }
        public string SoundEffect { get; set; }
        public string DisplayMode { get; set; }
        public IFormFile? ImageFile { get; set; }
        public bool ShowAuthor { get; set; } = false;
        public bool SnapToGrid { get; set; } = false;
    }
}
