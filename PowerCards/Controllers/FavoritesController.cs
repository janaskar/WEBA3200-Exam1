using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerCards.DAL.Interfaces;
using PowerCards.Models;

namespace PowerCards.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        // Dependency injection of the Favorite Repository
        private readonly IFavoriteRepository _favoriteRepository;

        // Constructor to initialize repository
        public FavoritesController(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        // POST: Favorites Create
        [HttpPost]
        public async Task<IActionResult> Favorite()
        {
            // Get the deck id from referer
            var referer = Request.Headers["Referer"].ToString();
            var deckid = Convert.ToInt32(referer.Substring(referer.LastIndexOf('/') + 1));

            // Get the username of current user
            var username = User.Identity?.Name;

            var favorite = new Favorite
            {
                UserName = username,
                DeckID = deckid
            };

            // Check if the favorite already exists
            var existingFavorite = await _favoriteRepository.GetByCompositeId(favorite.UserName ?? "", favorite.DeckID);

            // If the favorite does not exist, create it
            if (existingFavorite == null)
            {
                // Create the favorite
                await _favoriteRepository.Create(favorite);
            }
            else
            {
                await _favoriteRepository.DeleteConfirmed(favorite.UserName ?? "", favorite.DeckID);
            }
            return RedirectToAction("Details", "Decks", new { id = deckid });
        }
    }
}