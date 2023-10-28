using Microsoft.EntityFrameworkCore;
using PowerCards.Models;
using PowerCards.DAL.Interfaces;

namespace PowerCards.DAL
{
    public class CardRepository : ICardRepository
    {
        private readonly AppDbContext _db;
        public CardRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Card>> GetAll()
        {
            return await _db.Cards.ToListAsync();
        }
        public async Task<Card?> GetById(int id)
        {
            return await _db.Cards.FindAsync(id);
        }
        public async Task Create(Card card)
        {
            _db.Cards.Add(card);
            await _db.SaveChangesAsync();
        }
        public async Task Details(Card card)
        {
            _db.Cards.Add(card);
            await _db.SaveChangesAsync();
        }
        public async Task<bool> Delete(int id)
        {
            var card = await _db.Cards.FindAsync(id);
            if (card == null)
            {
                return false;
            }
            _db.Cards.Remove(card);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task Edit(Card card)
        {
            _db.Update(card);
            await _db.SaveChangesAsync();
        }



     

    }
}
