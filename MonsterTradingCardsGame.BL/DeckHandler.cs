using MTCGame.Model;
using MTCGame.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.BL
{
    public class DeckHandler
    {
        public List<Card> GetDeck(string mtcgAuth)
        {
            var repo = new PostgreSQLRepository();

            string auth = repo.CheckAuthorization(mtcgAuth);
            if (string.IsNullOrEmpty(auth))
            {
                throw new Exception("401: Access token is missing or invalid");
            }
            List<Card> deck = repo.GetDeck(auth);
            if (deck.Count == 0)
            {
                throw new Exception("204: The request was fine, but the deck doesn't have any cards");
            }
            return deck;
        }

        public void ConfigureDeck(string mtcgAuth, List<string> cardIds)
        {
            var repo = new PostgreSQLRepository();

            string auth = repo.CheckAuthorization(mtcgAuth);
            if (string.IsNullOrEmpty(auth))
            {
                throw new Exception("401: Access token is missing or invalid");
            }

            repo.ConfigureDeck(auth, cardIds);
        }
    }
}
