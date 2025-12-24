using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Phrase_App.Core.Models;

public class PhraseDbContext : IdentityDbContext<ApplicationUser>
{
    public PhraseDbContext(DbContextOptions<PhraseDbContext> options)
        : base(options)
    {
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
}