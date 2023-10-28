using PowerCards.Models;
namespace PowerCards.DAL
{
    public interface IDeckRepository
    {
        Task<IEnumerable<Deck>> GetAll();
        Task<Deck?> GetById(int id);
        Task Create(Deck deck);
        Task Edit(Deck deck);
        Task<bool> Delete(int id);
    }
}
