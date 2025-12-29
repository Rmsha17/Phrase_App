namespace Phrase_App.Core.Models
{
    public class Quote
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Content { get; set; }
        public string? Author { get; set; } = "Unknown";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relationship
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; } // Navigation Property
    }
}
