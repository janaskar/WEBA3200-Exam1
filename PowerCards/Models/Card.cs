using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PowerCards.Models
{
    public class Card
    {
        [ForeignKey("Deck")]
        public int DeckID { get; set; }
        [Key]
        public int CardID { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public virtual Deck? Deck { get; set; }
    }
}