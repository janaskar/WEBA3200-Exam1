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
        [MaxLength(50)]
        [ForeignKey("User")]
        public string? Username { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Subject? Subject { get; set; }
        public virtual User? User { get; set; }
        public ICollection<Card>? Cards { get; set; }
        public ICollection<Favorite>? Favorites { get; set; }
    }

}
