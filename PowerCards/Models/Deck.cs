using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PowerCards.DAL.Enum;

namespace PowerCards.Models
{
    public class Deck
    {
        [Key]
        public int DeckID { get; set; }

        [ForeignKey("User")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, ErrorMessage = "Title length cannot exceed 50 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Description length cannot exceed 250 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        public Subject? Subject { get; set; }

        public virtual User? User { get; set; }

        public virtual List<Card>? Cards { get; set; }

        public virtual List<Favorite>? Favorites { get; set; }
    }
}