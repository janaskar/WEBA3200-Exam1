using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerCards.DAL.Interfaces;
using PowerCards.DAL.Repositories;
using PowerCards.Models;

namespace PowerCards.Controllers
{
    public class CardsController : Controller
    {
        // Dependency injection of the Card Repository
        private readonly ICardRepository _cardRepository;
        private readonly ILogger<CardsController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor to initialize repository
        public CardsController(ICardRepository cardRepository, ILogger<CardsController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _cardRepository = cardRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        // POST: Card Create from Deck details view
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateFromDeckDetails(Card card)
        {
            card.DeckID = Convert.ToInt32(RouteData.Values["id"]);
            if (ModelState.IsValid)
            {
                // Create the card
                await _cardRepository.Create(card);
            }
            // If the model state is not valid, return to the deck details view
            return BadRequest($"'{card.DeckID}'");
            
            //return RedirectToAction("Details", "Decks", new { id = card.DeckID });
        }

        // GET: Card Edit
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var card = await _cardRepository.GetById(id);
            if (card == null)
            {
                _logger.LogError("[DeckController] Deck not found when updating/editing in the DeckID {DeckID:0000", id);
                return BadRequest("Deck not found for the DeckID");
            }
            return View(card);

        }

        // POST: Card Edit
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(Card card)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                var id = Convert.ToInt32(RouteData.Values["id"]);
                var CardEdit = await _cardRepository.GetById(id);
                string username = await _cardRepository.GetUserNameByDeckId(CardEdit.DeckID);
                if (username != _httpContextAccessor.HttpContext.User.Identity.Name)
                {
                    _logger.LogError("[CardsController] Edit() failed, error message {e}", "User is not authorized to edit this card");
                    return BadRequest("User is not authorized to edit this card");
                }

                CardEdit.Answer = card.Answer;
                CardEdit.Question = card.Question;
                bool success = await _cardRepository.Edit(CardEdit);
                // If the card was successfully edited, redirect to the deck details view
                if (success)
                {
                    return RedirectToAction("Details", "Decks", new { id = CardEdit.DeckID });
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
                _logger.LogError("[CardsController] DeleteConfirmed() failed, error message {e}", "Card not found");
                return NotFound();
            }

            // Delete the card using your card repository
            bool success = await _cardRepository.Delete(card.CardID);

            if (!success)
            {
                _logger.LogError("[CardsController] DeleteConfirmed() failed while deleting, error message {e}", "Failed to delete the card");
                return BadRequest("Failed to delete the card");
            }

            // Use the previously retrieved card's DeckID for redirection
            return RedirectToAction("Details", "Decks", new { id = card.DeckID });
        }
    }
}