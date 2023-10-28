using Microsoft.EntityFrameworkCore;
using PowerCards.Models;
using PowerCards.DAL.Interfaces;

namespace PowerCards.DAL
{
    public class CardRepository : ICardRepository
    {
        private readonly AppDbContext _db;
        public CardRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Card>> GetAll()
        {
            //return all the cards from the database
            return await _db.Cards.ToListAsync();
        }
        public async Task<Card?> GetById(int id)
        {
            //find the card by id
            return await _db.Cards.FindAsync(id);
        }
        public async Task Create(Card card)
        {
            //add the card to the database and save the changes
            _db.Cards.Add(card);
            await _db.SaveChangesAsync();
        }
        public async Task<bool> Delete(int id)
        {
            //retrieve the card from the database
            var card = await _db.Cards.FindAsync(id);
            //if the card is null, return false
            if (card == null)
            {
                return false;
            }
            //otherwise, remove the card from the database and save the changes
            _db.Cards.Remove(card);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task Edit(Card card)
        {
            //update the card in the database and save the changes
            _db.Update(card);
            await _db.SaveChangesAsync();
        }
        public async Task<bool> CardExists(int id)
        {
            //return true if the card exists in the database
            return await _db.Cards.AnyAsync(e => e.CardID == id);
        }





    }
}
