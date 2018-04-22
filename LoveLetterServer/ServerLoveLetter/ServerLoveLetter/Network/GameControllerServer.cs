
using LoveLetter.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace LoveLetter
{
    class GameControllerServer
    {
        public static Player winner;
        public static Player currentPlayer;
        public static int currentRound;
        public static int limitMarker;
        public static Card lastDiscardedCard = null;
        public static int PlayerReady;
        public static int PlayerReadyToRemove;



        //**Variable multiplayer**//
        public static Boolean isMultiPlayer = false;
        public static Network.Client playerClient;

        public static List<Player> playerConnected = new List<Player>();
        public static List<EndPoint> playerEndpoint = new List<EndPoint>();

        public static Boolean freeThread;
        public static Socket lobbySocket;

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

        public static void newGame(int nbrPlayers, List<Player> list, List<EndPoint> endPoint, Socket lb)
        {
            ServerLoveLetter.Properties.Settings1.Default.nbrPlayers = nbrPlayers;
            lobbySocket = lb;
            playerConnected = list;
            limitMarker = getLimitNumberMarker();


            foreach (Player p in playerConnected)
            {
                Game.Instance.addPlayer(p.getName(), true, 0);
            }

            

        }


 
        //Setup multiplayer game
        public static void InitializeGameMulti()
        {
         
            limitMarker = getLimitNumberMarker();
           

        }


        //play a turn for AI , if it's human it will does nothing, the human has to trigger play card effect
        private static void PlayMultiTurn()
        {
            
            if (Game.Instance.play_Deck.Count == 0 || Game.Instance.playersInCurrentRound.Count == 1)
            {
                Player winner = getWinnerOfRound();
                winner.nbMarker += 1;
                checkWinner(); // is the game over ?

                //check if the game is finished
                if (!Game.Instance.isFinished)
                {
                    SendEndOfRound(winner);
                    BeginNewRound();


                }
            
            }
            else
            {
                ChangeCurrentPlayer();
            }


        }

        static public void BeginNewRound()
        {
            PlayerReady = 0;
            PlayerReadyToRemove = 0;
            playerEndpoint.Clear();
            Thread.Sleep(4000);
        }
        static public void ChangeCurrentPlayer()
        {
            NextCurrentPlayer();
            SendPlayerToPlay(currentPlayer);
        }

        //Add a new player to the game
        public static void addPlayer(String name)
        {
            Game.Instance.players.Add(new Player(name));
        }

        //Si il y a un gagnant...
        public static void checkWinner()
        {
            Console.WriteLine(limitMarker + "Marqueur limit");
            foreach (Player p in Game.Instance.getPlayer())
            {
                Console.WriteLine("Marqueur: " + p.getNbMark());
                if (p.getNbMark() == limitMarker)
                {
                    Console.WriteLine("Dans if: " + p.getNbMark());
                    // GameController.gameView.showMessage(p.name + " has reached the limit number of marker, he won !");
                    EndGame(p);
                    SendGameIsOVer(p);
                }
            }
        }

        //Retourne le gagnant de la manche
        public static Player getWinnerOfRound()
        {
            //Si un seul joueur dans le round...
            if (Game.Instance.getPlayerInRound().Count == 1)
            {
                // GameController.gameView.showMessage(winner.name + " is the last player !");
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

        //call pickCard in the model and PickCard in the view
        public static void PickCard(Player player)
        {
            if (Game.Instance.play_Deck.Count > 0)
            {
                if (player.player_Deck.Count < 2)
                {

                    Card c = Game.Instance.getCard().Pop();
                    player.PickCard(c);

                }
                else
                {
                    //MessageBox.Show("You cannot pick more than one card");
                }
            }
            else
            {
                Console.WriteLine("No more card");
            }

         
        }


        public static void PlayCard(String card, String player)
        {
            Card c = Game.Instance.CreatCardByGame(card);
            try
            {
                Player p = Game.Instance.getPlayerInRoundByName(player);

                SendPlayCard(c.GetName(), p.getName());
                p.playCard(c);
            }
            catch (Exception e)
            {
                Console.WriteLine("Name " + c.GetName());
                Console.WriteLine(e);
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
        public static void FirstRandomPlayerMulti()
        {
            Random rnd = new Random();
            int r = rnd.Next(Game.Instance.getPlayer().Count());
            currentPlayer = Game.Instance.getPlayer().ElementAt(r);
            SendPlayerToPlay(currentPlayer);

            Console.WriteLine("AFFFTER : ");
            Game.Instance.printDeck();



        }

        //Give first card to Player
        public static void GiveFirstCardMulti()
        {
            foreach (Player p in Game.Instance.getPlayer())
            {
                Card c = Game.Instance.GetCardTop();
                Console.WriteLine("Joueur: " + p.getName() + " pick: " + c.GetName());
                SendCardToPlayer(p, c);
                p.PickCard(c);
            }

        }



        public static void killPlayer(Player p)
        {
            Game.Instance.removePlayerOfRound(p);
            Console.WriteLine("Player kill");
            p.isActive = false;
        }

        private static void gameViewLoaded(object sender, EventArgs e)
        {
            GameController.InitializeGame();
        }


        public static void InitMulitPlayerRound()
        {
            Game.Instance.reinitPlayerList();
            Game.Instance.initalizeCard();
            Game.Instance.shuffle();
            Game.Instance.printDeck();
            SendStackDeck(Game.Instance.getCard());


           
            GiveFirstCardMulti();
            FirstRandomPlayerMulti();


        }

        public static void RemoveReady()
        {
            SendRemoveReady();

            if (Game.Instance.players.Count == 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    Card c = Game.Instance.RemoveFirstCard();
                    Console.WriteLine("Carte retiré: " + c.GetName());
                }
            }
            else
            {
                Card c = Game.Instance.RemoveFirstCard();
            }
        }

        public static void PlayerPickCard(String card, String pseudo)
        {
            Console.WriteLine("Local" + card + "pseudo " + pseudo);
            
            Card c = Game.Instance.RemoveFirstCard();
            Player p = Game.Instance.getPlayerInRoundByName(pseudo);

            Console.WriteLine("Server" + c.GetName() + "pseudo " + p.getName());

            p.PickCard(c);
            SendPickCard(p.getName());

        }

        public static void PlayerAskDeck()
        {
            Message m_Stack = new Message(Commande.ASKDECK, CommandeType.REPONSE, "TEST", "server");
            foreach (EndPoint e in playerEndpoint)
            {
                lobbySocket.SendTo(m_Stack.GetBytes(), e);
            }
        }




        /****************** COMMUNICATION METHODS ********************/

        //Card distribution
        public static void SendStackDeck(Stack<Card> stack)
        {
            String data = Game.Instance.getCard().Count + "|";
            foreach (Card c in Game.Instance.getCard())
            {
                data += c.GetName() + "|";
            }

            Message m_Stack = new Message(Commande.GIVESTACK, CommandeType.REPONSE, data, "server");
            foreach (EndPoint e in playerEndpoint)
            {
                lobbySocket.SendTo(m_Stack.GetBytes(), e);
            }

        }
        public static void SendRemoveReady()
        {
            Console.WriteLine("Server send remove Ready");
            Message m_RemoveCard = new Message(Commande.REMOVEDECKCARD, CommandeType.REPONSE, "" + "", "server");

            foreach (EndPoint e in playerEndpoint)
            {

                lobbySocket.SendTo(m_RemoveCard.GetBytes(), e);
            }
        }


        //Send to client which player is the currentPlayer
        public static void SendPlayerToPlay(Player p)
        {
            Message m_PlayerToPlay = new Message(Commande.PLAYERTURN, CommandeType.REPONSE, p.getName(), "server");
            
            foreach (EndPoint e in playerEndpoint)
            {

                lobbySocket.SendTo(m_PlayerToPlay.GetBytes(), e);

            }
        }

        public static void SendCardToPlayer(Player p, Card c)
        {
            String data = p.getName() + "|" + c.GetName();

            Message m_Card = new Message(Commande.GIVECARD, CommandeType.REPONSE, data, "server");

            foreach (EndPoint e in playerEndpoint)
            {
                lobbySocket.SendTo(m_Card.GetBytes(), e);
            }

        }

        public static void SendPickCard(String p)
        {

            Message m_PickCard = new Message(Commande.PICKCARD, CommandeType.REPONSE, p, "server");
            foreach (EndPoint e in playerEndpoint)
            {
                lobbySocket.SendTo(m_PickCard.GetBytes(), e);
            }
        }

        public static void SendPlayCard(String card, String player)
        {
            String data = card + "|" + player;
            Message m_PickCard = new Message(Commande.PLAYCARD, CommandeType.REPONSE, data, "server");
            foreach (EndPoint e in playerEndpoint)
            {
                lobbySocket.SendTo(m_PickCard.GetBytes(), e);
            }
        }

   
        public static void SendEndOfRound(Player winner)
        {
            Message EndOfround = new Message(Commande.ROUNDOVER, CommandeType.REPONSE, winner.getName(), "server");
            foreach (EndPoint e in playerEndpoint)
            {
                lobbySocket.SendTo(EndOfround.GetBytes(), e);
            }
        }

        //CARD EFFECT ---------------------
        public static void GuardEffect(String source, String target, String targetCardValue, EndPoint endpoint)
        {
            Player s = Game.Instance.getPlayerInRoundByName(source);
            Player p = Game.Instance.getPlayerInRoundByName(target);
            Card c = Game.Instance.CreatCardByValue(int.Parse(targetCardValue));

            Message response = null;

            if (p.getCards().ElementAt(0).value == c.getValue())
            {
                response = new Message(Commande.GUARDEFFECT, CommandeType.REPONSE, "yes", "server");
                SendGuardEffet(s, c, p,true);
                killPlayer(p);
                //SendGuardEffectRes(s, true, c, p);
            }
            else
            {
                response = new Message(Commande.GUARDEFFECT, CommandeType.REPONSE, "no", "server");
                SendGuardEffet(s, c, p,false);
                //SendGuardEffectRes(s, false, c, p);
            }

            lobbySocket.SendTo(response.GetBytes(), endpoint);
      

            PlayMultiTurn();



            /*********************************************************/
        }

        public static void BarronEffect(String source, String target, EndPoint endpoint)
        {

            Player s = Game.Instance.getPlayerInRoundByName(source);
            Player t = Game.Instance.getPlayerInRoundByName(target);

            Message response = null;


            if (s.getCards().ElementAt(0).getValue() >
                t.getCards().ElementAt(0).getValue())
            {
                response = new Message(Commande.BARONEFFECT, CommandeType.REPONSE, "true", "server");           
                SendBaronEffect(s, t,t);
                Thread.Sleep(8000);
                killPlayer(t);
            }

            else if (t.getCards().ElementAt(0).getValue() >
                s.getCards().ElementAt(0).getValue())

            {
                response = new Message(Commande.BARONEFFECT, CommandeType.REPONSE, "false", "server");
                SendBaronEffect(s, t,s);
                Thread.Sleep(8000);
                killPlayer(t);
            }
            else
                response = new Message(Commande.BARONEFFECT, CommandeType.REPONSE, "", "server");
                SendBaronEffect(s, t,null);

            Thread.Sleep(100);
            lobbySocket.SendTo(response.GetBytes(), endpoint);

            PlayMultiTurn();

            /*********************************************************/
        }

        public static void PriestEffect(String target, String source, EndPoint endpoint)
        {

            Player p = Game.Instance.getPlayerInRoundByName(target);
            Player s = Game.Instance.getPlayerInRoundByName(source);

            String data = p.getName() + "|" + s.getName() + "|";
            foreach (Card c in p.getCards())
            {
                data += c.GetName() + "|";
            }
            data = data.Remove(data.Length - 1);
            Message response = new Message(Commande.PRIESTEFFECT, CommandeType.REPONSE, data, "server");

            lobbySocket.SendTo(response.GetBytes(), endpoint);
            SendPriestEffect(s, p);



            PlayMultiTurn();
            /*********************************************************/
        }

        public static void HandmaidEffect(String target, EndPoint endpoint)
        {

            Player p = Game.Instance.getPlayerInRoundByName(target);
            p.HMProtected = true;

            Message response = new Message(Commande.HANDMAIDEEFFECT, CommandeType.REPONSE, target, "server");

            lobbySocket.SendTo(response.GetBytes(), endpoint);

            Thread.Sleep(250);
            SendHandmaidEffect(p);


            PlayMultiTurn();
            /*********************************************************/
        }

        public static void PrinceEffect(String source, String target, EndPoint endpoint)
        {


            Player src = Game.Instance.getPlayerInRoundByName(source);
            Player trg = Game.Instance.getPlayerInRoundByName(target);




            Message response = new Message(Commande.PRINCEEFFECT, CommandeType.REPONSE, trg.getName(), "server");


            lobbySocket.SendTo(response.GetBytes(), endpoint);

            Card deletedCard = trg.player_Deck.ElementAt(0);



            SendPrinceEffect(src, trg);


            PlayMultiTurn();

            /*********************************************************/
        }

        public static void KingEffect(String source, String target, EndPoint endpoint)
        {



            Player p = Game.Instance.getPlayerInRoundByName(source);
            Player t = Game.Instance.getPlayerInRoundByName(target);


            Console.WriteLine("Main avant");
            Console.WriteLine(p.getCards().ElementAt(0).name);
            Console.WriteLine(t.getCards().ElementAt(0).name);


            Card p1 = p.getCard();
            Card p2 = t.getCard();

            p.getCards().Clear();
            p.getCards().Add(p2);

            t.getCards().Clear();
            t.getCards().Add(p1);

            Console.WriteLine("Main apres");
            Console.WriteLine(p.getCards().ElementAt(0).name);
            Console.WriteLine(t.getCards().ElementAt(0).name);

            Message response = new Message(Commande.KINGEFFECT, CommandeType.REPONSE, "", "server");

            lobbySocket.SendTo(response.GetBytes(), endpoint);
            Thread.Sleep(150);
            SendKingEffect(p, t);


            PlayMultiTurn();
            /*********************************************************/
        }
        //Effet princess
        public static void PrincessEffect(String target, EndPoint endpoint)
        {
            Player p = Game.Instance.getPlayerInRoundByName(target);
            killPlayer(p);
            SendPrincessEffect(p);

            PlayMultiTurn();
            /*********************************************************/
        }

        //Effet Countess
        public static void CountessEffect()
        {

            PlayMultiTurn();
            /*********************************************************/
        }

        public static void NoTarget()
        {
            PlayMultiTurn();
        }

        //-------------REFRESH OTHER PLAYER EFFECT--------------------//
        public static void SendGuardEffet(Player s, Card c, Player p,Boolean res)
        {
            String data = s.getName() + "|" + c.GetName() + "|" + p.getName() + "|" +res.ToString();
            Console.WriteLine(data);
            Message GuardEffect = new Message(Commande.GUARDEFFECT, CommandeType.REPONSE, data, "server");
            foreach (EndPoint e in playerEndpoint)
            {
                lobbySocket.SendTo(GuardEffect.GetBytes(), e);
            }
        }

     

        public static void SendPriestEffect(Player source, Player target)
        {
            Message PriestEffect = new Message(Commande.PRIESTEFFECT, CommandeType.REPONSE, source.getName() + "|" + target.getName(), "server");
            foreach (EndPoint e in playerEndpoint)
            {
                lobbySocket.SendTo(PriestEffect.GetBytes(), e);
            }
        }

        public static void SendBaronEffect(Player s, Player t,Player dead)
        {
            Message BaronEffect = null;
            if (dead == null)
            {
                BaronEffect = new Message(Commande.BARONEFFECT, CommandeType.REPONSE, s.getName() + "|" + t.getName() + "|" +"", "server");
            }
            else
            {
                BaronEffect = new Message(Commande.BARONEFFECT, CommandeType.REPONSE, s.getName() + "|" + t.getName() + "|" + dead.getName(), "server");
            }

            foreach (EndPoint e in playerEndpoint)
            {
                lobbySocket.SendTo(BaronEffect.GetBytes(), e);
            }
        }

        public static void SendBaronEffectRes(Player s, Player dead)
        {
            Message BaronEffect = null;
            if(dead==null)
            {
                 BaronEffect = new Message(Commande.BARONEFFECTRES, CommandeType.REPONSE, s.getName() + "|" + "", "server");
            }
            else
            {
                 BaronEffect = new Message(Commande.BARONEFFECTRES, CommandeType.REPONSE, s.getName() + "|" + dead.getName() + "|" + dead.getName(), "server");
            }
            
            foreach (EndPoint e in playerEndpoint)
            {
                lobbySocket.SendTo(BaronEffect.GetBytes(), e);
            }
        }

        public static void SendHandmaidEffect(Player p)
        {

            Message handMaidEffect = new Message(Commande.HANDMAIDEEFFECT, CommandeType.REPONSE, p.getName(), "server");
            foreach (EndPoint e in playerEndpoint)
            {
                lobbySocket.SendTo(handMaidEffect.GetBytes(), e);
            }
        }

        public static void SendPrinceEffect(Player s, Player t)
        {

            Message princeEffect = new Message(Commande.PRINCEEFFECTRES, CommandeType.REPONSE, s.getName() + "|" + t.getName(), "server");
            foreach (EndPoint e in playerEndpoint)
            {
                Thread.Sleep(100);
                lobbySocket.SendTo(princeEffect.GetBytes(), e);
                Console.WriteLine("PRINCE ENVOYE");
            }
        }

        public static void SendKingEffect(Player s, Player t)
        {

            Message KingEffect = new Message(Commande.KINGEFFECTRES, CommandeType.REPONSE, s.getName() + "|" + t.getName()+"|-------VIDE-----", "server");
            foreach (EndPoint e in playerEndpoint)
            {
                lobbySocket.SendTo(KingEffect.GetBytes(), e);
            }
        }

        public static void SendCountessEffect(Player p, Card c)
        {

            Message CountessEffect = new Message(Commande.COUNTESSEFFECTRES, CommandeType.REPONSE, p.getName(), " server");
            foreach (EndPoint e in playerEndpoint)
            {
                lobbySocket.SendTo(CountessEffect.GetBytes(), e);
            }

        }

        public static void SendPrincessEffect(Player p)
        {
            Message PrincessEffect = new Message(Commande.PRINCESSEFECT, CommandeType.REPONSE, p.getName(), "server");
            foreach (EndPoint e in playerEndpoint)
            {
                lobbySocket.SendTo(PrincessEffect.GetBytes(), e);
            }
        }

        public static void SendPlayCardVisual(Player p, Card c)
        {
            Message PlayCardVisual = new Message(Commande.PLAYCARDVISUALRES, CommandeType.REPONSE, p.getName()+"|"+c.GetName(), "server");
            foreach (EndPoint e in playerEndpoint)
            {
                lobbySocket.SendTo(PlayCardVisual.GetBytes(), e);
            }
        }

        public static void PlayCardVisual(String p, String value, EndPoint endPoint)
        {
            Player pl = Game.Instance.getPlayerInRoundByName(p);
            Card cr = Game.Instance.CreatCardByValue(int.Parse(value));

            Console.WriteLine(pl.getName() + "  " + cr.GetName());

            SendPlayCardVisual(pl, cr);

        }

        public static void SendGameIsOVer(Player p)
        {
            Message Winner = new Message(Commande.GAMEISOVER, CommandeType.REPONSE, p.getName(), "server");
            foreach (EndPoint e in playerEndpoint)
            {
                lobbySocket.SendTo(Winner.GetBytes(), e);
            }
        }

        public static void PlayerLeave()
        {
            Message PlayerLeave = new Message(Commande.PLAYERLEAVE, CommandeType.REPONSE, "", "server");
            foreach (EndPoint e in playerEndpoint)
            {
                lobbySocket.SendTo(PlayerLeave.GetBytes(), e);
            }
        }


        public static void AddPlayerEndPoint(EndPoint e)
        {
            if(!playerEndpoint.Contains(e))
            {
                Console.WriteLine("Player add ");
                playerEndpoint.Add(e);
            }
        }

        public static void ReadyToHaveCard(EndPoint e)
        {
            AddPlayerEndPoint(e);
            PlayerReady++;
            Console.WriteLine("En attente de joueur sur :" + PlayerReady + "-" + Game.Instance.getPlayer().Count);
            
            Console.WriteLine("Joueur connecté: "+PlayerReady);
           
            if (Game.Instance.getPlayer().Count == PlayerReady)
            {
                InitMulitPlayerRound();
                Console.WriteLine("La partie commence");
            }
            else{
                
                Console.WriteLine("Arrivé d'un joueur");
            }
        }

        public static void ReadyToRemoveCard(EndPoint e)
        {
            PlayerReadyToRemove++;
            Console.WriteLine("En attente de joueur sur :" + PlayerReadyToRemove + "-" + Game.Instance.getPlayer().Count);
            if (Game.Instance.getPlayer().Count == PlayerReadyToRemove)
            {
                RemoveReady();
                Console.WriteLine("remove commence");
            }
            else
            {

                Console.WriteLine("remove Arrivé d'un joueur");
            }
        }

    }
}
