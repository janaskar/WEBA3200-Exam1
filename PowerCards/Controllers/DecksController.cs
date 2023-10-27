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
    public class DecksController : Controller
    {
        // Dependency injection of the database context
        private readonly AppDbContext _context;

        // Constructor to initialize context
        public DecksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Decks Index
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Decks.Include(d => d.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Decks Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deck = await _context.Decks
                .Include(d => d.Cards)
                .FirstOrDefaultAsync(d => d.DeckID == id);

            if (deck == null)
            {
                return NotFound();
            }

            var viewModel = new DeckViewModel
            {
                Deck = deck,
                Card = new Card() { DeckID = deck.DeckID }
            };

            return View(viewModel);
        }

        // GET: Decks Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Decks Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeckID,UserName,Title,Description,Subject")] Deck deck)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deck);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deck);
        }

        // GET: Decks Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Decks == null)
            {
                return NotFound();
            }

            var deck = await _context.Decks.FindAsync(id);
            if (deck == null)
            {
                return NotFound();
            }
            return View(deck);
        }

        // POST: Decks Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeckID,UserName,Title,Description,Subject")] Deck deck)
        {
            if (id != deck.DeckID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deck);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeckExists(deck.DeckID))
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
            return View(deck);
        }

        // GET: Decks Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Decks == null)
            {
                return NotFound();
            }

            var deck = await _context.Decks
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.DeckID == id);
            if (deck == null)
            {
                return NotFound();
            }

            return View(deck);
        }

        // POST: Decks Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Decks == null)
            {
                return Problem("Entity set 'AppDbContext.Decks'  is null.");
            }
            var deck = await _context.Decks.FindAsync(id);
            if (deck != null)
            {
                _context.Decks.Remove(deck);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Utility method to check if a dard exists in the database
        private bool DeckExists(int id)
        {
          return (_context.Decks?.Any(e => e.DeckID == id)).GetValueOrDefault();
        }
     
    }
}