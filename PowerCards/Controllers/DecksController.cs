using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerCards.DAL.Interfaces;
using PowerCards.ViewModels;
using PowerCards.Models;

namespace PowerCards.Controllers
{
    public class DecksController : Controller
    {
        private readonly IDeckRepository _deckRepository;
        private readonly ILogger<DecksController> _logger;
        private readonly UserManager<User> _userManager;

        public DecksController(IDeckRepository deckRepository, ILogger<DecksController> logger,  UserManager<User> userManager)
        {
            _deckRepository = deckRepository;
            _logger = logger;
            _userManager = userManager;

        }

        public async Task<IActionResult> Index()
        {
            // Get all decks
            var decks = await _deckRepository.GetAll();
            // If the list is empty, return not found
            if(decks == null)
            {
                _logger.LogError("[DeckController] Item list not found while executing _deckRepository.GetAll()");
                return NotFound("Cannot find Decks");
            }
            // Get the current user
            return View(decks);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            // Check if the deck exists
            // Get the deck
            var deck = await _deckRepository.GetById(id.Value);
            if (deck == null)
            {
                // If the deck does not exist, return not found
                _logger.LogError("[DecksController] Deck not found while executing _deckRepository.GetAll()");
                return NotFound("Deck not found");
            }
            var viewModel = new DeckViewModel
            {
                // Get the cards of the deck
                Deck = deck,
                // Create a new card
                Card = new Card() { DeckID = deck.DeckID }
            };
            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Deck deck)
        {
            if (ModelState.IsValid)
            {
                // Create the deck
                bool success = await _deckRepository.Create(deck);
                if(success)
                    return RedirectToAction(nameof(Index));
            }
            _logger.LogWarning("[DecksController] Failed to create deck with DeckID: {DeckID}", deck.DeckID);
            return View(deck);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            // Check if the deck exists
            var deck = await _deckRepository.GetById(id);
            if (deck == null)
            {
                // If the deck does not exist, return not found
                _logger.LogError("[DecksController] Deck not found for the DeckID {DeckID: 0000}", id);
                return BadRequest("Deck not found for the DeckID");
            }
            return View(deck);



        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(Deck deck)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Update the deck
                bool success = await _deckRepository.Edit(deck);
                // If the deck was successfully edited, redirect to the deck details view
                if (success)
                {
                   return RedirectToAction(nameof(Index));
                }   
               
            }

            _logger.LogError("[DecksController] Edit() failed, error message {e}", "Deck not found");
            return View(deck);
        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deck = await _deckRepository.GetById(id);
            if(deck == null)
            {
                _logger.LogError("[DeckController] Deck not found for the DeckID {DeckID: 0000}", id);
                return BadRequest("Deck not found for the DeckID");
            }
            // Check if the current user is the owner of the deck
            return View(deck);

        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Check if the deck exists
            var DeckDelete = await _deckRepository.GetById(id);
            // If not, return bad request
            if (DeckDelete == null)
            {
                _logger.LogError("[DecksController] Deck not found for the DeckID {DeckID: 0000}", id);
                return BadRequest("Deck not found for the DeckID");
            }
            var success = await _deckRepository.Delete(id);
            if (!success)
            {
                // If the deck was not deleted, return not found
                _logger.LogError("[DecksController] Failed to delete deck with DeckID: {DeckID}", id);
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
