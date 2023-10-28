using Microsoft.EntityFrameworkCore;
using PowerCards.Models;
using PowerCards.DAL.Interfaces;

namespace PowerCards.DAL
{
    public class DeckRepository : IDeckRepository
    {
        private readonly AppDbContext _context;
        public DeckRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Deck>> GetAll()
        {
            return await _context.Decks.ToListAsync();
        }
        public async Task<Deck?> GetById(int id)
        {
            return await _context.Decks.FindAsync(id);
        }
        public async Task Create(Deck deck)
        {
            _context.Decks.Add(deck);
            await _context.SaveChangesAsync();
        }
        public async Task Edit(Deck deck)
        {
            _context.Update(deck);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> Delete(int id)
        {
            var deck = await _context.Decks.FindAsync(id);
            if (deck == null)
            {
                return false;
            }
            _context.Decks.Remove(deck);
            await _context.SaveChangesAsync();
            return true;
        }

       
    }
}
