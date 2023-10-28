using PowerCards.Models;
namespace PowerCards.DAL.Interfaces
{
    public interface IFavorite
    {
        Task<IEnumerable<Favorite>> GetAll();
        Task<Favorite?> GetById(int id);
        Task Create(Favorite favorite);
        Task<bool> Delete(int id);
    }
}
