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
            // Retrieve all decks from the database 
            var decks = await _deckRepository.GetAll();
            return View(decks);
        }



        // GET: Decks Details. This is not a part of CRUD Thats why it stays here
        public async Task<IActionResult> Details(int? id)
        {
            // Check if the id is null
            if (id == null)
            {
                return NotFound();
            }
            // Retrieve the deck to be displayed
            var deck = await _deckRepository.GetById((int)id);
            // Check if the deck exists
            if (deck == null)
            {
                // If the deck does not exist, return not found
                return NotFound();
            }

            // Create a view model to hold the deck and the card
            var viewModel = new DeckViewModel
            {
                // Set the deck to the deck retrieved from the database
                Deck = deck,
                // Create a new card with the deck id
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
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Create the deck
                await _deckRepository.Create(deck);
                // Redirect to the deck index view
                return RedirectToAction(nameof(Index));
            }
            return View(deck);
        }

        // GET: Decks Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if the id is null
            if (id == null)
            {
                // If the id is null, return not found
                return NotFound();
            }
            // Retrieve the deck to be edited
            var deck = await _deckRepository.GetById((int)id);
            // Check if the deck exists
            if (deck == null)
            {
                // If the deck does not exist, return not found
                return NotFound();
            }
            return View(deck);
        }

        // POST: Decks Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeckID,UserName,Title,Description,Subject")] Deck deck)
        {
            // Check if the id of the deck to be edited matches the id of the deck in the model
            if (id != deck.DeckID)
            {
                return NotFound();
            }
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Edit the deck
                    await _deckRepository.Edit(deck);
                }
                // Check if the deck exists
                catch (DbUpdateConcurrencyException)
                {
                    // If the deck does not exist, return not found
                    if (!(await _deckRepository.GetById(deck.DeckID) is not null))
                    {
                        return NotFound();
                    }
                    // If the deck exists, throw the exception
                    else
                    {
                        throw;
                    }
                }
                // Redirect to the deck details view
                return RedirectToAction("Details", "Decks", new { id = deck.DeckID });
            }
            // If the model state is not valid, return the deck
            return View(deck);
        }


        // GET: Decks Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            // Check if the id is null
            if (id == null)
            {
                return NotFound();
            }
            // Retrieve the deck to be deleted
            var deck = await _deckRepository.GetById((int)id);
            // Check if the deck exists
            if (deck == null)
            {
                // If the deck does not exist, return not found 
                return NotFound();
            }
            // Return the deck to be deleted
            return View(deck);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Retrieve the deck to be deleted
            bool success = await _deckRepository.Delete(id);
            // Check if the deck exists
            if (!success)
            {
                // If the deck does not exist, return not found
                // Til Jay: Vi kan adde errorhandling der notFound er
                return NotFound();
            }
            // Redirect to the deck index view
            return RedirectToAction(nameof(Index));
        }


        // Utility method to check if a dard exists in the database
        private async Task<bool> DeckExists(int id)
        {
            // Check if the deck exists by id
            return (await _deckRepository.GetById(id)) != null;
        }
    }
}