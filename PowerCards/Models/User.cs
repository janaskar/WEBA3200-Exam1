using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PowerCards.Models
{
    public class User : IdentityUser
    {
        [Key]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be at least 3 and at max 50 characters long.")]
        public override string? UserName { get; set; }
        public virtual List<Deck>? Decks { get; set; }
        public virtual List<Favorite>? Favorites { get; set; }
    }
}