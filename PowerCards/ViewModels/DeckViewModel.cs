using PowerCards.Models;

namespace PowerCards.ViewModels
{
    public class DeckViewModel
    {
        public Deck Deck { get; set; } = new Deck();
        public Card Card { get; set; } = new Card();
    }
}