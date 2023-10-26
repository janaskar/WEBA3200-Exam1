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
    public class CardsController : Controller
    {
        // Dependency injection of the database context
        private readonly AppDbContext _context;

        // Constructor to initialize context
        public CardsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Display all cards
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Cards.Include(c => c.Deck);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Display details of a specific card
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cards == null)
                return NotFound();

            var card = await _context.Cards
                .Include(c => c.Deck)
                .FirstOrDefaultAsync(m => m.CardID == id);

            if (card == null)
                return NotFound();

            return View(card);
        }

        // GET: Render the card creation view
        public IActionResult Create()
        {
            ViewData["DeckID"] = new SelectList(_context.Decks, "DeckID", "Title");
            return View();
        }

        // POST: Handle card creation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeckID,Question,Answer")] Card card)
        {
            if (ModelState.IsValid)
            {
                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeckID"] = new SelectList(_context.Decks, "DeckID", "Title", card.DeckID);
            return View(card);
        }

        // GET: Render the edit view for a card
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cards == null)
                return NotFound();

            var card = await _context.Cards.FindAsync(id);

            if (card == null)
                return NotFound();

            ViewData["DeckID"] = new SelectList(_context.Decks, "DeckID", "Title", card.DeckID);
            return View(card);
        }

        // POST: Handle edits to a card
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeckID,CardID,Question,Answer,Hint")] Card card)
        {
            if (id != card.CardID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(card);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardExists(card.CardID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Details", "Decks", new { id = card.DeckID });
            }
            ViewData["DeckID"] = new SelectList(_context.Decks, "DeckID", "Title", card.DeckID);
            return View(card);
        }

        // GET: Confirm deletion of a card
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cards == null)
                return NotFound();

            var card = await _context.Cards
                .Include(c => c.Deck)
                .FirstOrDefaultAsync(m => m.CardID == id);

            if (card == null)
                return NotFound();

            return View(card);
        }

        // POST: Handle confirmed deletion of a card
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cards == null)
                return Problem("Entity set 'AppDbContext.Cards'  is null.");

            var card = await _context.Cards.FindAsync(id);
            int? deckId = card?.DeckID;
            if (card != null)
            {
                _context.Cards.Remove(card);
                await _context.SaveChangesAsync();
            }

            if (deckId.HasValue)
                return RedirectToAction("Details", "Decks", new { id = deckId });

            return RedirectToAction(nameof(Index));
        }

        // Utility method to check if a card exists in the database
        private bool CardExists(int id)
        {
            return (_context.Cards?.Any(e => e.CardID == id)).GetValueOrDefault();
        }

        // POST: Create card from deck details view
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFromDeckDetails([Bind("DeckID,Question,Answer")] Card card)
        {
            if (ModelState.IsValid)
            {
                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Decks", new { id = card.DeckID });
            }
            return RedirectToAction("Details", "Decks", new { id = card.DeckID });
        }
    }
}
