using PowerCards.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerCards.ViewModels.Cards
{
    public class CreateCardViewModel
    {
        public int DeckID { get; set; }

        [StringLength(250, ErrorMessage = "Question length cannot exceed 250 characters.")]
        public string? Question { get; set; }

        [StringLength(250, ErrorMessage = "Answer length cannot exceed 250 characters.")]
        public string? Answer { get; set; }
    }
}