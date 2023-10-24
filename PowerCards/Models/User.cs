using System.ComponentModel.DataAnnotations;

namespace PowerCards.Models
{
    public class User
    {
        [Key]
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        public virtual List<Deck>? Decks { get; set; }
        public virtual List<Favorite>? Favorites { get; set;}
    }
}