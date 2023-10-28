using PowerCards.DAL;
using Microsoft.EntityFrameworkCore;
using PowerCards.DAL.Enum;

namespace PowerCards.Models
{
    public static class DBInit
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            AppDbContext context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
           // context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
          

            if (!context.Decks.Any())
            {
                var decks = new List<Deck>
                {
                    new Deck{
                        UserName = "NotBarack",
                        Title = "The 44th president", 
                        Description = "Learn about Barack Obama", 
                        Subject = Subject.Math, 
                        Cards = new List<Card>
                        {
                            new Card {Question = "What year was Barack Obama born?", Answer = "1961"},
                            new Card {Question = "What is Barack Obama's middle name?", Answer = "Hussein"},
                            new Card {Question = "What is Barack Obama's wife's name?", Answer = "Michelle"},
                            new Card {Question = "What is Barack Obama's Last Name?", Answer = "Obama"},
                            new Card {Question = "What is Barack Obama's first name?", Answer = "Barack"},
                            new Card {Question = "What is Barack Obama's favorite color?", Answer = "Blue"},
                            new Card {Question = "What is Barack Obama's favorite food?", Answer = "Pizza"},
                            new Card {Question = "What is Barack Obama's favorite animal?", Answer = "Dog"},
                        }
                        },

                };
                context.Decks.AddRange(decks);
                context.SaveChanges();
            }
            if (!context.Users.Any())
            {
                var users = new List<User>()
                {
                    new User{UserName = "NotBarack"},
                };
                context.Users.AddRange(users);
                context.SaveChanges();
            
            }

        }
    }
}
