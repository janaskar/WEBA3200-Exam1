using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerCards.DAL;
using PowerCards.Models;
using PowerCards.ViewModels;

namespace PowerCards.Controllers
{
    public class DecksController : Controller
    {
        // Dependency injection of the database context
        private readonly IDeckRepository _deckRepository;


        // Constructor to initialize context
        public DecksController(IDeckRepository deckRepository)
        {
            _deckRepository = deckRepository;
        }


        // GET: Decks Index
        public async Task<IActionResult> Index()
        {
            var decks = await _deckRepository.GetAll();
            return View(decks);
        }



        // GET: Decks Details. This is not a part of CRUD Thats why it stays here
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deck = await _deckRepository.GetById((int)id);
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
                await _deckRepository.Create(deck);
                return RedirectToAction(nameof(Index));
            }
            return View(deck);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deck = await _deckRepository.GetById((int)id);
            if (deck == null)
            {
                return NotFound();
            }
            return View(deck);
        }

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
                    await _deckRepository.Edit(deck);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await _deckRepository.GetById(deck.DeckID) is not null))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Decks", new { id = deck.DeckID });
            }
            return View(deck);
        }


        // GET: Decks Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deck = await _deckRepository.GetById((int)id);
            if (deck == null)
            {
                return NotFound();
            }
            return View(deck);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool success = await _deckRepository.Delete(id);
            if (!success)
            {
                return Problem("Error deleting the deck.");
            }
            return RedirectToAction(nameof(Index));
        }


        // Utility method to check if a dard exists in the database
        private async Task<bool> DeckExists(int id)
        {
            return (await _deckRepository.GetById(id)) != null;
        }
    }
}