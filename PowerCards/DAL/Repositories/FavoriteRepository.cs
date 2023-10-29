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
                // Add the favorite
                _db.Favorites.Add(favorite);
                // Save the changes
                await _db.SaveChangesAsync();
                // Return true if the favorite was created
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[FavoriteRepository] Favorite Add() failed when Create(), error message: {e}", e.Message);
                // Return false if the favorite was not created
                return false;
            }
        }
        public async Task<bool> DeleteConfirmed(string UserName, int DeckID)
        {
            try
            {
                // Get the favorite
                var favorite = await GetByCompositeId(UserName, DeckID);
                if (favorite == null)
                {
                    _logger.LogError("[FavoriteRepository] Favorite not found for username: {userName} and deckID: {deckID}", UserName, DeckID);
                    return false;
                }
                // Remove the favorite
                _db.Favorites.Remove(favorite);
                // Save the changes
                await _db.SaveChangesAsync();
                // Return true if the favorite was deleted
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "[FavoriteRepository] Favorite Remove() failed, error message: {e}", e.Message);
                // Return false if the favorite was not deleted
                return false;
            }
        }
    }
}
