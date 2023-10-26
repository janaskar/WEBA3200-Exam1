using PowerCards.Models;

namespace PowerCards.ViewModels
{
    public class DeckViewModel
    {
        public User User { get; set; } = new User();
        public Deck Deck { get; set; } = new Deck();
        public Card Card { get; set; } = new Card();
    }
}
