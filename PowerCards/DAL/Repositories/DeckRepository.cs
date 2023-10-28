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
            //return all the decks from the database
            return await _db.Decks.ToListAsync();
        }
        public async Task<Deck?> GetById(int id)
        {
            //find the deck by id
            return await _db.Decks.FindAsync(id);
        }
        public async Task Create(Deck deck)
        {
            //add the deck to the database and save the changes
            _db.Decks.Add(deck);
            await _db.SaveChangesAsync();
        }
        public async Task Edit(Deck deck)
        {
            //update the deck in the database and save the changes
            _db.Update(deck);
            await _db.SaveChangesAsync();
        }
        public async Task<bool> Delete(int id)
        {
            //retrieve the deck from the database
            var deck = await _db.Decks.FindAsync(id);
            //if the deck is null, return false
            if (deck == null)
            {
                return false;
            }
            //otherwise, remove the deck from the database and save the changes
            _db.Decks.Remove(deck);
            await _db.SaveChangesAsync();
            return true;
        }

       
    }
}
