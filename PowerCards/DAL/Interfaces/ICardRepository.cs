using PowerCards.Models;

namespace PowerCards.DAL.Interfaces
{
    public interface ICardRepository
    {
        Task<IEnumerable<Card>> GetAll();
        Task<Card?> GetById(int id);
        Task<bool>Create(Card card);
        Task<bool> Edit(Card card);
        Task<bool> Delete(int id);
    }
}