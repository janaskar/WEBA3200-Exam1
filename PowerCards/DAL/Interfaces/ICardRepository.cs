
using PowerCards.Models;

namespace PowerCards.DAL.Interfaces
{
    public interface ICardRepository
    {
        Task<IEnumerable<Card>> GetAll();
        Task<Card?> GetById(int id);
        Task Create(Card card);
        Task Edit(Card card);
        Task<bool> Delete(int id);
        Task<bool> CardExists(int id);
    }
}
