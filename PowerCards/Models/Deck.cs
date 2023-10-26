using PowerCards.DAL.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PowerCards.Models
{
    public class Deck
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeckID { get; set; }
        [ForeignKey("User")]
        public string? UserName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Subject? Subject { get; set; }
        public virtual User? User { get; set; }
        public virtual List<Card>? Cards { get; set; }
        public virtual List<Favorite>? Favorites { get; set; }
    }
}