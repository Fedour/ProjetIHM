using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveLetter
{
    class GameController
    {
        static Player winner;
        static Player currentPlayer;
        static int currentRound;
        static int limitMarker;
            

        //Retourne le nombre de marqueur nescessaire pour gagner la partie
        public static int getLimitNumberMarker()
        {
            switch (Game.Instance.getPlayer().Count())
            {
                case 2:
                    return 7;
                    break;
                case 3:
                    return 5;
                    break;
                default:
                    return 4;
                    break;
            }
        }

        //METHOD TO REMOVE
        public static void playerInit()
        {

        }
        public static void InitializeGame()
        {

            //ADD PLAYER IN CONSOLE REMOVE AFTER TESTING
            //-----

            limitMarker = getLimitNumberMarker();

            Game.Instance.initalizeCard();

            InitRound();
            FirstRandomPlayer();

            //-------
        }

        //Add a new player to the game
        public static void addPlayer(String name)
        {
            Game.Instance.players.Add(new Player(name));
        }


        //Play the game
        public static void PlayGame()
        {
            currentRound = 1;


            while (!Game.Instance.isFinished)
            {
                playRound();
            }
            

            


        }

        //Method for playing a round
        public static void playRound()
        {
            Console.WriteLine("--- ROUND " + currentRound + " ---");


            //Reinit a new round if it is not the first round of the game
            if (currentRound != 1)
            {
                InitRound();
                FirstRandomPlayer();
            }

            //Tant qu'il reste des carte, ou plus d'un joueur dans la manche...
            while (Game.Instance.getPlayerInRound().Count > 1
                && Game.Instance.getCard().Count > 0)
            {

                //Le joueur prend la carte sur la pile
                currentPlayer.PickCard(Game.Instance.getCard().Pop());

                //Le joueur joue ca carte...
                Console.WriteLine("Player " + currentPlayer.name + " Which card to play ?");

                //Representation carte
                int i = 1;
                foreach (Card card in currentPlayer.player_Deck)
                {
                    Console.WriteLine(i + currentPlayer.player_Deck.ElementAt(i - 1).name);
                    i++;
                }


                int cardIndex = int.Parse(Console.ReadLine());

                currentPlayer.playCard(currentPlayer.player_Deck.ElementAt(cardIndex - 1));


                Console.WriteLine("---------------");

            


                

                //End of round
                NextCurrentPlayer();
            }


            //Recupere le gagnant
            Player winnerOfRound = getWinnerOfRound();
            winnerOfRound.increaseMarker();

            //Check si gagnant
            checkWinner();

            currentRound += 1;
        }

        //Si il y a un gagnant...
        public static void checkWinner()
        {
            foreach (Player p in Game.Instance.getPlayer())
            {
                if (p.getNbMark() == limitMarker)
                {
                    EndGame(p);
                }
            }
        }

        //Retourne le gagnant de la manche
        public static Player getWinnerOfRound()
        {
            //Si un seul joueur dans le round...
            if (Game.Instance.getPlayerInRound().Count == 1)
            {
                return Game.Instance.getPlayerInRound().ElementAt(0);
            }
            //Si egalité
            else
            {
                Player winner = Game.Instance.getPlayerInRound().ElementAt(0);

                int sumOfStrenght = 0;
                int tempStrenght;

                int sumOfDeadCard=0;

                foreach (Player p in Game.Instance.getPlayerInRound())
                {
                    
                    tempStrenght = 0;
                    foreach (Card c in p.getCards())
                    {
                        tempStrenght += c.getValue();
                    }
                    if (tempStrenght > sumOfStrenght)
                    {
                        sumOfStrenght = tempStrenght;
                        winner = p;
                    }
                    //Si egalité de force celui qui a defaussé le plus grande nombre de carte de grande valeur gagne
                    else if (tempStrenght == sumOfStrenght)
                    {
                        tempStrenght = p.getSumOfDeadCard();
                        if (tempStrenght > sumOfDeadCard)
                        {
                            sumOfDeadCard = tempStrenght;
                            winner = p;
                        }

                    }
               
                }

                Console.WriteLine("Player : "+winner.getName() +" Won the round");
                return winner;

            }
        }
    


        public static void EndGame(Player player)
        {
            Console.WriteLine("Le joueur " + player.name + "a gagné");
            Game.Instance.setIsFinished(true);
        }

        //Choose next player
        public static void NextCurrentPlayer()
        {
            int pos = Game.Instance.getPlayer().IndexOf(currentPlayer);
            pos += 1;

            int nextPos = pos % Game.Instance.players.Count();

            currentPlayer = Game.Instance.getPlayer().ElementAt(nextPos);
        }

        //Choose a random Player to Start the round
        public static void FirstRandomPlayer()
        {
            Random rnd = new Random();
            int r = rnd.Next(Game.Instance.getPlayer().Count());
            currentPlayer = Game.Instance.getPlayer().ElementAt(r);
        }

        public static void InitRound()
        {      
            Game.Instance.reinitPlayerList();
            Game.Instance.initalizeCard();
            Game.Instance.shuffle();
            if (Game.Instance.players.Count == 2)
            {
                //Game.Instance.RemoveFirstCard2players();
            }
            else
            {
                Game.Instance.RemoveFirstCard();
            }
            Game.Instance.giveFirstCardToPlayer();
        }

        public Player GetPlayerByName(String name)
        {
            return null;
        }



    }
}
