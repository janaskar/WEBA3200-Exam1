using PowerCards.DAL.Enum;
using PowerCards.Models;
using PowerCards.ViewModels.Cards;

namespace PowerCards.ViewModels.Decks
{
    public class DetailsDeckViewModel
    {
        public int DeckID { get; set; }
        public string? UserName { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Subject? Subject { get; set; }
        public virtual List<Card>? Cards { get; set; }
        public virtual List<Favorite>? Favorites { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public bool Check { get; set; }
    }
}