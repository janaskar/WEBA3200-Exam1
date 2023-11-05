using PowerCards.DAL.Enum;
using System.ComponentModel.DataAnnotations;

namespace PowerCards.ViewModels.Decks
{
    public class CreateDeckViewModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, ErrorMessage = "Title length cannot exceed 50 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Description length cannot exceed 250 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        public Subject? Subject { get; set; }
    }
}