namespace Phrase_App.Core.Models
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public required string IconKey { get; set; } // e.g., "growth", "zen"
        public required string ColorHex { get; set; } // e.g., "#00E676"
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
