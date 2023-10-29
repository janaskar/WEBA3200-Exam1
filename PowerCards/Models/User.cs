using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace PowerCards.Models
{
    public class User : IdentityUser
    {
        [Key]
        public override string UserName { get; set; }
        [BindNever]
        public virtual List<Deck>? Decks { get; set; }
        [BindNever]
        public virtual List<Favorite>? Favorites { get; set; }
    }

}