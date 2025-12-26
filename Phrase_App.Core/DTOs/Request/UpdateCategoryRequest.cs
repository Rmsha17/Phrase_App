namespace Phrase_App.Core.DTOs.Request
{
    public record UpdateCategoryRequest(string Name, string IconKey, string ColorHex, bool IsActive);
}
