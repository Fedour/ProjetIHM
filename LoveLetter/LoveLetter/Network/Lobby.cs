using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LoveLetter.Network
{
    public class Lobby
    {
        int maxPlayers;
        int portNum;
        List<Player> connectedPlayers;
        String name;
        Socket LobbySocket;
        List<EndPoint> playerEndpoint = new List<EndPoint>();
        EndPoint endPointPlayer;

        public Lobby(int maxPlayers, String name, int portNum)
        {
            this.maxPlayers = maxPlayers;
            this.connectedPlayers = new List<Player>(maxPlayers);
            this.name = name;
            this.portNum = portNum;
        }

        public static Lobby createLobby(int maxPlayers, String name, int portNum)
        {
            Lobby lobby = new Lobby(maxPlayers, name, portNum);
            return lobby;

            
        }

        public void LobbyLoad()
        {
            LobbySocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
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
           
            try
            {
                switch (msg.commande)
                {
        

                    case Commande.ASKLOBBY:
                        askPlayerInLobby();
                        break;

                    case Commande.LEAVELOBBY:
                        getPlayer(msg.pseudo);
                        break;

                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            
        }

        public void askPlayerInLobby()
        {

            Console.WriteLine("Un client a fait une demande de joueur dans le lobby: "+portNum);
            //Envoie message Server
            String msg = connectedPlayers.Count + "|";

            Lobby lobbyCurrent;

            foreach(Player p in connectedPlayers)
            {
                msg += p.getName() + "|";
            }


            Console.WriteLine(msg);
            Message chat_nameLobby = new Message(Commande.MSG, CommandeType.REPONSE, msg + "", "");
            LobbySocket.SendTo(chat_nameLobby.GetBytes(), endPointPlayer);
            Console.WriteLine("Message envoyé sur port "+portNum);
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
        }

        public void RemovePlayerLobby(Player p)
        {
            if(p!=null)
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

        private Player CreatePlayer(String name)
        {
            return new Player(name);
        }
    }
}
