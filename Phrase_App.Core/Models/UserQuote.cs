using Phrase_App.Core.Models;

public class UserQuote
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    // System Quote Reference
    public Guid? QuoteId { get; set; }
    public Quote? Quote { get; set; }

    // Custom Quote Data
    public string? CustomContent { get; set; }
    public string? CustomAuthor { get; set; }

    // Favorite Filter
    public bool IsFavorite { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}