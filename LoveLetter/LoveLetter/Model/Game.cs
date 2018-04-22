using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public List<Card> removedCards;

        public bool isFinished = false;
        public int roundNumber = 1;
        
        //Methods
        Game()
        {
            this.play_DecTemp = new List<Card>();
            this.players = new List<Player>();
            this.playersInCurrentRound = new List<Player>();
            this.removedCards = new List<Card>(); 
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
                Card guard = new Card(1, Properties._string.Guard, String.Format(Properties._string.GuardEffectValue,"\n","\n"),Properties.Resources.guard);
                play_DecTemp.Add(guard);
            }

            //Create Priest
            for (int i = 0; i < 2; i++)
            {
                Card priest = new Card(2, Properties._string.Priest, String.Format(Properties._string.PriestEffectValue, "\n"), Properties.Resources.priest);
                play_DecTemp.Add(priest);
            }

            //Create Baron
            for (int i = 0; i < 2; i++)
            {
                Card baron = new Card(3, Properties._string.Baron, String.Format(Properties._string.BaronEffectValue, "\n", "\n"), Properties.Resources.baron);
                play_DecTemp.Add(baron);
            }

            //Create Handmaiden
            for (int i = 0; i < 2; i++)
            {
                Card handmaiden = new Card(4, Properties._string.Handmaid, String.Format(Properties._string.HandmaidEffectValue, "\n"),Properties.Resources.handmaid);
                play_DecTemp.Add(handmaiden);
            }

            //Create Prince
            for (int i = 0; i < 2; i++)
            {
                Card prince = new Card(5, Properties._string.Prince, String.Format(Properties._string.PrinceEffectValue, "\n", "\n"), Properties.Resources.prince);
                play_DecTemp.Add(prince);
            }

            //Create King
            Card king = new Card(6, Properties._string.King,  String.Format(Properties._string.KingEffectValue, "\n"), Properties.Resources.king);
            play_DecTemp.Add(king);

            //Create Countess
            Card countess = new Card(7, Properties._string.Countess, String.Format(Properties._string.CountessEffectValue, "\n", "\n"),Properties.Resources.countess);
            play_DecTemp.Add(countess);

            //Create Princess
            Card Princess = new Card(8, Properties._string.Princess, String.Format(Properties._string.PrincessEffectValue,"\n"),Properties.Resources.princess);
            play_DecTemp.Add(Princess);

        }

        public Player addPlayer(String name, Boolean isHuman, int index)
        {
            Player temp = new Player(name, isHuman, index);
            players.Add(temp);
            return temp;
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

        public void giveFirstCardToPlayer()
        {
            //Give one card to all players
           foreach(Player player in this.players)
            {
                GameController.PickCard(player);
            }
        }

        //Reinit a round with all Pplayerlayers
        public void reinitPlayerList()
        {

            this.playersInCurrentRound = new List<Player>(players);
            foreach(Player p in players)
            {
                p.clearCard();
                p.isActive = true;
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
            for(int i = 0; i < this.getPlayerInRound().Count; i++)
            {
                if(this.getPlayerInRound().ElementAt(i).name == name)
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


        /** MultiPlayer Method **/

        public void setStack(Stack<Card> c)
        {
            play_Deck = c;
        }

        public Card CreatCardByGame(String str)
        {
            switch (str.ToLower())
            {
                case "guard":
                    return new Card(1, "Guard", String.Format(Properties._string.GuardEffectValue, "\n", "\n"), Properties.Resources.guard);


                case "priest":
                    return new Card(2, "Priest", String.Format(Properties._string.PriestEffectValue, "\n"), Properties.Resources.priest);


                case "baron":
                    return new Card(3, "Baron", String.Format(Properties._string.BaronEffectValue, "\n", "\n"), Properties.Resources.baron);


                case "handmaiden":
                    return new Card(4, "Handmaid", String.Format(Properties._string.HandmaidEffectValue, "\n"), Properties.Resources.handmaid);

                case "handmaid":
                    return new Card(4, "Handmaid", String.Format(Properties._string.HandmaidEffectValue, "\n"), Properties.Resources.handmaid);


                case "prince":
                    return new Card(5, "Prince", String.Format(Properties._string.PrinceEffectValue, "\n", "\n"), Properties.Resources.prince);


                case "king":
                    return new Card(6, "King", String.Format(Properties._string.KingEffectValue, "\n"), Properties.Resources.king);


                case "countess":
                    return new Card(7, "Countess", String.Format(Properties._string.CountessEffectValue, "\n", "\n"), Properties.Resources.countess);


                case "princess":
                    return new Card(8, "Princess", String.Format(Properties._string.PrincessEffectValue, "\n"), Properties.Resources.princess);

                default:
                    return null;

            }
        }

        public Player getPlayerByName(String name)
        {
            foreach (Player p in players)
            {
               
                if (p.getName().ToLower() == name.ToLower())
                {

                    return p;
                }
            }
            return null;
        }

        public Player getPlayerByIndex(int index)
        {
            foreach (Player p in players)
            {
                if (p.indexPlayer== index)
                {
                    return p;
                }
            }
            return null;
        }

        public String getAllCardName()
        {
            string res = "";
            foreach (Card c in play_Deck)
            {
                res += Environment.NewLine + c.GetName();
            }
            return res;


        }

    }
}
