using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveLetter
{
    class Game
    {

        private static Game instance = null;
        private static readonly object padlock = new object();

        //Attribute
        public List<Card> play_DecTemp;
        public List<Player> players;
        public Stack<Card> play_Deck;
        public List<Player> playersInCurrentRound;
        public bool isFinished = false;

        //Methods
        Game()
        {
            this.play_DecTemp = new List<Card>();
            this.players = new List<Player>();
            this.playersInCurrentRound = new List<Player>();
        }


        //Acesseur
        public List<Player> getPlayer()
        {
            return this.players;
        }
        public List<Player> getPlayerInRound()
        {
            return this.playersInCurrentRound;
        }
        public Stack<Card> getCard()
        {
            return this.play_Deck;
        }

        //Singleton
        public static Game Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Game();
                    }
                    return instance;
                }
            }
        }

        public void initalizeCard()
        {

            //Create Guard
            for (int i = 0; i < 5; i++)
            {
                Card guard = new Card(1, "Guard", "this is a guard");
                play_DecTemp.Add(guard);
            }

            //Create Priest
            for (int i = 0; i < 2; i++)
            {
                Card priest = new Card(2, "Priest", "this is a priest");
                play_DecTemp.Add(priest);
            }

            //Create Baron
            for (int i = 0; i < 2; i++)
            {
                Card baron = new Card(3, "Baron", "this is a baron");
                play_DecTemp.Add(baron);
            }

            //Create Handmaiden
            for (int i = 0; i < 2; i++)
            {
                Card handmaiden = new Card(4, "Handmaiden", "this is a handmaiden");
                play_DecTemp.Add(handmaiden);
            }

            //Create Prince
            for (int i = 0; i < 2; i++)
            {
                Card prince = new Card(5, "Prince", "this is a prince");
                play_DecTemp.Add(prince);
            }

            //Create King
            Card king = new Card(6, "King", "this is a king");
            play_DecTemp.Add(king);

            //Create Countess
            Card countess = new Card(7, "Countess", "this is a countess");
            play_DecTemp.Add(countess);

            //Create Princess
            Card Princess = new Card(8, "Princess", "this is a princess");
            play_DecTemp.Add(Princess);

        }

        public void addPlayer(String name, Boolean isHuman, int index)
        {
            players.Add(new Player(name, isHuman, index));
        }

        //Melange le packet de carte
        public void shuffle()
        {
            this.play_Deck = new Stack<Card>();
            Random rnd = new Random();
            for (int i = 16; i > 0; i--)
            {
                int r = rnd.Next(i);
                play_Deck.Push(play_DecTemp.ElementAt(r));
                play_DecTemp.Remove(play_DecTemp.ElementAt(r));
            }
        }

        //Remove firstCard
        public Card RemoveFirstCard()
        {
            //Remove 1st Card of the Deck
            return this.play_Deck.Pop();
        }

        public Card GetCardTop()
        {
            //Return card of the top
            return this.play_Deck.Pop();
        }

        public void giveFirstCardToPlayer()
        {
            /*
            //Give one card to all players
            foreach (Player player in this.players)
            {
                GameController.PickCard(player);
            }
            */
        }

        //Reinit a round with all Pplayerlayers
        public void reinitPlayerList()
        {

            this.playersInCurrentRound = new List<Player>(players);
            foreach (Player p in players)
            {
                p.clearCard();
            }
        }

        //Delete a Defeated Player of a currentRound
        public void removePlayerOfRound(Player player)
        {
            playersInCurrentRound.Remove(player);
        }

        //Switch 2 players deck
        public void switchPlayersDeck(Player playerA, Player playerB)
        {
            List<Card> deck_temp = new List<Card>(playerA.player_Deck);
            playerA.player_Deck.Clear();
            playerA.player_Deck = new List<Card>(playerB.player_Deck);
            playerB.player_Deck.Clear();
            playerB.player_Deck = new List<Card>(deck_temp);
        }

        public Player getPlayerInRoundByName(String name)
        {
            Player res = null;
            for (int i = 0; i < this.getPlayerInRound().Count; i++)
            {
                if (this.getPlayerInRound().ElementAt(i).name == name)
                {
                    res = this.getPlayerInRound().ElementAt(i);
                }
            }
            return res;
        }
        public void setIsFinished(bool isfinished)
        {
            this.isFinished = isfinished;
        }

        public void eraseGame()
        {
            instance = null;
        }

        public void printDeck()
        {
            foreach(Card c in play_Deck)
            {
                Console.WriteLine(c.GetName());
            }
         
        }

        public Card CreatCardByGame(String str)
        {
            switch (str.ToLower())
            {
                case "guard":
                    return new Card(1, "Guard", "");


                case "priest":
                    return new Card(2, "Priest", "");


                case "baron":
                    return new Card(3, "Baron", "");


                case "handmaid":
                    return new Card(4, "Handmaid", "");


                case "prince":
                    return new Card(5, "Prince", "");


                case "king":
                    return new Card(6, "King", "");


                case "countess":
                    return new Card(7, "Countess", "");


                case "princess":
                    return new Card(8, "Princess", "");

                default:
                    return null;

            }
        }

        public Card CreatCardByValue(int value)
        {
            switch (value)
            {
                case 1:
                    return new Card(1, "Guard", "");


                case 2:
                    return new Card(2, "Priest", "");


                case 3:
                    return new Card(3, "Baron", "");


                case 4:
                    return new Card(4, "Handmaid", "");


                case 5:
                    return new Card(5, "Prince", "");


                case 6:
                    return new Card(6, "King", "");


                case 7:
                    return new Card(7, "Countess", "");


                case 8:
                    return new Card(8, "Princess", "");

                default:
                    return null;

            }
        }


    }
}
