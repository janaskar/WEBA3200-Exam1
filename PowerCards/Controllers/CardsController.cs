using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PowerCards.DAL;
using PowerCards.Models;
using PowerCards.ViewModels;

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

        // GET: Go Back to Deck Details
        public IActionResult DeckDetails(Card card)
        {
            int deckId = card.DeckID;
            return RedirectToAction("Details", "Decks", new { id = deckId });
        }

        // GET: Card Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cards == null)
            {
                return NotFound();
            }

            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            return View(card);
        }

        // POST: Card Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeckID,CardID,Question,Answer,Hint")] Card card)
        {
            if (id != card.CardID)
            {
                return NotFound();
            }

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
            return View(card);
        }

        // POST: Card Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cards == null)
            {
                return Problem("Entity set 'AppDbContext.Cards'  is null.");
            }

            var card = await _context.Cards.FindAsync(id);
            int? deckId = card?.DeckID;
            if (card != null)
            {
                _context.Cards.Remove(card);
                await _context.SaveChangesAsync();
            }

            if (deckId.HasValue)
            {
                return RedirectToAction("Details", "Decks", new { id = deckId });
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Card Create from Deck details view
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

        // Utility method to check if a card exists in the database
        private bool CardExists(int id)
        {
            return (_context.Cards?.Any(e => e.CardID == id)).GetValueOrDefault();
        }
    }
}