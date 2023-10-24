using Microsoft.EntityFrameworkCore;
using PowerCards.Models;

namespace PowerCards.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public override int SaveChanges()
        {
            // Before saving changes, automatically increment CardID for new Cards
            foreach (var entry in ChangeTracker.Entries<Card>())
            {
                if (entry.State == EntityState.Added)
                {
                    var newCard = entry.Entity;

                    // Find the maximum CardID for the given DeckID
                    var maxCardID = Cards
                        .Where(c => c.DeckID == newCard.DeckID)
                        .Select(c => (int?)c.CardID)
                        .Max();

                    // Increment CardID
                    newCard.CardID = (maxCardID ?? 0) + 1;
                }
            }
            return base.SaveChanges();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Deck>()
                .HasMany(i => i.Cards)
                .WithOne(c => c.Deck)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Card>()
                .HasKey(card => new { card.DeckID, card.CardID });
            modelBuilder.Entity<Favorite>()
                .HasKey(favorite => new { favorite.Username, favorite.DeckID });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}