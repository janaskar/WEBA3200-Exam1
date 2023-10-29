using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerCards.DAL.Interfaces;
using PowerCards.Models;

namespace PowerCards.Controllers
{
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
        [Authorize]
        public async Task<IActionResult> Create(Favorite favorite)
        {
            // Check if the model is valid
            if (ModelState.IsValid)
            {
                // Check if the favorite already exists
                var existingFavorite = await _favoriteRepository.GetByCompositeId(favorite.UserName, favorite.DeckID);

                // If the favorite does not exist, create it
                if (existingFavorite == null)
                {
                    // Create the favorite
                    await _favoriteRepository.Create(favorite);
                }
            }
            // Redirect to the deck details page
            return RedirectToAction("Details", "Decks", new { id = favorite.DeckID });
        }

        // POST: Favorites Delete
        [HttpPost, ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string id0, int id1)
        {
            // Delete the favorite
            bool isDeleted = await _favoriteRepository.DeleteConfirmed(id0, id1);
            // Check if the favorite was deleted
            if (!isDeleted)
            {
                // If the favorite was not deleted, return not found
                //error message
                return NotFound();
            }
            // Redirect to the deck details page
            return RedirectToAction("Details", "Decks", new { id = id1 });
        }
    }
}