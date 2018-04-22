using LoveLetter.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoveLetter
{
    class GameControllerMulti
    {
        public static Player winner;
        public static Player owner;
        public static Player currentPlayer;
        public static int currentRound;
        public static int limitMarker;
        public static game_window_multi gameView = null;
        public static Card lastDiscardedCard = null;
        public static bool CountessBlock = false;

        //**Variable multiplayer**//
        public static Boolean isMultiPlayer = true;
        public static Network.Client playerClient;
        public static Network.Client playerClientThread;

        public static Boolean freeThread = false;
        private static Thread ctThread;
        public static Boolean readyToRemoveCard = false;
        public static int port;


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


        //Multiplayer Constructor//
        public static void newGame(int nbrPlayers, List<String> playerName, Network.Client client, int portServer)
        {
            port = portServer;
            playerClient = client;

            isMultiPlayer = true;
            Properties.Settings.Default.nbrPlayers = nbrPlayers;

            client.getName();
            owner = Game.Instance.addPlayer(playerClient.getName(), true, 0);


            int idx = 1;


            foreach (String str in playerName)
            {
                //Ajout des autres joueurs
                if (str != playerClient.getName())
                {
                    Player add = Game.Instance.addPlayer(str, true, idx);
                    idx++;
                }
            }
            game_window_multi gw = new game_window_multi();
            gameView = gw;
            gameView.Shown += new EventHandler(gameViewLoaded);
            gameView.ShowDialog();
        }

        public static void NewTurn()
        {
            if(currentPlayer.HMProtected)
            {
                currentPlayer.HMProtected = false;
            }
        }

        //setup the game
        public static void InitializeGameMulti()
        {
            InitRound();
        }

        public static void StartThread()
        {
            ctThread = new Thread(GetServerMessage);
            ctThread.Start();
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
                    GameControllerMulti.gameView.showMessage(String.Format(Properties._string.HasReachedLimitNumberMarker, p.name), true);
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
                GameControllerMulti.gameView.showMessage(String.Format(Properties._string.IsTheLastPlayer, Game.Instance.getPlayerInRound().ElementAt(0).name), true);
                return Game.Instance.getPlayerInRound().ElementAt(0);
            }
            //Si egalité
            else
            {
                Player winner = Game.Instance.getPlayerInRound().ElementAt(0);

                int sumOfStrenght = 0;
                int tempStrenght;

                int sumOfDeadCard = 0;

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
                return winner;
            }
        }



        public static void PickCardMulti(Player player)
        {


            if (Game.Instance.play_Deck.Count > 0)
            {
                if (player.getName().ToLower() == playerClient.getName().ToLower())
                {
                    if (player.player_Deck.Count < 2)
                    {

                        Card c = Game.Instance.RemoveFirstCard();
                        player.PickCard(c); 

                        GameControllerMulti.gameView.PickCard(Game.Instance.getPlayer().IndexOf(player));
                        CardEffectMullti.MustPlayCountess(player, playerClient);

                        playerClient.ClientPickCard(c); //Informe les autres joueurs
                    }
                    else
                    {
                        MessageBox.Show(Properties._string.YouCannotPickMoreThanOneCard);

                    }
                }

                else
                {
                    MessageBox.Show(Properties._string.ItsNotYourTurn);

                }
            }
            else
            {
                MessageBox.Show(Properties._string.TheGameDeckIsEmpty);
            }

        }

        public static void PickCardMultiPrince(Player player)
        {


            if (player.player_Deck.Count < 2)
            {
                Card c = Game.Instance.RemoveFirstCard();
                player.PickCard(c);
                GameControllerMulti.gameView.PickCard(Game.Instance.getPlayer().IndexOf(player));
                playerClient.ClientPickCard(c);
            }
            else
            {
                MessageBox.Show("You cannot pick more than one card");

            }


        }


        /** Multiplayer **/
        public static void PickSpecificCard(Player player, Card c)
        {
            player.PickCard(c);
            GameControllerMulti.gameView.PickCard(Game.Instance.getPlayer().IndexOf(player));


        }


        //function reserved to human player, trigger the effect of the card C and then call playturn function to make other player play
        public async static void PlayCardMulti(Card c, Player p)
        {
            if (p.player_Deck.Count == 2) //check if the player picked a card
            {
                if(c.value!=7 &&CountessBlock)
                {
                    MessageBox.Show(String.Format(Properties._string.MustPlayCountess));
                    
                }
                else
                {
                    if (c.value == 7)
                        CountessBlock = false;
                    GameControllerMulti.gameView.PlayCard(c, p);

                    p.playCard(c); //play the card
                    playerClient.ClientPlayCard(p, c);

                    try
                    {
                        await Task.Run(() => c.MultiPlayerEffect(p, null, -1, port,playerClient));
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }

                }
                NextCurrentPlayer();
                

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

        public static void InitRound()
        {

            GameControllerMulti.gameView.showMessage(String.Format(Properties._string.NewRound, Game.Instance.roundNumber), true);
            Game.Instance.reinitPlayerList();
            foreach(Player p in Game.Instance.getPlayer())
            {
                p.clearCard();
            }
           
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public static void killThread()
        {
            ctThread.Abort();
        }

        public static void killPlayer(Player p)
        {
            Game.Instance.removePlayerOfRound(p);
            p.isActive = false;
            GameControllerMulti.gameView.showMessage(String.Format(Properties._string.IsOutOfTheGame, p.name), true);
            GameControllerMulti.gameView.showCardsHandOnBoard(p);
        }

        private static void gameViewLoaded(object sender, EventArgs e)
        {
            readyToRemoveCard = false;
            GameControllerMulti.InitializeGameMulti();
        
            StartThread();
            PlayerReadyToHaveCard();
           
        }

        /****************** MULTI COMMUNICATION METHODS ********************/
        static private void GetServerMessage()
        {
            
            try
            {
                while (!freeThread)
                {

                    Network.Message m = playerClient.ClientReceiveMsgText();
                    if (m.pseudo == "server")
                    {
                       
                        switch (m.commande)
                        {


                            case Network.Commande.GIVESTACK:
                                SetStack(m.data);
                                
                                break;

                            case Network.Commande.REMOVEDECKCARD:
                                RemoveCard();
                                break;

                            case Network.Commande.GIVECARD:
                                ReceiveCard(m.data);
                                break;

                            case Network.Commande.PLAYERTURN:
                                PlayerTurn(m.data);
                                break;

                            case Network.Commande.PICKCARD:
                                RefreshAfterPick(m.data);
                                break;

                            case Network.Commande.PLAYCARD:
                                String[] dataArray = m.data.Split('|');
                                RefreshAfterPlay(dataArray[1], dataArray[0]);
                                break;


                            //RECEIVING CARD EFFECT
                            case Network.Commande.GUARDEFFECT:
                                ReceiveGuardEffect(m.data);
                                break;
                            case Network.Commande.GUARDEFFECTRES:
                                //ReceiveGuardEffectRes(m.data);
                                break;


                            case Network.Commande.PRIESTEFFECT:
                                ReceivePriestEffect(m.data);
                                break;

                            case Network.Commande.BARONEFFECT:
                                ReceiveBaronEffect(m.data);
                                break;
                       

                            case Network.Commande.HANDMAIDEEFFECT:
                                ReceiveHandmaidEffect(m.data);
                                break;

                            case Network.Commande.PRINCEEFFECTRES:
                                ReceivePrinceEffect(m.data);
                                break;

                            case Network.Commande.KINGEFFECTRES:
                                ReceiveKingEffect(m.data);
                                break;
                 

                            case Network.Commande.PRINCESSEFECT:
                                ReceivePrincessEffect(m.data);
                                break;

                            //--------------------------------//
                            case Network.Commande.ROUNDOVER:
                                AskCloseThread();
                                RoundOver(m.data);
                                break;

                            case Network.Commande.PLAYCARDVISUALRES:
                                PlayCardVisual(m.data);
                                break;

                            case Network.Commande.GAMEISOVER:
                                GameIsOver(m.data);
                                break;

                            case Network.Commande.PLAYERLEAVE:
                                SomeOneLeave();
                                break;

                        }
                    }
              
                }

            }
            catch (Exception e)
            {
                freeThread = true;
            }
           
            


        }
        public static void RoundOver(String data)
        {


            Player p = Game.Instance.getPlayerByName(data);
            p.nbMarker += 1;
            Game.Instance.roundNumber += 1;



            GameControllerMulti.gameView.showMessage(String.Format(Properties._string.IsTheLastPlayer, p.name), true);

          
            Boolean mutedTemp = GameControllerMulti.gameView.muted;
            double mainTemp = GameControllerMulti.gameView.mainVolume;
            double musicTemp = GameControllerMulti.gameView.musicVolume;
            double effectTemp = GameControllerMulti.gameView.effectVolume;

            GameControllerMulti.gameView.endOfround();
            GameControllerMulti.gameView = new game_window_multi(mutedTemp, mainTemp, musicTemp, effectTemp);
            gameView.Shown += new EventHandler(gameViewLoaded);
            gameView.ShowDialog();
            
            
            



        }
        public static void GameIsOver(String data)
        {
            Player p = Game.Instance.getPlayerByName(data);
            GameControllerMulti.gameView.showMessage(String.Format(Properties._string.HasReachedLimitNumberMarker, p.name), true);
            GameControllerMulti.backToMenu();
        }

        //BackToMenu ne fonctionne pas dans la vue
        public static void backToMenu()
        {
            GameControllerMulti.gameView.backToMenu();
        }

        public static void SomeOneLeave()
        {
            GameControllerMulti.gameView.showMessage(String.Format(Properties._string.SomeoneLeave), Properties.Settings.Default.showMessage);
            GameControllerMulti.backToMenu();
        }

        public static void RefreshAfterPick(String name)
        {
            if (!matchPlayerClient(name))
            {

                Card c = Game.Instance.RemoveFirstCard();
                currentPlayer.PickCard(c);


                GameControllerMulti.gameView.PickCard(Game.Instance.getPlayer().IndexOf(currentPlayer));
            }

        }

        public static void RefreshAfterPlay(String name, String card)
        {
            try
            {
                if (!matchPlayerClient(name))
                {
                    Card cardReal = null;
                    foreach (Card c in currentPlayer.getCards())
                    {

                        if (c.name.ToLower() == card.ToLower())
                        {
                            cardReal = c;
                        }
                    }
                    
                    GameControllerMulti.gameView.PlayCard(cardReal, currentPlayer);
                    currentPlayer.playCard(cardReal);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e + "");
            }

        }

        public static void SetStack(String data)
        {
            List<Card> temp = new List<Card>();
            Stack<Card> resStack = new Stack<Card>();

            String[] dataArray = data.Split('|');
            int cardCount = int.Parse(dataArray[0]);

            for (int i = 1; i <= cardCount; i++)
            {
                temp.Add(Game.Instance.CreatCardByGame(dataArray[i]));
            }
            temp.Reverse();
            foreach (Card c in temp)
            {

                resStack.Push(c);

            }


            Game.Instance.setStack(resStack);
            PlayerReadyToRemoveCard();
        }

        public static void RemoveCard()
        {
            if (Game.Instance.players.Count == 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    Card c = Game.Instance.RemoveFirstCard();
                    GameControllerMulti.gameView.removeCardFromGameDeck2Players(c);
                }
            }
            else
            {
                Card c = Game.Instance.RemoveFirstCard();
                GameControllerMulti.gameView.remove1CardFromGameDeck(c);
            }
        }


        public static void AskCloseThread()
        {
            if (isMultiPlayer)
            {                
                playerClient.ClientFreeTrheadGameForm();

            }
        }

        public static void ReceiveCard(String data)
        {
            String[] dataArray = data.Split('|');
            String playerName = dataArray[0];


            Game.Instance.RemoveFirstCard();
            Card c = Game.Instance.CreatCardByGame(dataArray[1]);

            PickSpecificCard(Game.Instance.getPlayerByName(playerName), c);

        }

        public static void PlayerTurn(String data)
        {
            Player p = Game.Instance.getPlayerByName(data);
            currentPlayer = p;
            GameControllerMulti.gameView.showMessage(String.Format(Properties._string.PlayerPlay, currentPlayer.name), Properties.Settings.Default.showMessage);
            NewTurn();
        }

        public static Boolean matchPlayerClient(String name)
        {
            return name.ToLower() == playerClient.getName().ToLower();
        }

        public static void ReceiveHandmaidEffect(String source)
        {
            Player pSource = Game.Instance.getPlayerInRoundByName(source);
            
            if (!matchPlayerClient(source))
            {
                GameControllerMulti.gameView.showMessage(String.Format(Properties._string.IsProtectedForOneTurn, pSource.name), Properties.Settings.Default.showMessage);
                pSource.HMProtected = true;
            }
            
        }

        public static void ReceiveGuardEffect(String data)
        {

            String[] arrayData = data.Split('|');

            Player source = Game.Instance.getPlayerInRoundByName(arrayData[0]);
            Card c = Game.Instance.CreatCardByGame(arrayData[1]);
            Player target = Game.Instance.getPlayerInRoundByName(arrayData[2]);
            Boolean res = false;
            if (arrayData[3].ToLower() == "true")
                res = true;


            GameControllerMulti.gameView.showMessage(String.Format(Properties._string.ThinksThatHas, source.name,
            target.name, c.name, Properties.Settings.Default.showMessage), true);

            if (res)
            {
                GameControllerMulti.gameView.showMessage(Properties._string.WellGuessed, Properties.Settings.Default.showMessage);
                GameControllerMulti.killPlayer(target);
            }
            else
            {
                GameControllerMulti.gameView.showMessage(string.Format(Properties._string.DoesNotHaveTheCard, target.name, c.GetName()), true);
            }

           
        }

        public static void ReceiveGuardEffectRes(String data)
        {

            String[] arrayData = data.Split('|');
            Player s = Game.Instance.getPlayerInRoundByName(arrayData[0]);
            String res = arrayData[1];
            Card c = Game.Instance.CreatCardByGame(arrayData[2]);
            Player target = Game.Instance.getPlayerInRoundByName(arrayData[3]);



            if (!matchPlayerClient(s.getName()))
            {
                if (res.ToLower() == "true")
                {
                    GameControllerMulti.gameView.showMessage(Properties._string.WellGuessed, Properties.Settings.Default.showMessage);
                    killPlayer(target);
                }
                else
                {
                    GameControllerMulti.gameView.showMessage(string.Format(Properties._string.DoesNotHaveTheCard, target.name, c.GetName()), Properties.Settings.Default.showMessage);
                }

            }

        }
        public static void ReceivePriestEffect(String data)
        {

            String[] arrayData = data.Split('|');
            Player s = Game.Instance.getPlayerInRoundByName(arrayData[0]);
            Player target = Game.Instance.getPlayerInRoundByName(arrayData[1]);


          
            if (!matchPlayerClient(s.getName()))
            {
                GameControllerMulti.gameView.showMessage(String.Format(Properties._string.UseAPriestToSee, s.name, target.name), Properties.Settings.Default.showMessage);
            }

        }

        public static void ReceiveBaronEffect(String data)
        {

            String[] arrayData = data.Split('|');
            Player source = Game.Instance.getPlayerInRoundByName(arrayData[0]);
            Player target = Game.Instance.getPlayerInRoundByName(arrayData[1]);
            Player dead = Game.Instance.getPlayerInRoundByName(arrayData[2]);

            GameControllerMulti.gameView.compareCardsHand(String.Format(Properties._string.ComparingHandsOfAnd, source.name, target.name), source, target);

            if (source.player_Deck.ElementAt(0).value > target.player_Deck.ElementAt(0).value)
            {
                GameControllerMulti.gameView.showMessage(String.Format(Properties._string.WonTheBattle, source.name, target.name), true);
                GameControllerMulti.killPlayer(target);
            }
            else if (source.player_Deck.ElementAt(0).value < target.player_Deck.ElementAt(0).value)
            {
                GameControllerMulti.gameView.showMessage(String.Format(Properties._string.WonTheBattle, target.name, source.name), true);
                GameControllerMulti.killPlayer(source);
            }
            else if(arrayData[2]=="")
            {
                GameControllerMulti.gameView.showMessage(Properties._string.Draw, true);
            }

        }

        public static void ReceiveBaronEffectRes(String data)
        {

        }

        public static void ReceivePrinceEffect(String data)
        {

            String[] arrayData = data.Split('|');
          

            Player s = Game.Instance.getPlayerInRoundByName(arrayData[0]);
            Player target = Game.Instance.getPlayerInRoundByName(arrayData[1]);

            Card deletedCard = target.player_Deck.ElementAt(0);
            GameControllerMulti.PickCardMultiPrince(target);
            GameControllerMulti.gameView.PlayCard(deletedCard, target);
            target.deadCard.Add(deletedCard);
            GameControllerMulti.lastDiscardedCard = deletedCard;
            target.player_Deck.Remove(deletedCard);
            GameControllerMulti.gameView.showMessage(String.Format(Properties._string.HadToDiscard, target.name), Properties.Settings.Default.showMessage);


  
        }

        //For prince EFFECT
        public static void PlayCardVisual(String data)
        {
           
            String[] arrayData = data.Split('|');
            Player s = Game.Instance.getPlayerInRoundByName(arrayData[0]);
            Card c = Game.Instance.CreatCardByGame(arrayData[1]);

            s.deadCard.Add(c);
            GameControllerMulti.lastDiscardedCard = c;
           

            GameControllerMulti.gameView.PlayCard(c, s);
            s.player_Deck.Remove(c);
        }

        public static void ReceiveKingEffect(String data)
        {

    
            
            String[] arrayData = data.Split('|');
            Player s = Game.Instance.getPlayerInRoundByName(arrayData[0]);
            Player target = Game.Instance.getPlayerInRoundByName(arrayData[1]);

            GameControllerMulti.gameView.showMessage(String.Format(Properties._string.SwitchHands, s.name, target.name), Properties.Settings.Default.showMessage);

            if (target.getName().ToLower() == playerClient.getName().ToLower())
            {
                List<Card> tmp = new List<Card>(s.player_Deck);
                s.player_Deck = new List<Card>(target.player_Deck);
                target.player_Deck = new List<Card>(tmp);

                GameControllerMulti.gameView.updateHand(target);
            }
    }

        public static void ReceivePrincessEffect(String data)
        {
            
            Player dead = Game.Instance.getPlayerByName(data);
            if (!matchPlayerClient(dead.getName()))
            {
                killPlayer(dead);
            }
        }

        public static void PlayerLeave()
        {
            playerClient.ClientLeaveGame();
        }

        public static void PlayerReadyToHaveCard()
        {
            playerClient.ClientReadyToHaveCard();
        }

        public static void PlayerReadyToRemoveCard()
        {
            playerClient.ClientReadyToRemoveCard();
        }

    }
}
