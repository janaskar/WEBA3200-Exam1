using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PowerCards.Models
{
    public class Favorite
    {
        [Key, Column(Order = 0)]
        [ForeignKey("User")]
        public string? UserName { get; set; }
        [Key, Column(Order = 1)]
        [ForeignKey("Deck")]
        public int DeckID { get; set; }
        [BindNever]
        public virtual User? User { get; set; }
        [BindNever]
        public virtual Deck? Deck { get; set; }
    }
}