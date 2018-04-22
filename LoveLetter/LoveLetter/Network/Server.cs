using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LoveLetter.Network
{
    public class Server
    {
        static int numPortServer = 16789;
        public static String serverIp = "89.89.17.60";
        static int nextLobbyPort;
        static Socket serverSocket;
        static List<Lobby> lobbyConnected = new List<Lobby>();
        static List<Message> messageList = new List<Message>();
        static List<EndPoint> clientEndPoint = new List<EndPoint>();
        public static EndPoint lastClientEndPoint;

        public static Dictionary<Lobby, Socket> socketLobby = new Dictionary<Lobby, Socket>();

        /*
        public static void Main(string[] args)
        {
            //Serveur 
            Server.Launch();
            Server.Process();


            Console.ReadKey();
        }
        */

        public static void Launch()
        {
            try
            {
                Console.WriteLine("Demarage Serveur");
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Tcp);

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
            ShutDown();

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
           
           
                // Reception message client
                lastClientEndPoint = new IPEndPoint(IPAddress.Any, 0);



                byte[] buffer = new byte[Message.bufferSize];

                serverSocket.ReceiveFrom(buffer, buffer.Length, SocketFlags.None, ref lastClientEndPoint);
                addClient(lastClientEndPoint);

                Message msg = new Message(buffer);

                //Traitement du message
                ProcessMsg(msg, lastClientEndPoint);
            
          



        }

        //Envoie de Message
        static void SendMsg()
        {

        }


        //Traitement des message
        static void ProcessMsg(Message msg, EndPoint clientEP)
        {
            string dataMsg = "";
            string[] dataArray;
            try
            {
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
                        dataArray = dataMsg.Split('|');
                        JoinLobby(dataArray[0], dataArray[1]);

                        break;

                    case Commande.LEAVELOBBY:
                        dataMsg = msg.data;
                        dataArray = dataMsg.Split('|');
                        Console.Write(dataArray[0] + " fsdfsf");
                        LeaveLobby(dataArray[0], dataArray[1]);
                        break;


                    case Commande.PLAYCARD:
                        break;

                    case Commande.LISTLOBBY:
                        SendLobbyList();
                        break;

                    case Commande.MSG:
                        messageList.Add(msg);
                        Console.WriteLine(msg.data);

                        //Renvoie du dernier message au client
                        Message chat_SendMessageClient = new Message(Commande.MSG, CommandeType.REPONSE, messageList.ElementAt(messageList.Count - 1).data, msg.pseudo);


                        foreach (EndPoint e in clientEndPoint)
                        {
                            serverSocket.SendTo(chat_SendMessageClient.GetBytes(), e);
                        }


                        Console.WriteLine("Message envoyé");
                        break;

                    default:
                        Console.WriteLine("Erreur");
                        break;


                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur process msg: " + e.Message);
                Console.ReadKey();
            }

        }

        //Gestion lobby
        static void createLobby(String lobbyName, int maxPlayer)
        {
            try
            {
                Console.WriteLine("Creation d'un lobby");
                Lobby lobby = new Lobby(maxPlayer, lobbyName, GetValidPort());
                lobbyConnected.Add(lobby);


            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur creation Lobby: " + e.Message);
                Console.ReadKey();
            }
        }

        public static void JoinLobby(String lobbyName, String playerName)
        {
            Lobby lobby = GetLobbyByName(lobbyName);

            if (lobby != null)
            {
                lobby.AddPlayerToLobby(playerName);
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
                    + lobbyCurrent.getMaxPlayer()
                    
                    + "|";
            }

            Console.WriteLine(msg);
            Message chat_nameLobby = new Message(Commande.MSG, CommandeType.REPONSE, msg + "", "");
            serverSocket.SendTo(chat_nameLobby.GetBytes(), lastClientEndPoint);
            Console.WriteLine("Message envoyé");
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
            Lobby lb=GetLobbyByName(name);
            Player p = getPlayer(player, lb);
            lb.getPlayerConnected().Remove(p);

        }

        public static Player getPlayer(String name, Lobby lb)
        {
            Player p = null;
            foreach (Player play in lb.getPlayerConnected())
            {
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
            return nextLobbyPort + 1;
        }

    }
}