using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PowerCards.Models
{
    public class Card
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Deck")]
        public int DeckID { get; set; }
        [Key, Column(Order = 1)]
        public int CardID { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public string? Hint { get; set; }
        public virtual Deck? Deck { get; set; }
    }
}
