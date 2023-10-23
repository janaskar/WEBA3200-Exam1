using System.ComponentModel.DataAnnotations;

namespace PowerCards.Models
{
    public class User
    {
        [Key]
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        public ICollection<Deck>? Decks { get; set; }
        public ICollection<Favorite>? Favorites { get; set;}
    }
}