using PowerCards.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerCards.ViewModels.Cards
{
    public class EditCardViewModel
    {
        public int DeckID { get; set; }

        [Required(ErrorMessage = "Question is required.")]
        [StringLength(250, ErrorMessage = "Question length cannot exceed 250 characters.")]
        public string? Question { get; set; }

        [Required(ErrorMessage = "Answer is required.")]
        [StringLength(250, ErrorMessage = "Answer length cannot exceed 250 characters.")]
        public string? Answer { get; set; }
        public virtual Deck? Deck { get; set; }
    }
}