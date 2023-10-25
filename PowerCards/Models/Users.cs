using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PowerCards.Models
{
    public class User : IdentityUser
    {
        public virtual List<Deck>? Decks { get; set; }
        public virtual List<Favorite>? Favorites { get; set; }
    }
}