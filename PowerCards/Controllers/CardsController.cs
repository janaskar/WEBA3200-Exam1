using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // Constructor to initialize repository
        public CardsController(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        // GET: Go Back to Deck Details
        public IActionResult DeckDetails(Card card)
        {
            // Retrieve the deck id from the card
            int deckId = card.DeckID;
            // Redirect to the deck details view
            return RedirectToAction("Details", "Decks", new { id = deckId });
        }

        // GET: Card Edit
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if the id is null
            if (id == null)
            {

                return NotFound();
            }
            // Retrieve the card to be edited
            var card = await _cardRepository.GetById(id.Value);
            // Check if the card exists
            if (card == null)
            {
                // If the card does not exist, return not found
                return NotFound();
            }
            return View(card);
        }


        // POST: Card Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeckID,CardID,Question,Answer,Hint")] Card card)
        {   
            // Check if the id of the card to be edited matches the id of the card in the model
            if (id != card.CardID)
            {
                return NotFound();
            }
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Edit the card
                    await _cardRepository.Edit(card);
                }
                catch (Exception)
                {
                    // Check if the card exists
                    if (!await _cardRepository.CardExists(card.CardID))
                        return NotFound();
                    // If the card exists, throw the exception
                    else
                        throw;
                }
                // Redirect to the deck details view
                return RedirectToAction("Details", "Decks", new { id = card.DeckID });
            }
            return View(card);
        }



        // POST: Card Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Retrieve the card to be deleted
            var card = await _cardRepository.GetById(id);
            if (card == null)
            {
                
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
        // POST: Card Create from Deck details view
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFromDeckDetails([Bind("DeckID,Question,Answer")] Card card)
        {
            if (ModelState.IsValid)
            {
                // Create the card
                await _cardRepository.Create(card);
                // Redirect to the deck details view
                return RedirectToAction("Details", "Decks", new { id = card.DeckID });
            }
            // If the model state is not valid, return to the deck details view
            return RedirectToAction("Details", "Decks", new { id = card.DeckID });
        }


        // Utility method to check if a card exists in the database
        private async Task<bool> CardExists(int id)
        {
            // Use the CardExists method from the Card Repository
            return await _cardRepository.CardExists(id);
        }
    }
}