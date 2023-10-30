using PowerCards.Models;

namespace PowerCards.DAL.Interfaces
{
    public interface IDeckRepository
    {
        Task<IEnumerable<Deck>?> GetAll();
        Task<Deck?> GetById(int id);
        Task<bool> Create(Deck deck);
        Task<bool>Edit(Deck deck);
        Task<bool> Delete(int id);
    }
}