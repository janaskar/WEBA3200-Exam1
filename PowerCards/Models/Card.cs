﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PowerCards.Models
{
    public class Card
    {
        [ForeignKey("Deck")]
        public int DeckID { get; set; }

        [Key]
        public int CardID { get; set; }

        [Required(ErrorMessage = "Question is required.")]
        [StringLength(250, ErrorMessage = "Question length cannot exceed 250 characters.")]
        public string? Question { get; set; }

        [Required(ErrorMessage = "Answer is required.")]
        [StringLength(1000, ErrorMessage = "Answer length cannot exceed 1000 characters.")]
        public string? Answer { get; set; }
        public virtual Deck? Deck { get; set; }
    }
}
