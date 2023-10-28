using Microsoft.EntityFrameworkCore;
using PowerCards.Models;
using PowerCards.DAL.Interfaces;
namespace PowerCards.DAL.Repositories
{
    public class FavoriteRepository : IFavorite
    {
        private readonly AppDbContext _db;
        public FavoriteRepository(AppDbContext db)
        {
            _db = db;
        }
       
        public async Task<IEnumerable<Favorite>> GetAll()
        {
            //return all the favorites from the database
            return await _db.Favorites.ToListAsync();
        }
        public async Task<Favorite?> GetById(int id)
        {
            //find the favorite by id
            return await _db.Favorites.FindAsync(id);
        }
        public async Task Create(Favorite favorite)
        {
            //add the favorite to the database and save the changes
            _db.Favorites.Add(favorite);
            await _db.SaveChangesAsync();
        }
        public async Task<bool> Delete(int id)
        {
            //retrieve the favorite from the database
            var favorite = await _db.Favorites.FindAsync(id);
            //if the favorite is null, return false
            if (favorite == null)
            {
                return false;
            }
            //otherwise, remove the favorite from the database and save the changes
            _db.Favorites.Remove(favorite);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

