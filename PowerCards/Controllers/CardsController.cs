using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PowerCards.DAL;
using PowerCards.DAL.Interfaces;
using PowerCards.Models;
using PowerCards.ViewModels;

namespace PowerCards.Controllers
{
    public class CardsController : Controller
    {
        // Dependency injection of the Card Repository
        private readonly ICardRepository _cardRepository;
        private readonly ILogger<CardsController> _logger;

        // Constructor to initialize repository
        public CardsController(ICardRepository cardRepository, ILogger<CardsController> logger)
        {
            _cardRepository = cardRepository;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IActionResult DeckDetails(Card card)
        {
            try
            {
                // Retrieve the deck id from the card
                int deckId = card.DeckID;
                // Redirect to the deck details view
                return RedirectToAction("Details", "Decks", new { id = deckId });
            }
            catch(Exception e)
            {
                _logger.LogError("[CardsController] DeckDetails() failed, error message {e}", e.Message);
                return NotFound();
            }
           
        }
        // POST: Card Create from Deck details view
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateFromDeckDetails(Card card)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Create the card
                    await _cardRepository.Create(card);
                }
                // If the model state is not valid, return to the deck details view
                return RedirectToAction("Details", "Decks", new { id = card.DeckID });
            }
            catch(Exception e)
            {
                _logger.LogError("[CardsController] CreateFromDeckDetails() failed, error message {e}", e.Message);
                // Redirect to the deck details view
                return RedirectToAction("Details", "Decks", new { id = card.DeckID });
            }
               
            
           
        }
        // GET: Card Edit
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var card = await _cardRepository.GetById(id);
            if (card == null)
            {
                _logger.LogError("[CardsController] Edit() failed, error message {e}", "Card not found");
                return BadRequest("Card not fouind for the CardID");
            }
            return View(card);
        }


        // POST: Card Edit
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(Card card)
        {
            if (ModelState.IsValid)
            {
                bool success = await _cardRepository.Edit(card);
                if (success)
                {
                       return RedirectToAction("Details", "Decks", new { id = card.DeckID });
                }               
            }
            _logger.LogError("[CardsController] Edit() failed, error message {e}", "Card not found");
            return BadRequest("Card not fouind for the CardID");
        }


        // POST: Card Delete
        [HttpPost, ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Retrieve the card to be deleted
            var card = await _cardRepository.GetById(id);
            if (card == null)
            {
                _logger .LogError("[CardsController] DeleteConfirmed() failed, error message {e}", "Card not found");
                return NotFound();
            }

            bool isDeleted = await _cardRepository.Delete(id);
            if (!isDeleted)
            {

                return NotFound();
            }

            // Use the previously retrieved card's DeckID for redirection
            return RedirectToAction("Details", "Decks", new { id = card.DeckID });
        }
    }
}