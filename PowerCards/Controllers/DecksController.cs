using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerCards.DAL.Interfaces;
using PowerCards.ViewModels;
using PowerCards.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace PowerCards.Controllers
{
    // add the Authorize attribute to the class with loginpath to be /identity/account/login
    [Authorize]
    public class DecksController : Controller
    {
        private readonly IDeckRepository _deckRepository;
        private readonly ILogger<DecksController> _logger;

        public DecksController(IDeckRepository deckRepository, ILogger<DecksController> logger)
        {
            _deckRepository = deckRepository;
            _logger = logger;
        }

        [AllowAnonymous]
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
        [AllowAnonymous]
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Deck deck)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Create the deck
                deck.UserName = User.Identity.Name;
                bool success = await _deckRepository.Create(deck);

                // Redirect to the deck index page
                if(success)
                    return RedirectToAction(nameof(Index));
            }
            return View(deck);
        }

        [HttpGet]
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
        public async Task<IActionResult> Edit(Deck deck)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Get the deck id from the route
                var id = Convert.ToInt32(RouteData.Values["id"]);
                var DeckEdit = await _deckRepository.GetById(id);

                // Check if the current user is the owner of the deck
                if (DeckEdit.UserName != User.Identity.Name)
                {
                    _logger.LogError("[DecksController] This Deck does not belong to the current user");
                    return BadRequest("Deck does not belong to the current user");
                }

                // Update the deck
                DeckEdit.Title = deck.Title;
                DeckEdit.Description = deck.Description;
                DeckEdit.Subject = deck.Subject;
                bool success = await _deckRepository.Edit(DeckEdit);

                // Redirect to the deck index page
                if (success)
                    return RedirectToAction(nameof(Index));
            }
            return View(deck);
        }

        [HttpGet]
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
        public async Task<IActionResult> DeleteConfirmed()
        {
            // Get the deck id from the route
            var id = Convert.ToInt32(RouteData.Values["id"]);
            var DeckDelete = await _deckRepository.GetById(id);

            // If not, return bad request
            if (DeckDelete == null)
            {
                _logger.LogError("[DecksController] Deck not found for the DeckID {DeckID: 0000}", id);
                return BadRequest("Deck not found for the DeckID");
            }

            // Check if the current user is the owner of the deck
            if (DeckDelete.UserName != User.Identity.Name)
            {
                _logger.LogError("[DecksController] This Deck does not belong to the current user");
                return BadRequest("Deck does not belong to the current user");
            }

            // Delete the deck
            var success = await _deckRepository.Delete(id);

            // Redirect to the deck index page
            if (success)
                return RedirectToAction(nameof(Index));

            _logger.LogError("[DecksController] Failed to delete deck with DeckID: {DeckID}", id);
            return NotFound();
        }
    }
}