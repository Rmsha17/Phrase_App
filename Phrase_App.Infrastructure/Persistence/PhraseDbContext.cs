using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Phrase_App.Core.Constants;
using Phrase_App.Core.Models;

public class PhraseDbContext : IdentityDbContext<ApplicationUser>
{
    public PhraseDbContext(DbContextOptions<PhraseDbContext> options)
        : base(options)
    {
    }

    // PhraseApp.Infrastructure/Data/ApplicationDbContext.cs
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = Guid.NewGuid(), Name = "Growth", IconKey = CategoryDefaults.Icons["growth"], ColorHex = CategoryDefaults.Colors[0] },
            new Category { Id = Guid.NewGuid(), Name = "Focus", IconKey = CategoryDefaults.Icons["focus"], ColorHex = CategoryDefaults.Colors[1] },
            new Category { Id = Guid.NewGuid(), Name = "Zen", IconKey = CategoryDefaults.Icons["zen"], ColorHex = CategoryDefaults.Colors[2] },
            new Category { Id = Guid.NewGuid(), Name = "Wisdom", IconKey = CategoryDefaults.Icons["wisdom"], ColorHex = CategoryDefaults.Colors[3] },
            new Category { Id = Guid.NewGuid(), Name = "Energy", IconKey = CategoryDefaults.Icons["energy"], ColorHex = CategoryDefaults.Colors[4] },
            new Category { Id = Guid.NewGuid(), Name = "Career", IconKey = CategoryDefaults.Icons["career"], ColorHex = CategoryDefaults.Colors[5] },
            new Category { Id = Guid.NewGuid(), Name = "Wealth", IconKey = CategoryDefaults.Icons["wealth"], ColorHex = CategoryDefaults.Colors[6] },
            new Category { Id = Guid.NewGuid(), Name = "Discipline", IconKey = CategoryDefaults.Icons["discipline"], ColorHex = CategoryDefaults.Colors[7] },
            new Category { Id = Guid.NewGuid(), Name = "Love", IconKey = CategoryDefaults.Icons["love"], ColorHex = CategoryDefaults.Colors[8] },
            new Category { Id = Guid.NewGuid(), Name = "Peace", IconKey = CategoryDefaults.Icons["peace"], ColorHex = CategoryDefaults.Colors[9] },
            new Category { Id = Guid.NewGuid(), Name = "Gratitude", IconKey = CategoryDefaults.Icons["gratitude"], ColorHex = CategoryDefaults.Colors[10] },
            new Category { Id = Guid.NewGuid(), Name = "Happiness", IconKey = CategoryDefaults.Icons["happiness"], ColorHex = CategoryDefaults.Colors[11] },
            new Category { Id = Guid.NewGuid(), Name = "Resilience", IconKey = CategoryDefaults.Icons["resilience"], ColorHex = CategoryDefaults.Colors[12] },
            new Category { Id = Guid.NewGuid(), Name = "Courage", IconKey = CategoryDefaults.Icons["courage"], ColorHex = CategoryDefaults.Colors[13] },
            new Category { Id = Guid.NewGuid(), Name = "Fitness", IconKey = CategoryDefaults.Icons["fitness"], ColorHex = CategoryDefaults.Colors[14] },
            new Category { Id = Guid.NewGuid(), Name = "Hope", IconKey = CategoryDefaults.Icons["hope"], ColorHex = CategoryDefaults.Colors[15] }
        );

        // One-to-Many: One Category has Many Quotes
        modelBuilder.Entity<Quote>()
            .HasOne(q => q.Category)
            .WithMany() // Or .WithMany(c => c.Quotes) if you add a list to Category
            .HasForeignKey(q => q.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ensure IsFavorite defaults to false
        modelBuilder.Entity<UserQuote>()
            .Property(b => b.IsFavorite)
            .HasDefaultValue(false);

        // Relationship: If a system quote is deleted, keep the user record but null the reference
        modelBuilder.Entity<UserQuote>()
            .HasOne(uq => uq.Quote)
            .WithMany()
            .HasForeignKey(uq => uq.QuoteId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure One-to-Many for Schedule -> Days
        modelBuilder.Entity<QuoteSchedule>()
            .HasMany(s => s.Days)
            .WithOne()
            .HasForeignKey(d => d.ScheduleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.ToTable("ErrorLogs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Message).IsRequired();
        });
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Quote> Quotes { get; set; }
    public DbSet<UserQuote> UserQuotes { get; set; }
    // New tables for scheduling
    public DbSet<QuoteSchedule> QuoteSchedules { get; set; }
    public DbSet<ScheduledDay> ScheduledDays { get; set; }
    public DbSet<ErrorLog> ErrorLogs { get; set; }
}