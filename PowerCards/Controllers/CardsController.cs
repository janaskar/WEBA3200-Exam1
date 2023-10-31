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

        // Constructor to initialize repository
        public CardsController(ICardRepository cardRepository, ILogger<CardsController> logger)
        {
            _cardRepository = cardRepository;
            _logger = logger;
        }

        // POST: Card Create from Deck details view
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateFromDeckDetails(Card card)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Get the deck id from referer
                var referer = Request.Headers["Referer"].ToString();
                var deckid = Convert.ToInt32(referer.Substring(referer.LastIndexOf('/') + 1));

                // Get the username of the deck and check if it matches the current user
                string username = await _cardRepository.GetUserNameByDeckId(deckid);
                if (username != User.Identity.Name)
                {
                    _logger.LogError("[CardsController] CreateFromDeckDetails() failed, error message {e}", "User is not authorized to create this card");
                    return BadRequest("User is not authorized to create this card");
                }

                // Create a new card
                var CardCreate = new Card
                {
                    DeckID = deckid,
                    Question = card.Question,
                    Answer = card.Answer
                };
                bool success = await _cardRepository.Create(CardCreate);

                // Redirect to the deck details page
                if (success)
                    return RedirectToAction("Details", "Decks", new { id = deckid });
            }
            _logger.LogError("[CardsController] CreateFromDeckDetails() failed, error message {e}", "Card not found");
            return BadRequest("Card not found for the CardID");
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
                // Get the card id from the route
                var id = Convert.ToInt32(RouteData.Values["id"]);

                // Get the username of the deck and check if it matches the current user
                var CardEdit = await _cardRepository.GetById(id);
                string username = await _cardRepository.GetUserNameByDeckId(CardEdit.DeckID);
                if (username != User.Identity.Name)
                {
                    _logger.LogError("[CardsController] Edit() failed, error message {e}", "User is not authorized to edit this card");
                    return BadRequest("User is not authorized to edit this card");
                }

                // Update the card
                CardEdit.Question = card.Question;
                CardEdit.Answer = card.Answer;
                bool success = await _cardRepository.Edit(CardEdit);

                // Redirect to the deck details page
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

            // Get the deck id from referer
            var referer = Request.Headers["Referer"].ToString();
            var deckid = Convert.ToInt32(referer.Substring(referer.LastIndexOf('/') + 1));

            // If not, return bad request
            if (card == null)
            {
                _logger.LogError("[CardsController] DeleteConfirmed() failed, error message {e}", "Card not found");
                return BadRequest("Card not found for the CardID");
            }

            // Get the username of the deck and check if it matches the current user
            string username = await _cardRepository.GetUserNameByDeckId(deckid);
            if (username != User.Identity.Name)
            {
                _logger.LogError("[CardsController] CreateFromDeckDetails() failed, error message {e}", "User is not authorized to create this card");
                return BadRequest("User is not authorized to create this card");
            }

            // Delete the card using your card repository
            bool success = await _cardRepository.Delete(card.CardID);

            // Redirect to the deck details page
            if (success)
                return RedirectToAction("Details", "Decks", new { id = deckid });
            _logger.LogError("[CardsController] DeleteConfirmed() failed while deleting, error message {e}", "Failed to delete the card");
            return BadRequest("Failed to delete the card");
        }
    }
}