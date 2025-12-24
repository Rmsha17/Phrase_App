using Microsoft.AspNetCore.Identity;

namespace Phrase_App.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
