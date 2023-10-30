using PowerCards.DAL.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PowerCards.Models
{
    public class Deck
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeckID { get; set; }

        [ForeignKey("User")]
        [StringLength(100, ErrorMessage = "UserName length cannot exceed 100 characters.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title length cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description length cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        public Subject? Subject { get; set; }

        public virtual User? User { get; set; }

        public virtual List<Card>? Cards { get; set; }

        public virtual List<Favorite>? Favorites { get; set; }
    }
}
