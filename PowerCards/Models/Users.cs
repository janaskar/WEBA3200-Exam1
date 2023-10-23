using System.ComponentModel.DataAnnotations;

namespace PowerCards.Models
{
    public class User
    {
        [Key]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }
    }

}
