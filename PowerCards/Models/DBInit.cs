using PowerCards.DAL;
using Microsoft.EntityFrameworkCore;
using PowerCards.DAL.Enum;
using Microsoft.AspNetCore.Builder;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using PowerCards.Models;
using System.Collections.Generic;

namespace PowerCards.Models
{
    public static class DBInit
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            AppDbContext context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Database.EnsureCreated();

            // Seed User
            if (!context.Users.Any())
            {
                User user = new User()
                {
                    UserName = "test"
                };

                context.Users.Add(user);

                // Seed Deck
                Deck deck = new Deck()
                {
                    UserName = user.UserName,
                    Title = "Sample Deck",
                    Description = "This is a sample deck description.",
                    Subject = Subject.Math,
                };

                context.Decks.Add(deck);

                // Seed Card
                Card card = new Card()
                {
                    DeckID = deck.DeckID,
                    Question = "Sample Question",
                    Answer = "Sample Answer",
                    Hint = "Sample Hint"
                };

                context.Cards.Add(card);

                // Seed Favorite
                Favorite favorite = new Favorite()
                {
                    UserName = user.UserName,
                    DeckID = deck.DeckID
                };

                context.Favorites.Add(favorite);

                context.SaveChanges();
            }
        }
    }
}
