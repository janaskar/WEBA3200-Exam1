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
       //Get current user favorites
       private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public IList<Favorite> UserFavorites { get; set; }
        public IList<Deck> Decks { get; set; }

        public FavouritesModel(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userName = _userManager.GetUserName(User);
            UserFavorites = await _context.Favorites
                          .Where(f => f.UserName == userName)  
                          .Include(f => f.Deck)  
                          .ToListAsync();

            return Page();
        }


    }
}
