using LoveLetter.View;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoveLetter
{
    class GameController
    {
        public static Player winner;
        public static Player currentPlayer;
        public static int currentRound;
        public static int limitMarker;
        public static game_window gameView = null;
        public static Card lastDiscardedCard = null;

        //Retourne le nombre de marqueur nescessaire pour gagner la partie
        public static int getLimitNumberMarker()
        {
            switch (Game.Instance.getPlayer().Count())
            {
                case 2:
                    return 7;
                case 3:
                    return 5;
                default:
                    return 4;
            }
        }

        public static void newGame(int nbrPlayers,String name)
        {
            Properties.Settings.Default.nbrPlayers = nbrPlayers;

            Game.Instance.addPlayer(name, true, 0);

            for (int i = 1; i < nbrPlayers; i++)
            {
                Game.Instance.addPlayer(Properties._string.Player + i, false, i); //Test
            }
            game_window gw = new game_window();
            gameView = gw;
            gameView.Shown += new EventHandler(gameViewLoaded);
            gameView.ShowDialog();

           
        }

        //setup the game
        public static void InitializeGame()
        {
            limitMarker = getLimitNumberMarker();

            InitRound();
            PlayGame();           
        }

        private static void PlayGame()
        {
                PlayTurn();
        }

        //play a turn for AI , if it's human it will does nothing, the human has to trigger play card effect
        private static void PlayTurn()
        {
            //if the current player is protected at the beggining of the turn , we have to remove his immunity (because it only last one turn)
            if(currentPlayer.HMProtected)
            {
                currentPlayer.HMProtected = false;
            }

            //check if the round is over
            if(Game.Instance.play_Deck.Count == 0 || Game.Instance.playersInCurrentRound.Count==1)
            {
                Player winner = getWinnerOfRound();
                winner.nbMarker += 1;
                Game.Instance.roundNumber += 1;
                checkWinner(); // is the game over ?

                //check if the game is finished
                if(!Game.Instance.isFinished)
                {
                    Boolean mutedTemp = GameController.gameView.muted;
                    double mainTemp = GameController.gameView.mainVolume;
                    double musicTemp = GameController.gameView.musicVolume;
                    double effectTemp = GameController.gameView.effectVolume;

                    game_window newGameView = new game_window(mutedTemp, mainTemp, musicTemp, effectTemp);
                    newGameView.Shown += new EventHandler(gameViewLoaded);

                    GameController.gameView.ambianceSound.Stop();
                    GameController.gameView.ambianceSound.Close();
                    GameController.gameView.Close();
                    GameController.gameView.Dispose();
                    GameController.gameView = newGameView; 

                    newGameView.ShowDialog();
                }
                else
                {
                    GameController.gameView.backToMenu();
                }
            }
            else
            {
                if (currentPlayer.isHuman)
                {
                    GameController.gameView.showIndications(Properties._string.Take1CardFromTheDeck, "deck");
                }
                else
                {
                    Thread.Sleep(900);
                    Player player = Game.Instance.playersInCurrentRound.Find(x => x.indexPlayer == currentPlayer.indexPlayer);
                    GameController.PickCard(player);
                    Thread.Sleep(900);

                    if(CardEffect.MustPlayCountess(player)) //must play the countess
                    {
                        //tell all AIs what has been played
                        for (int p = 0; p < Game.Instance.players.Count; p++)
                        {
                            Player otherPlayer = Game.Instance.players[p];
                            if (!otherPlayer.isHuman)
                                otherPlayer.ai.CardPlayed(player.indexPlayer, 7);
                        }
                    }
                    else //normal turn
                    {
                            AI.Decision decision = player.ai.Decide();
                            Card card = player.player_Deck[decision.chosenCard];
                            GameController.gameView.PlayCard(card, player);
                            player.playCard(card);
                            //tell all AIs what has been played
                            for (int p = 0; p < Game.Instance.players.Count; p++)
                            {
                                Player otherPlayer = Game.Instance.players[p];
                                if (!otherPlayer.isHuman)
                                    otherPlayer.ai.CardPlayed(player.indexPlayer, card.value);
                            }
                            card.Effect(player, Game.Instance.players[decision.guessPlayer], decision.guessCard);
                    }
                    NextCurrentPlayer();
                    PlayTurn();
                }
            }

        }

        //Add a new player to the game
        public static void addPlayer(String name)
        {
            Game.Instance.players.Add(new Player(name));
        }

        //Si il y a un gagnant...
        public static void checkWinner()
        {
            foreach (Player p in Game.Instance.getPlayer())
            {
                if (p.getNbMark() == limitMarker)
                {
                    GameController.gameView.showMessage(String.Format(Properties._string.HasReachedLimitNumberMarker, p.name),true);
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
                GameController.gameView.showMessage(String.Format(Properties._string.IsTheLastPlayer, Game.Instance.getPlayerInRound().ElementAt(0).name),true);
                return Game.Instance.getPlayerInRound().ElementAt(0);               
            }
            //Si egalité
            else
            {
                String reasonOfVictory = "";
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
                        reasonOfVictory = Properties._string.hasTheStrongestCard;
                    }
                    //Si egalité de force celui qui a defaussé le plus grande nombre de carte de grande valeur gagne
                    else if (tempStrenght == sumOfStrenght)
                    {
                        tempStrenght = p.getSumOfDeadCard();
                        if (tempStrenght > sumOfDeadCard)
                        {
                            sumOfDeadCard = tempStrenght;
                            winner = p;
                            reasonOfVictory = Properties._string.hasTheMostStrongestCardDiscarded;
                        }
                    }
                }
                GameController.gameView.showMessage(String.Format(reasonOfVictory, winner.name),true);
                return winner;
            }
        }

        //call pickCard in the model and PickCard in the view
        public static void PickCard(Player player)
        {
            if (player.player_Deck.Count < 2)
            {
                player.PickCard(Game.Instance.getCard().Pop());
                GameController.gameView.PickCard(Game.Instance.getPlayer().IndexOf(player));

                if (player.isHuman)//check if the human player need to play the countess
                {
                            CardEffect.MustPlayCountess(player);
                }
            }
            else
            {
                MessageBox.Show(Properties._string.YouCannotPickMoreThanOneCard);
            }

        }


        //function reserved to human player, trigger the effect of the card C and then call playturn function to make other player play
        public async static void PlayCard(Card c, Player p)
        {
            if (p.player_Deck.Count == 2) //check if the player picked a card 
            {
                GameController.gameView.PlayCard(c, p);
                p.playCard(c); //play the card
                await Task.Run(() => c.Effect(p, null, -1)); //trigger the effect ( arg 2 and 3 doesn't matter when the source is human because arugments will be updated with interface-user interaction )
                NextCurrentPlayer();
                PlayTurn();
            }
            else //otherwise he cannot play
            {
                MessageBox.Show(Properties._string.YouNeedToPickACardFirst);
            }                   
        }

        public static void EndGame(Player player)
        {
            Game.Instance.setIsFinished(true);
        }

        //Choose next player
        public static void NextCurrentPlayer()
        {
            int pos = Game.Instance.getPlayerInRound().IndexOf(currentPlayer);
            pos += 1;

            int nextPos = pos % Game.Instance.getPlayerInRound().Count();

            currentPlayer = Game.Instance.getPlayerInRound().ElementAt(nextPos);
        }

        //Choose a random Player to Start the round
        public static void FirstRandomPlayer()
        {
             Random rnd = new Random();
             int r = rnd.Next(Game.Instance.getPlayer().Count());
             currentPlayer = Game.Instance.getPlayer().ElementAt(r);
            //currentPlayer = Game.Instance.getPlayer().ElementAt(0);
        }

        public static void InitRound()
        {
            GameController.gameView.showMessage(String.Format(Properties._string.NewRound, Game.Instance.roundNumber),true);

            Game.Instance.reinitPlayerList();
            Game.Instance.initalizeCard();
            Game.Instance.shuffle();

            Game.Instance.removedCards.Clear();
            if (Game.Instance.players.Count == 2)
            {
                for(int i =0; i<3;i++)
                {
                    Card c = Game.Instance.RemoveFirstCard(); 
                    Game.Instance.removedCards.Add(c);
                    GameController.gameView.removeCardFromGameDeck2Players(c);
                }
            }
            else
            {
                Card c = Game.Instance.RemoveFirstCard();
                Game.Instance.removedCards.Add(c);
                GameController.gameView.remove1CardFromGameDeck(c);
            }

            Game.Instance.giveFirstCardToPlayer();
            GameController.FirstRandomPlayer();

            //les IA oublient les cartes des adversaires
            for (int p=0;p<Game.Instance.players.Count;p++)
            {
                Player player = Game.Instance.players[p];
                if (!player.isHuman)
                    player.ai.RoundStarts(); 
            }
        }

        public static void killPlayer(Player p)
        {
            //player discarded princess
            if(p.deadCard.Count>0 && p.deadCard.ElementAt(p.deadCard.Count-1).value == 8)
            {
                GameController.gameView.showMessage(String.Format(Properties._string.DiscardedPrincess, p.name),Properties.Settings.Default.showMessage);
            }
            Game.Instance.removePlayerOfRound(p);
            p.isActive = false;      
            GameController.gameView.showCardsHandOnBoard(p);
        }

        private static void gameViewLoaded(object sender,EventArgs e)
        {
            GameController.InitializeGame();
        }

    }
}
