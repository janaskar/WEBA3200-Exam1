using Microsoft.EntityFrameworkCore;
using PowerCards.Models;
using PowerCards.DAL.Interfaces;

namespace PowerCards.DAL.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly AppDbContext _db;
        private readonly ILogger<CardRepository> _logger;
        public CardRepository(AppDbContext db, ILogger<CardRepository> Logger)
        {
            _db = db;
            _logger = Logger;
        }
        public async Task<IEnumerable<Card>> GetAll()
        {
            try
            {
                //return all cards from the database
                return await _db.Cards.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[ItemReposository] Card ToListAsync() failed when GetAll(), error message {e}", e.Message);
                return new List<Card>();
            }
           
        }
        public async Task<Card?> GetById(int id)
        {
           try
           {
                //return the card with the given id
                return await _db.Cards.FindAsync(id);
           }
           catch (Exception e)
           {
                _logger.LogError("[ItemReposository] Card FindAsync() failed when GetById(), error message {e}", e.Message);
                return null;
           }
        }
        public async Task<string> GetUserNameByDeckId(int id)
        {
            try
            {
                var deck = await _db.Decks.FindAsync(id);
                return deck.UserName;
            }
            catch (Exception e)
            {
                _logger.LogError("[ItemReposository] Card Add() failed when Create(), error message {e}", e.Message);
                return "not found";
            }
        }

        public async Task<bool> Create(Card card)
        {
            try
            {
                //add the card
                _db.Cards.Add(card);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[ItemReposository] Card Add() failed when Create(), error message {e}", e.Message);
                return false;
            }
        }
        public async Task<bool> Edit(Card card)
        {
            try
            {
                //update the card
                _db.Cards.Update(card);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[ItemReposository] Card Update() failed when Edit(), error message {e}", e.Message);
                return false;
            }
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                //find the card with the given id
                var card = await _db.Cards.FindAsync(id);
                if (card == null)
                {
                    _logger.LogError("[ItemReposository] Card FindAsync() failed when Delete(), error message {e}", "Card not found");
                    return false;
                }
                //remove the card
                _db.Cards.Remove(card);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[ItemReposository] Card Remove() failed when Delete(), error message {e}", e.Message);
                return false;
            }
        }

    }
}