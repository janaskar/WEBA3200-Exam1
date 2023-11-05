using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerCards.DAL.Interfaces;
using PowerCards.ViewModels.Cards;
using PowerCards.Models;

namespace PowerCards.Controllers
{
    [Authorize]
    public class CardsController : Controller
    {
        private readonly ICardRepository _cardRepository;
        private readonly ILogger<CardsController> _logger;

        public CardsController(ICardRepository cardRepository, ILogger<CardsController> logger)
        {
            _cardRepository = cardRepository;
            _logger = logger;
        }

        // Get username of the current user
        public string? CurrentUserName => User.Identity?.Name;

        public PartialViewResult CreateCardPartialView(int deckId, string? question, string? answer, bool check, string q)
        {
            // Create a new card view model
            var createCardViewModel = new CreateCardViewModel
            {
                DeckID = deckId,
                Question = question,
                Answer = answer,
            };

            // If check is true enable modelvalidation
            if (check)
            {
                if (createCardViewModel.Question == null)
                    ModelState.AddModelError("Question", "Question is required");
                if (createCardViewModel.Answer == null)
                    ModelState.AddModelError("Answer", "Answer is required");
            }
            return PartialView("_Create", createCardViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int deckId, Card card)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Get the username of the deck and check if it matches the current user
                string username = await _cardRepository.GetUserNameByDeckId(deckId);
                if (username != CurrentUserName)
                {
                    _logger.LogError("Card Create() does not belong to UserName:{UserName:} DeckID:{DeckID:}", CurrentUserName, deckId);
                    return Forbid();
                }

                // Create a new card
                var CardCreate = new Card
                {
                    DeckID = deckId,
                    Question = card.Question,
                    Answer = card.Answer
                };
                bool success = await _cardRepository.Create(CardCreate);

                // Redirect to the deck details page
                if (success)
                    return RedirectToAction("Details", "Decks", new { id = deckId });
            }
            return RedirectToAction("Details", "Decks", new { id = deckId, question = card?.Question, answer = card?.Answer, check = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Check if the card exists
            var card = await _cardRepository.GetById(id);
            if (card == null)
            {
                _logger.LogError("Card Edit() not found CardID:{CardID:0000}.", id);
                return NotFound();
            }

            // Check if the current user is the owner of the card
            string username = await _cardRepository.GetUserNameByDeckId(card.DeckID);
            if (username != CurrentUserName)
            {
                _logger.LogError("Card Edit() does not belong to UserName:{UserName:} DeckID:{DeckID:}", CurrentUserName, card.DeckID);
                return Forbid();
            }

            // Create a new card view model
            var editCardViewModel = new EditCardViewModel
            {
                DeckID = card.DeckID,
                Question = card.Question,
                Answer = card.Answer
            };
            return View(editCardViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCardViewModel editCardViewModel)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Get the card id from the route
                var id = Convert.ToInt32(RouteData.Values["id"]);
                var CardEdit = await _cardRepository.GetById(id);

                // If the card does not exist, return not found
                if (CardEdit == null)
                {
                    _logger.LogError("Card Edit() not found CardID:{CardID:0000}.", id);
                    return NotFound();
                }

                // Check if the current user is the owner of the card
                string username = await _cardRepository.GetUserNameByDeckId(CardEdit.DeckID);
                if (username != CurrentUserName)
                {
                    _logger.LogError("Card Edit() does not belong to UserName:{UserName:} DeckID:{DeckID:}", CurrentUserName, CardEdit.DeckID);
                    return Forbid();
                }

                // Update the card
                CardEdit.Question = editCardViewModel.Question;
                CardEdit.Answer = editCardViewModel.Answer;
                bool success = await _cardRepository.Edit(CardEdit);

                // Redirect to the deck details page
                if (success)
                    return RedirectToAction("Details", "Decks", new { id = CardEdit.DeckID });
            }
            return View(editCardViewModel);
        }

        // POST: Card Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, int deckId)
        {
            // Retrieve the card to be deleted
            var card = await _cardRepository.GetById(id);

            // If the card does not exist, return not found
            if (card == null)
            {
                _logger.LogError("Card Delete() not found CardID:{CardID:0000}.", id);
                return NotFound();
            }

            // Get the username of the deck and check if it matches the current user
            string username = await _cardRepository.GetUserNameByDeckId(deckId);
            if (username != CurrentUserName)
            {
                _logger.LogError("Card Delete() does not belong to UserName:{UserName:} DeckID:{DeckID:}", CurrentUserName, deckId);
                return Forbid();
            }

            // Delete the card using your card repository
            bool success = await _cardRepository.Delete(card.CardID);

            // Redirect to the deck details page
            if (success)
                return RedirectToAction("Details", "Decks", new { id = deckId });

            _logger.LogError("Card DeleteConfirmed() could not delete CardID:{CardID:0000}", id);
            return Conflict();
        }
    }
}