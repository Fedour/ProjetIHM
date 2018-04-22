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
    public class Server
    {
        static int numPortServer = 16789;
        public static String serverIp = "192.168.1.100";
        static int nextLobbyPort = numPortServer;
        static Socket serverSocket;
        static List<Lobby> lobbyConnected = new List<Lobby>();
        static List<Message> messageList = new List<Message>();
        static List<EndPoint> clientEndPoint = new List<EndPoint>();
        static List<String> pseudoList = new List<String>();
        static EndPoint lastClientEndPoint;
        static bool CountessBlock = false;

        public static Dictionary<Lobby, Socket> socketLobby = new Dictionary<Lobby, Socket>();
        



        public static void Main(string[] args)
        {
            //Serveur 
            Server.Launch();
            Server.Process();


        }
        

        public static void Launch()
        {
            try
            {
                Console.WriteLine("Demarage Serveur");
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                int num_port = numPortServer;
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, num_port));

                Console.WriteLine("Demarage OK");


            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur initialisation " + e.Message);
                Console.ReadKey();
            }

        }
        static void addClient(EndPoint e)
        {
            if (!clientEndPoint.Contains(e))
            {
                clientEndPoint.Add(e);
                Console.WriteLine("Client ajouté");
            }
        }
        static void Process()
        {
            while (true)
            {
                Console.WriteLine("Attente Message");
                ReceiveMsg();
            }
            

        }


        //Fermeture Server
        static void ShutDown()
        {
            Console.WriteLine("Fermeture Socket...");
            serverSocket.Close();

        }

        //Reception de Message
        static void ReceiveMsg()
        { 

            try
            {
                byte[] buffer = new byte[Message.bufferSize];
                lastClientEndPoint = new IPEndPoint(IPAddress.Any, 0);

                //Attend requete Client
                serverSocket.ReceiveFrom(buffer, buffer.Length, SocketFlags.None, ref lastClientEndPoint);
                

                Message msg = new Message(buffer);

                //Traitement du message
                ProcessMsg(msg, lastClientEndPoint);




            }
            catch(Exception e)
            {
                //Reprise sur panne
                Console.WriteLine("Exception" + e);
                ReceiveMsg();
            }

        }

        //Traitement des message
        static void ProcessMsg(Message msg, EndPoint clientEP)
        {
            string dataMsg = "";
            string[] dataArray;
            try
            {
                Console.WriteLine(" commande: " + msg.commande.ToString());
                switch (msg.commande)
                {
             
                    case Commande.CREATELOBBY:
                        dataMsg = msg.data;

                        //Split nom + player room
                        dataArray = dataMsg.Split('|');

                        createLobby(dataArray[0], int.Parse(dataArray[1]));
                        break;

                    case Commande.JOINLOBBY:
                        dataMsg = msg.data;
                        Console.WriteLine("Le joueur a rejoint: " + msg
                            .data
                            );
                        dataArray = dataMsg.Split('|');
                        JoinLobby(dataArray[0], dataArray[1]);

                        break;

                    case Commande.LEAVELOBBY:
                        dataMsg = msg.data;
                        dataArray = dataMsg.Split('|');
                        
                        LeaveLobby(dataArray[0], dataArray[1]);
                        break;

                    case Commande.DESUBSCRIBE:
                        
                        clientEndPoint.Remove(lastClientEndPoint);
                        break;


                    case Commande.PLAYCARD:
                        
                        
                    case Commande.SUBSCRIBE:
                        addClient(lastClientEndPoint);
                        break;

                    case Commande.LISTLOBBY:
                        SendLobbyList();
                        break;

                    case Commande.FREETHREAD:
                        FreeThread();
                        break;


                    case Commande.MSG:
                        messageList.Add(msg);
                        Console.WriteLine(msg.data);

                        dataMsg = msg.data;
                        dataArray = dataMsg.Split('|');
                        

                        if(dataArray.Length==1)
                        {
                            //Renvoie du dernier message au client
                            Message chat_SendMessageClient = new Message(Commande.MSG, CommandeType.REPONSE, messageList.ElementAt(messageList.Count - 1).data, msg.pseudo);
                            foreach (EndPoint e in clientEndPoint)
                            {
                                serverSocket.SendTo(chat_SendMessageClient.GetBytes(), e);
                            }
                        }
                        else
                        {
                            Lobby lob = GetLobbyByName(dataArray[0]);
                            Message chat_SendMessageLobbyClient = new Message(Commande.MSG, CommandeType.REPONSE, messageList.ElementAt(messageList.Count - 1).data, msg.pseudo);
                            foreach (EndPoint e in clientEndPoint)
                            {
                                serverSocket.SendTo(chat_SendMessageLobbyClient.GetBytes(), e);
                            }
                        }

                      
                        break;

                    case Commande.ASKLOBBY:
                        Lobby lb = GetLobbyByName(msg.data);
                        

                        break;

                    case Commande.ASKLOBBYPORT:
                        Lobby lobby = GetLobbyByName(msg.data);
                        Message chat_SendResponse = new Message(Commande.ASKLOBBYPORT, CommandeType.REPONSE,lobby.getPort()+"", msg.pseudo);
                        serverSocket.SendTo(chat_SendResponse.GetBytes(), lastClientEndPoint);


                        break;

                    case Commande.CHECKNAME:
                        String res = "";
                        if (pseudoList.Contains(msg.pseudo.ToLower()))
                            res = "true";
                        else
                        {
                            res = "false";
                            pseudoList.Add(msg.pseudo.ToLower());
                        }

                        Message m_CheckPseudo = new Message(Commande.ASKLOBBYPORT, CommandeType.REPONSE ,res, msg.pseudo);
                        serverSocket.SendTo(m_CheckPseudo.GetBytes(), lastClientEndPoint);
                        break;

                    case Commande.DELETENAME:
                        pseudoList.Remove(msg.data.ToLower());
                        break;



                    default:
                        Console.WriteLine("Erreur commande inconnu: "+msg.commande.ToString());
                        break;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur process msg: " + e.Message);
                Console.ReadKey();
            }

        }

        //Envoie message Client
        static void sendResponseClient(String data)
        {


        }

        static void FreeThread()
        {
            Console.WriteLine("Free thread attend");
            Message chat_nameLobby = new Message(Commande.FREETHREAD, CommandeType.REPONSE,  "", "server");
            serverSocket.SendTo(chat_nameLobby.GetBytes(),  lastClientEndPoint);
            Console.WriteLine("Free thread envoyé");
        }

        //Gestion lobby
        static void createLobby(String lobbyName, int maxPlayer)
        {
            try
            {
                Console.WriteLine("Creation d'un lobby");
                Lobby lobby = new Lobby(maxPlayer, lobbyName, GetValidPort());
                lobbyConnected.Add(lobby);
            
                RefreshList(lobbyName);


            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur creation Lobby: " + e.Message);
                Console.ReadKey();
            }
        }

        public static void RefreshList(String lobbyName)
        {
            Console.WriteLine("Refresh List");
            
            Lobby lobby = GetLobbyByName(lobbyName);

            if (lobby != null)
            {
                
                String msg = lobbyConnected.Count + "|";

                Lobby lobbyCurrent;

                for (int i = 0; i < lobbyConnected.Count(); i++)
                {
                    lobbyCurrent = lobbyConnected.ElementAt(i);
                    msg += lobbyCurrent.getName() + "&"
                        + lobbyCurrent.getPlayerConnected().Count() + "&"
                        + lobbyCurrent.getMaxPlayer() + "&"
                        + lobbyCurrent.getPort()
                        + "|";
                }

                Console.WriteLine("DEMANDE DE REFRESHHHHHHHHHHH");
                Message chat_nameLobby = new Message(Commande.REFRESH, CommandeType.REPONSE, msg + "", "server");

                
                foreach (EndPoint e in clientEndPoint)

                    serverSocket.SendTo(chat_nameLobby.GetBytes(), e);
                    
            }
            
        }

        public static void JoinLobby(String lobbyName, String playerName)
        {
            
            
            Lobby lobby = GetLobbyByName(lobbyName);

            if (lobby != null)
            {
                lobby.AddPlayerToLobby(playerName);
                
                RefreshList(lobbyName);
                lobby.Refresh();
                
               
               
            }
            else
            {
                Console.WriteLine("Lobby Introuvable");
            }
            
        }

        //Envoie un message au client contenant la liste des lobby
        public static void SendLobbyList()
        {

            Console.WriteLine("Un client a fait une demande de ListLobby");
            //Envoie message Server
            String msg = lobbyConnected.Count + "|";

            Lobby lobbyCurrent;

            for (int i = 0; i < lobbyConnected.Count(); i++)
            {
                lobbyCurrent = lobbyConnected.ElementAt(i);
                msg += lobbyCurrent.getName() + "&"
                    + lobbyCurrent.getPlayerConnected().Count() + "&"
                    + lobbyCurrent.getMaxPlayer() + "&"
                    + lobbyCurrent.getPort()
                    + "|";
            }

            Console.WriteLine(msg);
            Message chat_nameLobby = new Message(Commande.MSG, CommandeType.REPONSE, msg + "", "");
            serverSocket.SendTo(chat_nameLobby.GetBytes(), lastClientEndPoint);
            Console.WriteLine("Message envoyé");
        }

        public static void getLobbyList()
        {

        }

        public static Lobby GetLobbyByName(String name)
        {
            Lobby res = null;
            foreach (Lobby lb in lobbyConnected)
            {
                Console.WriteLine("Compare " + lb.getName().ToUpper() + " / " + name.ToUpper());
                if (lb.getName().ToUpper().Equals(name.ToUpper()))
                {
                    res = lb;
                    return res;
                }
                else
                {
                    Console.WriteLine("Aucun lobby avec ce nom");
                }

            }
            return res;
        }


        static void LeaveLobby(String name, String player)
        {
            Lobby lb = GetLobbyByName(name);
            Console.Write("Player Leave Lobby: " + player);
            Player p = getPlayer(player, lb);
            lb.getPlayerConnected().Remove(p);


            RefreshList(name);
            lb.Refresh();

        }

        public static Player getPlayer(String name, Lobby lb)
        {
            Player p = null;
            foreach (Player play in lb.getPlayerConnected())
            {
                Console.WriteLine(play.getName() + "###");
                Console.WriteLine("Compare  " + play.getName().ToUpper() + " / " + name.ToUpper());
                if (play.getName().ToUpper().Equals(name.ToUpper()))
                {
                    p = play;
                    return p;
                }
                else
                {
                    Console.WriteLine("Aucun joueur avec ce nom");
                }

            }
            return p;
        }


        static int GetValidPort()
        {
            //A securiser
            return nextLobbyPort += 1;
        }

    }
}