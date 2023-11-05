using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerCards.DAL.Interfaces;
using PowerCards.ViewModels.Decks;
using PowerCards.Models;
using PowerCards.ViewModels.Cards;

namespace PowerCards.Controllers
{
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

        // Get username of the current user
        public string? CurrentUserName => User.Identity?.Name;

        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchQuery)
        {
            // Get all decks
            var decks = await _deckRepository.GetAll();

            // If the list is empty, return not found
            if (decks == null)
            {
                _logger.LogError("Decks Index() not found");
                return NotFound();
            }

            // If a search query is provided, filter the decks based on the 'titel' and 'desciption' fields
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                decks = await _deckRepository.Search(searchQuery);
            }

            // Create a new deck view model
            var indexDeckViewModel = new IndexDeckViewModel
            {
                Decks = decks?.ToList() ?? new List<Deck>()
            };
            return View(indexDeckViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id, string? question, string? answer, bool check)
        {
            // Check if the deck exists
            var deck = await _deckRepository.GetById(id);
            if (deck == null)
            {
                // If the deck does not exist, return not found
                _logger.LogError("Deck Details() not found DeckID:{DeckID:0000}", id);
                return NotFound();
            }

            // Create a new deck view model
            var detailsDeckViewModel = new DetailsDeckViewModel
            {
                DeckID = deck?.DeckID ?? 0,
                UserName = deck?.UserName ?? "",
                Title = deck?.Title ?? "",
                Description = deck?.Description,
                Subject = deck?.Subject,
                Cards = deck?.Cards,
                Favorites = deck?.Favorites,
                Question = question,
                Answer = answer,
                Check = check
            };
            return View(detailsDeckViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDeckViewModel createDeckViewModel)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Create the deck
                var deck = new Deck
                {
                    UserName = CurrentUserName,
                    Title = createDeckViewModel.Title,
                    Description = createDeckViewModel.Description,
                    Subject = createDeckViewModel.Subject
                };
                bool success = await _deckRepository.Create(deck);

                // Redirect to the deck index page
                if (success)
                    return RedirectToAction(nameof(Index));
            }
            return View(createDeckViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Check if the deck exists
            var deck = await _deckRepository.GetById(id);
            if (deck == null)
            {
                // If the deck does not exist, return not found
                _logger.LogError("Deck Edit() not found DeckID:{DeckID:0000}.", id);
                return NotFound();
            }

            // Check if the current user is the owner of the deck
            if (deck?.UserName != CurrentUserName)
            {
                _logger.LogError("Deck Edit() does not belong to UserName:{UserName:} DeckID:{DeckID:}", CurrentUserName, id);
                return Forbid();
            }

            // Create a new deck view model
            var editDeckViewModel = new EditDeckViewModel
            {
                DeckID = deck?.DeckID ?? 0,
                Title = deck?.Title ?? "",
                Description = deck?.Description,
                Subject = deck?.Subject
            };
            return View(editDeckViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditDeckViewModel editDeckViewModel)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Get the deck id from the route
                var id = Convert.ToInt32(RouteData.Values["id"]);
                var DeckEdit = await _deckRepository.GetById(id);

                // If the deck does not exist, return not found
                if (DeckEdit == null)
                {
                    _logger.LogError("Deck Edit() not found DeckID:{DeckID:0000}", id);
                    return NotFound();
                }

                // Check if the current user is the owner of the deck
                if (DeckEdit.UserName != CurrentUserName)
                {
                    _logger.LogError("Deck Edit() does not belong to UserName:{UserName:} DeckID:{DeckID:}", CurrentUserName, id);
                    return Forbid();
                }

                // Update the deck
                DeckEdit.Title = editDeckViewModel.Title;
                DeckEdit.Description = editDeckViewModel.Description;
                DeckEdit.Subject = editDeckViewModel.Subject;
                bool success = await _deckRepository.Edit(DeckEdit);

                // Redirect to the deck index page
                if (success)
                    return RedirectToAction("Details", new { id = DeckEdit.DeckID });
            }
            return View(editDeckViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // Check if the deck exists
            var deck = await _deckRepository.GetById(id);
            if (deck == null)
            {
                _logger.LogError("Deck Delete() not found DeckID:{DeckID:0000}", id);
                return NotFound();
            }

            // Check if the current user is the owner of the deck
            if (deck?.UserName != CurrentUserName)
            {
                _logger.LogError("Deck Delete() does not belong to UserName:{UserName:} DeckID:{DeckID:}", CurrentUserName, id);
                return Forbid();
            }

            // Create a new deck view model
            var deleteDeckViewModel = new DeleteDeckViewModel
            {
                Title = deck?.Title ?? ""
            };
            return View(deleteDeckViewModel);
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
                _logger.LogError("Deck DeleteConfirmed() not found DeckID:{DeckID:0000}", id);
                return NotFound();
            }

            // Check if the current user is the owner of the deck
            if (DeckDelete.UserName != CurrentUserName)
            {
                _logger.LogError("Deck DeleteConfirmed() does not belong to UserName:{UserName:} DeckID:{DeckID:}", CurrentUserName, id);
                return Forbid();
            }

            // Delete the deck
            var success = await _deckRepository.Delete(id);

            // Redirect to the deck index page
            if (success)
                return RedirectToAction(nameof(Index));

            _logger.LogError("Deck DeleteConfirmed() could not delete DeckID:{DeckID:0000}", id);
            return Conflict();
        }
    }
}