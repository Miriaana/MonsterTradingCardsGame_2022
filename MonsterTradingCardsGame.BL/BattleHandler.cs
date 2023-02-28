using MTCGame.DAL;
using MTCGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGame.BL
{
    public class BattleHandler
    {
        public string StartBattle(string mtcgAuth)
        {
            var repo = new PostgreSQLRepository();

            //get auth
            string auth = repo.CheckAuthorization(mtcgAuth);
            if (string.IsNullOrEmpty(auth))
            {
                throw new Exception("401: Access token is missing or invalid");
            }

            //get deck
            List<Card> deck = repo.GetDeck(auth);
            if (deck.Count == 0)
            {
                throw new Exception("204: The deck doesn't have any cards");
            }
            
            //prepare user and cards (fill in element, type, etc)
            User player = new User(auth);
            player.Deck = deck; 
            //todo: fill in element/type
            foreach(Card card in player.Deck) 
            {
                card.FillTypes();
            }
            
            //join lobby and wait until battle is finished
            BattleLog battleLog = new Lobby().Join(player);

            //remove prev deckcards and add new ones to stack
            if(battleLog.Player.Username == player.Username) {
                player.Deck = battleLog.Player.Deck;
            }/*
            else if (battleLog.Player2.Username == player.Username)
            {
                player.Deck = battleLog.Player2.Deck;
            }*/
            else
            {
                throw new Exception("500: battlelog doesn't contain expected player in property Player");
            }
            repo.RemoveDeck(auth);
            repo.AcquireCards(auth, player.Deck);
            //send log
            return battleLog.LogString;
        }
    }
}
