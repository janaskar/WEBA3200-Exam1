using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PowerCards.DAL;
using PowerCards.Models;

namespace PowerCards.Controllers
{
    public class FavoritesController : Controller
    {
        // Dependency injection of the database context
        private readonly AppDbContext _context;

        // Constructor to initialize context
        public FavoritesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult YourAction()
        {
            // Assuming your _context is defined in your controller
            var favorite = _context.Favorites.FirstOrDefault(f => f.UserName == "SomeUser" && f.DeckID == 123);

            ViewBag.Context = _context;
            return View(favorite);
        }

        // GET: Favorites Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,DeckID")] Favorite favorite)
        {
            if (ModelState.IsValid)
            {
                var existingFavorite = await _context.Favorites
                    .FirstOrDefaultAsync(f => f.UserName == favorite.UserName && f.DeckID == favorite.DeckID);

                if (existingFavorite == null)
                {
                    _context.Add(favorite);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Details", "Decks", new { id = favorite.DeckID });
        }

        // POST: Favorites Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id0, int id1)
        {
            if (_context.Favorites == null)
            {
                return Problem("Entity set 'AppDbContext.Favorites'  is null.");
            }
            var favorite = await _context.Favorites.FindAsync(id0, id1);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Decks", new { id = id1 });
        }
    }
}
