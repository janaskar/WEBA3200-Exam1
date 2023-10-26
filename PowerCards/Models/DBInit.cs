using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using PowerCards.DAL.Enum;
using PowerCards.DAL;

namespace PowerCards.Models
{
    public static class DBInit
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            AppDbContext context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            // Ensure the database is deleted and created
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Seed Users
            if (!context.Users.Any())
            {
                var users = new List<User>
                    {
                        new User { Username = "user1" },
                        new User { Username = "user2" }
                    };
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            // Seed Decks
            if (!context.Decks.Any())
            {
                var decks = new List<Deck>
                    {
                        new Deck
                        {
                            DeckID = 1,
                            Username = "user1",
                            Title = "Math Deck 1",
                            Description = "Description for Math Deck 1",
                            Subject = Subject.Math
                        },
                        new Deck
                        {
                            DeckID = 2,
                            Username = "user1",
                            Title = "Science Deck 1",
                            Description = "Description for Science Deck 1",
                            Subject = Subject.Science
                        },
                        // Add more decks
                    };
                context.Decks.AddRange(decks);
                context.SaveChanges();
            }

            // Seed Cards for Math Deck
            if (!context.Cards.Any(c => c.DeckID == 1))
            {
                var mathDeckCards = new List<Card>
                    {
                        new Card
                        {
                            DeckID = 1,
                            CardID = 1,
                            Question = "Question 1",
                            Answer = "Answer 1",
                            Hint = "Hint 1"
                        },
                        new Card
                        {
                            DeckID = 1,
                            CardID = 2,
                            Question = "Question 2",
                            Answer = "Answer 2",
                            Hint = "Hint 2"
                        },
                        new Card
                        {
                            DeckID = 1,
                            CardID = 3,
                            Question = "Question 3",
                            Answer = "Answer 3",
                            Hint = "Hint 3"
                        },
                        new Card
                        {
                            DeckID = 1,
                            CardID = 4,
                            Question = "Question 4",
                            Answer = "Answer 4",
                            Hint = "Hint 4"
                        },
                    };
                context.Cards.AddRange(mathDeckCards);
                context.SaveChanges();
            }

            // Seed Cards for Science Deck
            if (!context.Cards.Any(c => c.DeckID == 2))
            {
                var scienceDeckCards = new List<Card>
                    {
                        new Card
                        {
                            DeckID = 2,
                            CardID = 1,
                            Question = "Science Question 1",
                            Answer = "Science Answer 1"
                        },
                        new Card
                        {
                            DeckID = 2,
                            CardID = 2,
                            Question = "Science Question 2",
                            Answer = "Science Answer 2"
                        },
                        new Card
                        {
                            DeckID = 2,
                            CardID = 3,
                            Question = "Science Question 3",
                            Answer = "Science Answer 3"
                        },
                        new Card
                        {
                            DeckID = 2,
                            CardID = 4,
                            Question = "Science Question 4",
                            Answer = "Science Answer 4"
                        },
                    };
                context.Cards.AddRange(scienceDeckCards);
                context.SaveChanges();
            }

            // Seed Favorites
            if (!context.Favorites.Any())
            {
                var favorites = new List<Favorite>
                    {
                        new Favorite { Username = "user1", DeckID = 1 },
                        new Favorite { Username = "user2", DeckID = 2 },
                        // Add more favorites
                    };
                context.Favorites.AddRange(favorites);
                context.SaveChanges();
            }
        }
    }
}