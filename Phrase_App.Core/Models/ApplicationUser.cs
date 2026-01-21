using Microsoft.AspNetCore.Identity;

namespace Phrase_App.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? ProfileImageUrl { get; set; } 
        public string? Bio { get; set; }
        public bool DarkMode { get; set; } = false; 
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
