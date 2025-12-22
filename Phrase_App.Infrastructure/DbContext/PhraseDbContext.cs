using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Phrase_App.Core.Models;

namespace Phrase_App.Infrastructure.DbContext
{
    public class PhraseDbContext : IdentityDbContext<ApplicationUser>
    {
        public PhraseDbContext(DbContextOptions<PhraseDbContext> options)
            : base(options) { }
    }

}
