using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PowerCards.DAL.Enum;
using PowerCards.Models;

namespace PowerCards.DAL
{
    public class DbInit
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var servicesScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = servicesScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();

                // User seeding
                if (!context.Users.Any())
                {
                    context.Users.Add(new User
                    {
                        UserName = "_NotBarack"
                    });
                    context.SaveChanges();
                }

                int newDeckId = 0; // To store the ID of the newly added deck

                // Deck seeding
                if (!context.Decks.Any())
                {
                    var deck = new Deck
                    {
                        UserName = "_NotBarack",
                        Title = "Learn about Barack Obama",
                        Description = "The 44th President",
                        Subject = Subject.Other
                    };
                    context.Decks.Add(deck);
                    context.SaveChanges();
                    newDeckId = deck.DeckID;  // Retrieve the ID after saving the changes
                }

                // Card seeding
                if (!context.Cards.Any())
                {
                    // First card
                    context.Cards.Add(new Card
                    {
                        Question = "What year was Barack Obama born?",
                        Answer = "1961",
                        DeckID = newDeckId  // Use the retrieved deck ID
                    });

                    // Second card
                    context.Cards.Add(new Card
                    {
                        Question = "Which years did Barack Obama serve as president?",
                        Answer = "2009-2017",
                        DeckID = newDeckId
                    });

                    // Third card
                    context.Cards.Add(new Card
                    {
                        Question = "Which state did Barack Obama represent in the U.S. Senate?",
                        Answer = "Illinois",
                        DeckID = newDeckId
                    });

                    context.Cards.Add(new Card
                    {
                        Question = "What is Barack Obama's Last Name?",
                        Answer = "Obama",
                        DeckID = newDeckId
                    });
                    context.Cards.Add(new Card
                    {
                        Question = "What is Barack Obama's First Name?",
                        Answer = "Barack",
                        DeckID = newDeckId
                    });
                    context.Cards.Add(new Card
                    {
                        Question = "What is Barack Obama's favorite color?",
                        Answer = "Blue",
                        DeckID = newDeckId
                    });

                    context.SaveChanges();

                }

                // Favorite seeding
                if (!context.Favorites.Any())
                {
                    context.Favorites.Add(new Favorite
                    {
                        UserName = "_NotBarack",
                        DeckID = newDeckId  // Use the retrieved deck ID
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
