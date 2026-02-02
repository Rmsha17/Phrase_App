namespace Phrase_App.Core.DTOs.Request
{
    public record UpdateCategoryRequest(Guid id,string Name, string IconKey, string ColorHex, bool IsActive = true);
}
