using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PowerCards.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PowerCards.DAL
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasKey(u => new { u.UserName });
            modelBuilder.Entity<Deck>()
                .HasMany(i => i.Cards)
                .WithOne(c => c.Deck)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Favorite>()
                .HasKey(favorite => new { favorite.UserName, favorite.DeckID });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}