using Microsoft.EntityFrameworkCore;
using PowerCards.Models;
using PowerCards.DAL.Interfaces;

namespace PowerCards.DAL
{
    public class DeckRepository : IDeckRepository
    {
        private readonly AppDbContext _db;
        private readonly ILogger<DeckRepository> _logger;
        public DeckRepository(AppDbContext db, ILogger<DeckRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IEnumerable<Deck>?> GetAll()
        {
            try
            {
                //return all decks from the database
                return await _db.Decks.ToListAsync();
            }
            catch(Exception e)
            {
                _logger.LogError("[DeckRepository] GetAll() failed, error message: {e},", e.Message);
                return null;
            }   
        
        }
        public async Task<Deck?> GetById(int id)
        {
            try
            {
                //return the deck with the given id
                return await _db.Decks.FindAsync(id);
            }
            catch(Exception e)
            {
                _logger.LogError("[DeckRepository] deck FindAsync(id) failed when getting the DeckID {DeckID:0000}, error message: {e},", id, e.Message);
                return null;
            }
        }
        public async Task<bool> Create(Deck deck)
        {
            try
            {
                //add the deck
                _db.Decks.Add(deck);
                //save the changes
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[DeckRepository] deck FindAsync(id) failed when creating the DeckID {DeckID:0000}, error message: {e},", deck, e.Message);
                return false;
            }
        }
        public async Task<bool> Edit(Deck deck)
        {
            try
            {
                // Update the deck
                _db.Decks.Update(deck);
                // Save changes
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[DeckRepository] deck FindAsync(id) failed when editing the DeckID {DeckID:0000}, error message: {e},", deck, e.Message);
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                // Find the deck with the given id
                var deck = await _db.Decks.FindAsync(id);
                if (deck == null)
                {
                    _logger.LogError("[DeckRepository] deck FindAsync(id) failed when deleting the DeckID {DeckID:0000}" ,id);
                    return false;
                }
                // Remove the deck
                _db.Decks.Remove(deck);
                // Save changes
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[DeckRepository] deck FindAsync(id) failed when deleting the DeckID {DeckID:0000}, error message: {e},", id, e.Message);
                return false;
            }



      }
    }
}
