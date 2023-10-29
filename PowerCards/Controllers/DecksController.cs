using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerCards.DAL;
using PowerCards.Models;
using PowerCards.ViewModels;

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
            var decks = await _deckRepository.GetAll();
            if(decks == null)
            {
                _logger.LogError("[DeckController] Item list not found while executing _deckRepository.GetAll()");
                return NotFound("Deck ");
            }
            return View(decks);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                _logger.LogError("[DecksController] DeckID not found");
                return NotFound("DeckID not found");
            }

            var deck = await _deckRepository.GetById(id.Value);
            if (deck == null)
            {
                _logger.LogError("[DecksController] Deck not found while executing _deckRepository.GetAll()");
                return NotFound("Deck not found");
            }
            var viewModel = new DeckViewModel
            {
                Deck = deck,
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
            var deck = await _deckRepository.GetById(id);
            if(deck == null)
            {
                _logger.LogError("[DeckController] Deck not found when updating/editing in the DeckID {DeckID:0000", id);
                return BadRequest("Deck not found for the DeckID");
            }
            // Check if the current user is the owner of the deck
            var currUsername = User.Identity.Name;
            if (deck.UserName != currUsername)
            {
                // If not, return unauthorized
                return Unauthorized("You are not the owner");
            }   
            return View(deck);
            
           

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(Deck deck)
        {

            if (ModelState.IsValid)
            {
                bool success = await _deckRepository.Edit(deck);
                // If the deck was successfully edited, redirect to the deck details view
                if (success)
                    return RedirectToAction(nameof(Index));
            } 
            // If the model state is not valid, return to the deck details view
            _logger.LogWarning("[DecksController] Failed to edit deck with DeckID: {DeckID}", deck.DeckID);
            // Check if the current user is the owner of the deck
            var currUsername = User.Identity.Name;
            if (deck.UserName != currUsername)
            {
                // If not, return unauthorized
                return Unauthorized("You are not the owner");
            }
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
            var currUsername = User.Identity.Name;
            if (deck.UserName != currUsername)
            {
                // If not, return unauthorized
                return Unauthorized("You are not the owner");
            }
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
            // Check if the current user is the owner of the deck
            var currUsername = User.Identity.Name;
            if(DeckDelete.UserName != currUsername)
            {
                // If not, return unauthorized
                return Unauthorized("You are not the owner");
            }
            // If the deck exists, delete it
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
