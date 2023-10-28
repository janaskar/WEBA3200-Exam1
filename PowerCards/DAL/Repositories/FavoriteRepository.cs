using Microsoft.EntityFrameworkCore;
using PowerCards.Models;
using PowerCards.DAL.Interfaces;
namespace PowerCards.DAL.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _db;
        public FavoriteRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Favorite>> GetAll()
        {
            // Retrieve all favorites from the database 
            return await _db.Favorites.ToListAsync();
        }
        public async Task<Favorite?> GetByCompositeId(string? UserName, int deckId)
        {
            // Retrieve the favorite to be displayed
            return await _db.Favorites.FirstOrDefaultAsync(f => f.UserName == UserName && f.DeckID == deckId);
        }
        public async Task Create(Favorite favorite)
        {
            // Add the favorite to the database and save changes
            _db.Favorites.Add(favorite);
            await _db.SaveChangesAsync();
        }
        public async Task<bool> DeleteConfirmed(string UserName, int deckId)
        {
            // Retrieve the favorite to be deleted
            var favorite = await GetByCompositeId(UserName, deckId);
            // Check if the favorite exists
            if (favorite != null)
            {
                // If the favorite exists, delete it and save changes
                _db.Favorites.Remove(favorite);
                await _db.SaveChangesAsync();
                return true;
            }
            // If the favorite does not exist, return false
            return false;
        }
    }
}
