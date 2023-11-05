using Microsoft.EntityFrameworkCore;
using PowerCards.Models;
using PowerCards.DAL.Interfaces;

namespace PowerCards.DAL.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _db;
        private readonly ILogger<FavoriteRepository> _logger;
        public FavoriteRepository(AppDbContext db, ILogger<FavoriteRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<IEnumerable<Favorite>?> GetAll()
        {
            try
            {
                //return all decks from the database
                return await _db.Favorites.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[FavoriteRepository] Favorite ToListAsync() failed when GetAll(), error message: {e}", e.Message);
                return null;
            }
        }
        //Get by composite id
        public async Task<Favorite?> GetByCompositeId(string UserName, int DeckID)
        {
            try
            {
                //return the deck with the given id (username and deckid)
                return await _db.Favorites.FindAsync(UserName, DeckID);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[FavoriteRepository] Favorite FindAsync() failed when GetByCompositeId(), error message: {e}", e.Message);
                return null;
            }
        }  
        public async Task<bool> Create(Favorite favorite)
        { 
            try
            {
                //add the favorite
                _db.Favorites.Add(favorite);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[FavoriteRepository] Favorite Add() failed when Create(), error message: {e}", e.Message);
                return false;
            }
        }
        public async Task<bool> DeleteConfirmed(string UserName, int DeckID)
        {
            try
            {
                //find the favorite with the given id
                var favorite = await GetByCompositeId(UserName, DeckID);
                if (favorite == null)
                {
                    _logger.LogError("[FavoriteRepository] Favorite not found for username: {userName} and deckID: {deckID}", UserName, DeckID);
                    return false;
                }
                //remove the favorite
                _db.Favorites.Remove(favorite);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[FavoriteRepository] Favorite Remove() failed, error message: {e}", e.Message);
                return false;
            }
        }
    }
}