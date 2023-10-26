using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PowerCards.Models
{
    public class User : IdentityUser
    {
        [Key]
        public override string UserName { get; set; }
        public virtual List<Deck>? Decks { get; set; }
        public virtual List<Favorite>? Favorites { get; set; }
    }
}