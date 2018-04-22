using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveLetter
{
    public class Player
    {
        //Attribute
        public String name;
        public int nbMarker;
        public List<Card> player_Deck;
        public List<Card> deadCard;
        public Boolean HMProtected;
        public Boolean isHuman;
        public int indexPlayer;
        public AI ai;
        public Boolean isActive = true;
        //Methods
        //Constructor
        public Player() { }

        public Player(String name) 
        {
            this.name = name;
            this.nbMarker = 0;
            this.player_Deck = new List<Card>();
            this.deadCard = new List<Card>();
            this.HMProtected = false;
            this.isHuman = true; // human player default
        }

        public Player(String name, Boolean isH, int index)
        {
            this.name = name;
            this.nbMarker = 0;
            this.player_Deck = new List<Card>();
            this.deadCard = new List<Card>();
            this.HMProtected = false;
            this.isHuman = isH;
            this.indexPlayer = index;
            if (!isHuman)
            {
                this.ai = new LoveLetter.AI(this); 
            }
        }

        /*public Player(String name, int nbMarker)
        {
            this.name = name;
            this.nbMarker = nbMarker;
            this.player_Deck = new List<Card>();
            this.deadCard = new List<Card>();
            this.HMProtected = false;
        }*/

        //get all the non protected player in the round
        public static List<Player> getCurrentNonProtectedPlayer()
        {
            List<Player> res = new List<Player>(Game.Instance.playersInCurrentRound);
            foreach (Player p in res.ToList())
            {
                if (p.HMProtected)
                {
                    res.Remove(p);
                }
            }
            return res;
        }


        public void PickCard(Card card)
        {            
            player_Deck.Add(card);
        }

        public void playCard(Card card)
        {
            deadCard.Add(card);
            GameController.lastDiscardedCard = card;
            player_Deck.Remove(card);
        }

        //Discard selected card
        public void discardCard()
        {
            this.player_Deck.Clear();
        }

        //Return the only card 1 player have
        public Card getCard()
        {
            return this.player_Deck.ElementAt(0);
        }

        public void clearCard()
        {
            this.player_Deck.Clear();
            this.deadCard.Clear(); 
        }

        public int getNbMark()
        {
            return nbMarker;
        }
        public List<Card> getDeadCard()
        {
            return this.deadCard;
        }

        public List<Card> getCards()
        {
            return this.player_Deck;
        }

        public int getSumOfDeadCard()
        {
            int res = 0;
            foreach (Card card in getDeadCard())
            {
                res += card.getValue();
            }
            return res;
        }

        public String getName()
        {
            return this.name;
        }

        public void increaseMarker()
        {
            nbMarker += 1;
        }

        public void deleteCard(Card c)
        {
            player_Deck.Remove(c);
        }

    }
}
