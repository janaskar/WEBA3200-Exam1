#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PowerCards.DAL;
using PowerCards.Models;

namespace PowerCards.Areas.Identity.Pages.Account.Manage
{
    public class FavouritesModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public IList<Deck> UserFavourites { get; set; }

        public FavouritesModel
        (
            AppDbContext context,
            UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userName = _userManager.GetUserName(User);
            UserFavourites = await _context.Decks
                          .Where(d => d.UserName == userName)
                          .ToListAsync();
            return Page();
        }
    }
}