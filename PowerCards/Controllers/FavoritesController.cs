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
        private readonly AppDbContext _context;

        public FavoritesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Favorites
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Favorites.Include(f => f.Deck).Include(f => f.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Favorites/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Favorites == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorites
                .Include(f => f.Deck)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.UserName == id);
            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // GET: Favorites/Create
        public IActionResult Create()
        {
            ViewData["DeckID"] = new SelectList(_context.Decks, "DeckID", "Title");
            ViewData["UserName"] = new SelectList(_context.Users, "UserName", "UserName");
            return View();
        }

        // POST: Favorites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,DeckID")] Favorite favorite)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favorite);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Decks", new { id = favorite.DeckID });
            }
            ViewData["DeckID"] = new SelectList(_context.Decks, "DeckID", "Title", favorite.DeckID);
            ViewData["UserName"] = new SelectList(_context.Users, "UserName", "UserName", favorite.UserName);
            return View();
        }

        // GET: Favorites/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Favorites == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorites.FindAsync(id);
            if (favorite == null)
            {
                return NotFound();
            }
            ViewData["DeckID"] = new SelectList(_context.Decks, "DeckID", "Title", favorite.DeckID);
            ViewData["UserName"] = new SelectList(_context.Users, "UserName", "UserName", favorite.UserName);
            return View(favorite);
        }

        // POST: Favorites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,DeckID")] Favorite favorite)
        {
            if (id != favorite.UserName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favorite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoriteExists(favorite.UserName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeckID"] = new SelectList(_context.Decks, "DeckID", "Title", favorite.DeckID);
            ViewData["UserName"] = new SelectList(_context.Users, "UserName", "UserName", favorite.UserName);
            return View(favorite);
        }

        // GET: Favorites/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Favorites == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorites
                .Include(f => f.Deck)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.UserName == id);
            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // POST: Favorites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Favorites == null)
            {
                return Problem("Entity set 'AppDbContext.Favorites'  is null.");
            }
            var favorite = await _context.Favorites.FindAsync(id);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
            
            await _context.SaveChangesAsync();
            
           return RedirectToAction("Details", "Decks", new { id = favorite.DeckID });
        }

        private bool FavoriteExists(string id)
        {
          return (_context.Favorites?.Any(e => e.UserName == id)).GetValueOrDefault();
        }
      
    }
}
