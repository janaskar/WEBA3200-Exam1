using PowerCards.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PowerCards.DAL.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<IEnumerable<Favorite>?> GetAll();
        //Since both userName and deckId are primary keys, we can use them to get a single favorite
        Task<Favorite?> GetByCompositeId(string UserName, int DeckID);
        Task<bool> Create(Favorite favorite);
        Task<bool> DeleteConfirmed(string UserName, int deckID);
    }
}
