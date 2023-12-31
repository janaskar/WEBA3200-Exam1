#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PowerCards.DAL;
using PowerCards.Models;

namespace PowerCards.Areas.Identity.Pages.Account.Manage
{
    public class MyDecksModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public IList<Deck> UserDecks { get; set; }

        public MyDecksModel
        (
            AppDbContext context,
            UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Get the current user
            var userName = _userManager.GetUserName(User);
            // Get the user decks
            UserDecks = await _context.Decks
                // Filter by the current user
                          .Where(d => d.UserName == userName)
                          .ToListAsync();
            return Page();
        }
    }
}