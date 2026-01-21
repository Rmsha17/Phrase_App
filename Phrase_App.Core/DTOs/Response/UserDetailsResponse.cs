namespace Phrase_App.Core.DTOs.Response
{
    public class UserDetailsDto 
    {
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public bool IsPremium { get; set; } = false;
        public bool DarkMode { get; set; } = false;
        public string? ProfilePicUrl { get; set; }
    }
}
