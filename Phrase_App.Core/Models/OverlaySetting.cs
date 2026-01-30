using System.ComponentModel.DataAnnotations;

namespace Phrase_App.Core.Models
{
    public class OverlaySetting
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        // Typography
        public double FontSize { get; set; } = 24.0;
        public string FontColor { get; set; } = "FFFFFFFF";
        public string FontFamily { get; set; } = "Default";
        public double Opacity { get; set; } = 1.0;

        // Background
        public string BackgroundType { get; set; } = "Image";
        public string? BackgroundValue { get; set; }

        // Animation & Behavior
        public string AnimationType { get; set; } = "Fade";
        public int IntervalMinutes { get; set; } = 10;
        public string Position { get; set; } = "Center";

        // Feedback
        public bool VibrationEnabled { get; set; } = true;
        public string SoundEffect { get; set; } = "Nature Chime";
        public string DisplayMode { get; set; } = "Compact Box";

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
