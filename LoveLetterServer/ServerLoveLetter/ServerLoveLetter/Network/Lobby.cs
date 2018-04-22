using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoveLetter.Network
{
    public class Lobby
    {
        int maxPlayers;
        int portNum;
        List<Player> connectedPlayers;
        List<Message> msgList = new List<Message>();

        String name;
        Socket LobbySocket;
        List<EndPoint> playerSubScribe = new List<EndPoint>();
        EndPoint endPointPlayer;
        Boolean gameAsStart = false;

        public Lobby(int maxPlayers, String name, int portNum)
        {
            this.maxPlayers = maxPlayers;
            this.connectedPlayers = new List<Player>(maxPlayers);
            this.name = name;
            this.portNum = portNum;
            LobbyLoad();


            Thread ctThread = new Thread(Process);
            ctThread.Start();


        }

        public static Lobby createLobby(int maxPlayers, String name, int portNum)
        {
            Lobby lobby = new Lobby(maxPlayers, name, portNum);
            return lobby;


        }

        public void LobbyLoad()
        {
            LobbySocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            Console.WriteLine("Nouveau lobby chargé port: " + portNum);

            LobbySocket.Bind(new IPEndPoint(IPAddress.Any, portNum));

            //Ajout du socket au dictionnaireServeur
            Server.socketLobby.Add(this, LobbySocket);
        }

        public void Process()
        {
            while (true)
            {
                Console.WriteLine("Attente Message");
                ReceiveMsg();
            }
        }

        public void ReceiveMsg()
        {
            try
            {
                // Reception message client
                endPointPlayer = new IPEndPoint(IPAddress.Any, 0);



                byte[] buffer = new byte[Message.bufferSize];
                LobbySocket.ReceiveFrom(buffer, buffer.Length, SocketFlags.None, ref endPointPlayer);
                //addClient(lastClientEndPoint);

                Message msg = new Message(buffer);

                //Traitement du message
                ProcessMsg(msg, endPointPlayer);
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur receiveMsg: " + e.Message);
                Console.ReadKey();
            }
        }


        public void ProcessMsg(Message msg, EndPoint clientEP)
        {
            string dataMsg = "";
            string[] dataArray;
            try
            {
                Console.WriteLine("Commande: " + msg.commande);
                switch (msg.commande)
                {
                    case Commande.LEAVELOBBY:
                        playerSubScribe.Remove(endPointPlayer);
                        Player p =getPlayer(msg.pseudo);
                        RemovePlayerLobby(p);
                        Server.RefreshList(this.getName());
                        Refresh();
                        break;

                    case Commande.ASKLOBBY:
                        addClient(endPointPlayer);
                        askPlayerInLobby();
                          
                        break;


                    case Commande.FREETHREAD:
                        FreeThread(msg.data);
                        break;

                    case Commande.DESUBSCRIBE:
                        playerSubScribe.Remove(endPointPlayer);
                        break;

                    case Commande.MSG:
                        msgList.Add(msg);
                        Console.WriteLine("Dans Lobby"+msg.data);

                        dataMsg = msg.data;
                        dataArray = dataMsg.Split('|');
     
                        Message chat_SendMessageLobbyClient = new Message(Commande.MSG, CommandeType.REPONSE, msgList.ElementAt(msgList.Count - 1).data, msg.pseudo);
                        foreach (EndPoint e in playerSubScribe)
                        {
                           
                        LobbySocket.SendTo(chat_SendMessageLobbyClient.GetBytes(), e);
                        }
                        break;

                    case Commande.BEGIN:
                        CheckBegin();
                        break;

                    case Commande.GO:
                        Console.WriteLine("GAME AS START: " + gameAsStart.ToString());
                        if (!gameAsStart)
                        {
                            
                            InitGame();
                            gameAsStart = true;
                        }
                        break;

                    case Commande.PICKCARD:
                        GameControllerServer.PlayerPickCard(msg.data,msg.pseudo);
                        break;

                    case Commande.ASKCARD:               
                        break;

                    case Commande.PLAYCARD:
                        GameControllerServer.PlayCard(msg.data, msg.pseudo);
                        break;

                    //EFFECT

                    case Commande.GUARDEFFECT:
                        String[] guardArray = msg.data.Split('|');
                        GameControllerServer.GuardEffect(guardArray[0], guardArray[1],guardArray[2],endPointPlayer);
                        break;

                    case Commande.PRIESTEFFECT:
                        GameControllerServer.PriestEffect(msg.data,msg.pseudo, endPointPlayer);
                        break;

                    case Commande.BARONEFFECT:
                        GameControllerServer.BarronEffect(msg.pseudo, msg.data, endPointPlayer);
                        break;


                    case Commande.HANDMAIDEEFFECT:
                        GameControllerServer.HandmaidEffect(msg.data, endPointPlayer);
                        break;

                    case Commande.PRINCEEFFECT:
                        GameControllerServer.PrinceEffect(msg.pseudo, msg.data, endPointPlayer);
                        break;

                    case Commande.KINGEFFECT:
                        GameControllerServer.KingEffect(msg.pseudo, msg.data, endPointPlayer);
                        break;
                        
          
                    case Commande.PRINCESSEFECT:
                        GameControllerServer.PrincessEffect(msg.pseudo, endPointPlayer);
                        break;

                    case Commande.COUNTESSEFFECT:
                        GameControllerServer.CountessEffect();
                        break;

                    case Commande.PLAYCARDVISUAL:
                        GameControllerServer.PlayCardVisual(msg.pseudo, msg.data, endPointPlayer);
                        break;

                    case Commande.NOVALIDTARGET:
                        GameControllerServer.NoTarget();
                        break;

                    case Commande.ASKDECK:
                        GameControllerServer.PlayerAskDeck();
                        break;

                    case Commande.PLAYERLEAVE:
                        GameControllerServer.PlayerLeave();
                        break;

                    case Commande.READYTOHAVECARD:
                        GameControllerServer.ReadyToHaveCard(endPointPlayer);
                        break;

                    case Commande.READYTOREMOVECARD:
                        GameControllerServer.ReadyToRemoveCard(endPointPlayer);
                        break;



                    default:
                        Console.WriteLine("Erreur commande inconnu: "+msg.commande);
                        break;


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }

        static void LeaveLobby(Player p)
        {     
            Console.Write("Player Leave Lobby: " + p.getName());
        }

        

        public void Refresh()
        {
            String msg = connectedPlayers.Count + "|";

            foreach (Player p in connectedPlayers)
            {
                msg += p.getName() + "|";
            }

            foreach (EndPoint e in playerSubScribe)
            {
                Message chat_nameLobby = new Message(Commande.REFRESH, CommandeType.REPONSE,  msg, "server");
                LobbySocket.SendTo(chat_nameLobby.GetBytes(), e);
            }
        }

        public void CheckBegin()
        {
            if(connectedPlayers.Count()==maxPlayers)
            {
                foreach (EndPoint e in playerSubScribe)
                {
                    
                    Message chat_begin = new Message(Commande.BEGIN, CommandeType.REPONSE,"ok", "server");
                    LobbySocket.SendTo(chat_begin.GetBytes(), e);
                    
                }
            }
            else
            {
                Message chat_begin = new Message(Commande.BEGIN, CommandeType.REPONSE, "no", "server");
                LobbySocket.SendTo(chat_begin.GetBytes(), endPointPlayer);
            }
        }
        public void askPlayerInLobby()
        {

            Console.WriteLine("Un client a fait une demande de joueur dans le lobby: " + portNum);
            //Envoie message Server
            String msg = connectedPlayers.Count + "|";

            

            foreach (Player p in connectedPlayers)
            {
                msg += p.getName() + "|";
            }


            
            Message chat_nameLobby = new Message(Commande.REFRESH, CommandeType.REPONSE, msg + "", "");
            LobbySocket.SendTo(chat_nameLobby.GetBytes(), endPointPlayer);
           
        }

        private void InitGame()
        {
            Console.WriteLine("GAME INIT");
            GameControllerServer.newGame(maxPlayers, connectedPlayers, playerSubScribe,LobbySocket);
        }


        public void FreeThread(String data)
        {
            if(data== "gameform")
            {
                Console.WriteLine("IN FREE GAME FORM");
                Message chat_nameLobby = new Message(Commande.FREETHREAD, CommandeType.REPONSE, "gameform", "server");
                LobbySocket.SendTo(chat_nameLobby.GetBytes(), endPointPlayer);
            }
            else
            {
                Message chat_nameLobby = new Message(Commande.FREETHREAD, CommandeType.REPONSE, "", "server");
                LobbySocket.SendTo(chat_nameLobby.GetBytes(), endPointPlayer);
            }
            
            
           
        }

        public String getName()
        {
            return name;
        }

        public int getMaxPlayer()
        {
            return maxPlayers;
        }

        public List<Player> getPlayerConnected()
        {
            return this.connectedPlayers;
        }

        public void AddPlayerToLobby(String name)
        {
            connectedPlayers.Add(CreatePlayer(name));
            Console.WriteLine("MMM PLAYER AJOUTE");
        }

        public int getPort()
        {
            return portNum;
        }

        public void RemovePlayerLobby(Player p)
        {
            if (p != null)
            {
                connectedPlayers.Remove(p);
            }

        }

        public Player getPlayer(String name)
        {
            Player p = null;
            foreach (Player player in connectedPlayers)
            {
                Console.WriteLine("Compare " + player.getName().ToUpper() + " / " + name.ToUpper());
                if (player.getName().ToUpper().Equals(name.ToUpper()))
                {
                    p = player;
                    return p;
                }
                else
                {
                    Console.WriteLine("Aucun joueur avec ce nom");
                    
                }

            }
            return p;
        }

        public void addClient(EndPoint e)
        {
            if (!playerSubScribe.Contains(e))
            {
                playerSubScribe.Add(e);
                Console.WriteLine("Client ajouté");
            }
        }

        private Player CreatePlayer(String name)
        {
            return new Player(name);
        }
    }
}
