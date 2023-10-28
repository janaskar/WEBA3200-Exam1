using Microsoft.EntityFrameworkCore;
using PowerCards.Models;
using PowerCards.DAL.Interfaces;
namespace PowerCards.DAL.Repositories
{
    public class FavoriteRepository
    {
        public class CardRepository : ICardRepository
        {
            private readonly AppDbContext _db;
            public CardRepository(AppDbContext db)
            {
                _db = db;
            }
        }
}
