using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
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
                var userManager = servicesScope.ServiceProvider.GetRequiredService<UserManager<User>>();

                context.Database.EnsureCreated();

                // User seeding
                if (!context.Users.Any())
                {
                    var user1 = new User
                    {
                        UserName = "_NotBarack",
                    };
                    userManager.CreateAsync(user1, "Password1");
                    var user2 = new User
                    {
                        UserName = "OrangeDonald",
                    };
                    userManager.CreateAsync(user2, "Password2");
                    var user3 = new User
                    {
                        UserName = "Nor",
                    };
                    userManager.CreateAsync(user3, "Password3");
                }

                // Deck seeding
                if (!context.Decks.Any())
                {
                    var deck1 = new Deck
                    {
                        UserName = "_NotBarack",
                        Title = "Learn about Barack Obama",
                        Description = "The 44th President",
                        Subject = Subject.History
                    };
                    context.Decks.Add(deck1);

                    var deck2 = new Deck
                    {
                        UserName = "OrangeDonald",
                        Title = "Learn about Donald Trump",
                        Description = "The 45th President",
                        Subject = Subject.History
                    };
                    context.Decks.Add(deck2);

                    var deck3 = new Deck
                    {
                        UserName = "Nor",
                        Title = "My Math Homework",
                        Description = "Math homework for the week",
                        Subject = Subject.Math
                    };
                    context.Decks.Add(deck3);

                    var deck4 = new Deck
                    {
                        UserName = "Nor",
                        Title = "My Science Homework",
                        Description = "Science homework for the week",
                        Subject = Subject.Science
                    };
                    context.Decks.Add(deck4);

                    var deck5 = new Deck
                    {
                        UserName = "_NotBarack",
                        Title = "Learn about US History",
                        Description = "The history of the United States",
                        Subject = Subject.History
                    };
                    context.Decks.Add(deck5);

                    var deck6 = new Deck
                    {
                        UserName = "OrangeDonald",
                        Title = "Coding 101 with Donald Trump",
                        Description = "Learn how to code with Donald Trump",
                        Subject = Subject.Coding
                    };
                    context.Decks.Add(deck6);

                    var deck7 = new Deck
                    {
                        UserName = "Nor",
                        Title = "Cool Websites",
                        Description = "Cool websites to check out",
                        Subject = Subject.Other
                    };
                    context.Decks.Add(deck7);

                    context.SaveChanges();

                    // Card seeding
                    if (!context.Cards.Any())
                    {
                        // Deck 1
                        context.Cards.Add(new Card
                        {
                            Question = "What year was Barack Obama born?",
                            Answer = "1961",
                            DeckID = deck1.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "Which years did Barack Obama serve as president?",
                            Answer = "2009-2017",
                            DeckID = deck1.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "Which state did Barack Obama represent in the U.S. Senate?",
                            Answer = "Illinois",
                            DeckID = deck1.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "What is Barack Obama's Last Name?",
                            Answer = "Obama",
                            DeckID = deck1.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "What is Barack Obama's First Name?",
                            Answer = "Barack",
                            DeckID = deck1.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "What is Barack Obama's favorite color?",
                            Answer = "Blue",
                            DeckID = deck1.DeckID
                        });

                        // Deck 2
                        context.Cards.Add(new Card
                        {
                            Question = "What year was Donald Trump born?",
                            Answer = "1946",
                            DeckID = deck2.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "Which years did Donald Trump serve as president?",
                            Answer = "2017-2021",
                            DeckID = deck2.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "Which state did Donald Trump represent in the U.S. Senate?",
                            Answer = "None",
                            DeckID = deck2.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "What is Donald Trump's Last Name?",
                            Answer = "Trump",
                            DeckID = deck2.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "What is Donald Trump's First Name?",
                            Answer = "Donald",
                            DeckID = deck2.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "What is Donald Trump's favorite color?",
                            Answer = "Orange",
                            DeckID = deck2.DeckID
                        });

                        // Deck 3
                        context.Cards.Add(new Card
                        {
                            Question = "What is 2 + 2?",
                            Answer = "4",
                            DeckID = deck3.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "What is 4 - 3?",
                            Answer = "1",
                            DeckID = deck3.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "What is 12 * 12?",
                            Answer = "144",
                            DeckID = deck3.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "What is 12 / 2?",
                            Answer = "6",
                            DeckID = deck3.DeckID
                        });

                        // Deck 4
                        context.Cards.Add(new Card
                        {
                            Question = "What is the scientific name for water?",
                            Answer = "H2O",
                            DeckID = deck4.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "What is the scientific name for salt?",
                            Answer = "NaCl",
                            DeckID = deck4.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "What is the powerhouse of the cell?",
                            Answer = "Mitochondria",
                            DeckID = deck4.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "What is the 6th planet from the sun?",
                            Answer = "Saturn",
                            DeckID = deck4.DeckID
                        });

                        // Deck 5
                        context.Cards.Add(new Card
                        {
                            Question = "What year was the United States founded?",
                            Answer = "1776",
                            DeckID = deck5.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "Which years did John Adams serve as president?",
                            Answer = "1797-1801",
                            DeckID = deck5.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "What is the national animal of the United States?",
                            Answer = "Bald Eagle",
                            DeckID = deck5.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "How many stars are on the United States flag?",
                            Answer = "50",
                            DeckID = deck5.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "How many stripes are on the United States flag?",
                            Answer = "13",
                            DeckID = deck5.DeckID
                        });

                        // Deck 6
                        context.Cards.Add(new Card
                        {
                            Question = "What is this programming language?",
                            Answer = "C#",
                            DeckID = deck6.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "What is an if statment?",
                            Answer = "A conditional statement that runs a block of code if a condition is true",
                            DeckID = deck6.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "What is a for loop?",
                            Answer = "A loop that runs a block of code a specified number of times",
                            DeckID = deck6.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "What is a while loop?",
                            Answer = "A loop that runs a block of code while a condition is true",
                            DeckID = deck6.DeckID
                        });

                        // Deck 7
                        context.Cards.Add(new Card
                        {
                            Question = "What is the biggest website?",
                            Answer = "Google",
                            DeckID = deck7.DeckID
                        });
                        context.Cards.Add(new Card
                        {
                            Question = "Random Face Generator",
                            Answer = "thispersondoesnotexist.com",
                            DeckID = deck7.DeckID
                        });
                        // Scale of the Universe
                        context.Cards.Add(new Card
                        {
                            Question = "Scale of the Universe",
                            Answer = "htwins.net/scale2",
                            DeckID = deck7.DeckID
                        });

                        context.SaveChanges();

                        // Favorite seeding
                        if (!context.Favorites.Any())
                        {
                            context.Favorites.Add(new Favorite
                            {
                                UserName = "_NotBarack",
                                DeckID = deck1.DeckID
                            });
                            context.Favorites.Add(new Favorite
                            {
                                UserName = "_NotBarack",
                                DeckID = deck2.DeckID
                            });
                            context.Favorites.Add(new Favorite
                            {
                                UserName = "OrangeDonald",
                                DeckID = deck3.DeckID
                            });
                            context.Favorites.Add(new Favorite
                            {
                                UserName = "Nor",
                                DeckID = deck3.DeckID
                            });
                            context.Favorites.Add(new Favorite
                            {
                                UserName = "Nor",
                                DeckID = deck4.DeckID
                            });
                            context.Favorites.Add(new Favorite
                            {
                                UserName = "_NotBarack",
                                DeckID = deck5.DeckID
                            });
                            context.Favorites.Add(new Favorite
                            {
                                UserName = "OrangeDonald",
                                DeckID = deck6.DeckID
                            });
                            context.Favorites.Add(new Favorite
                            {
                                UserName = "Nor",
                                DeckID = deck6.DeckID
                            });
                            context.Favorites.Add(new Favorite
                            {
                                UserName = "Nor",
                                DeckID = deck7.DeckID
                            });

                            context.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}