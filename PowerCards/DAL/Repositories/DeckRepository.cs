using Microsoft.EntityFrameworkCore;
using PowerCards.Models;
using PowerCards.DAL.Interfaces;

namespace PowerCards.DAL
{
    public class DeckRepository : IDeckRepository
    {
        private readonly AppDbContext _db;
        public DeckRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Deck>> GetAll()
        {
            return await _db.Decks.ToListAsync();
        }
        public async Task<Deck?> GetById(int id)
        {
            return await _db.Decks.FindAsync(id);
        }
        public async Task Create(Deck deck)
        {
            _db.Decks.Add(deck);
            await _db.SaveChangesAsync();
        }
        public async Task Edit(Deck deck)
        {
            _db.Update(deck);
            await _db.SaveChangesAsync();
        }
        public async Task<bool> Delete(int id)
        {
            var deck = await _db.Decks.FindAsync(id);
            if (deck == null)
            {
                return false;
            }
            _db.Decks.Remove(deck);
            await _db.SaveChangesAsync();
            return true;
        }

       
    }
}
