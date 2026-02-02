namespace Phrase_App.Core.DTOs.Response
{
    public record CategoryResponse(
        Guid Id,
        string Name,
        string IconKey,
        string ColorHex,
        int count,
        DateTime CreatedDate
    );
}
