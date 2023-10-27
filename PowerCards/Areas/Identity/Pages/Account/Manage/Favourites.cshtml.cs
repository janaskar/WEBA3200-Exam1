using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PowerCards.DAL;
using PowerCards.Models;

namespace PowerCards.Areas.Identity.Pages.Account.Manage
{
    public class MyFavouritesModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public IList<Favorite> UserFavourites{ get; set; }

        public MyFavouritesModel(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            UserFavourites = await _context.Favorites
                .Include(f => f.Deck)
                .Where(f => f.UserName == user.UserName)
                .ToListAsync();
            return Page();
        }
    }
}
