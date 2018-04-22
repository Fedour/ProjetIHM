using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LoveLetter.Network
{
    public class Client
    {
        static EndPoint server;
        static bool createRoom;

        static int port=16789;
        static String ServerAdress= "192.168.1.100";

        // Création du EndPoint serveur
        
        Stream stm;
        static Player ClientPlayer;


        //Creation Socket TCP
        TcpClient clientTCP;


        public Client(Player client)
        {
            ClientPlayer = client;

            clientTCP = new TcpClient();

            //Connexion
            clientTCP.Connect(ServerAdress, port);
            stm = clientTCP.GetStream();
            
        }

        static string getMessage()
        {
            String smg = String.Empty;
            bool messageValide = true;

            while (messageValide)
            {
                //TODO
            }

            return "rien";
        }

        static void DisplayMenu()
        {
            int actionInt;
            if (createRoom)
            {
                Console.WriteLine("1: Jouer");
                Console.WriteLine("2: Envoie msg");
                actionInt = int.Parse(Console.ReadLine());
            }
            else
            {
                Console.WriteLine("1: Creer lobby");
                actionInt = int.Parse(Console.ReadLine());
            }

            Action(actionInt);
        }

        public void ClientSendMsgText()
        {
            Console.WriteLine("Votre message ?");
            String data = Console.ReadLine();

            //Envoie message Server

            Message chat_nameLobby = new Message(Commande.MSG, CommandeType.REQUETE, data, ClientPlayer.getName());
            byte[] buffer_room = chat_nameLobby.GetBytes();
            stm.Write(buffer_room, 0, buffer_room.Length);
        }

        static void ClientCreateLobby()
        {
            Console.Write("Nom lobby ?: ");
            String name = Console.ReadLine();

            Console.Write("Nombre joueur max ?: ");
            int nbPlayer = int.Parse(Console.ReadLine());

            //Envoie message Server

            String data = name + "|" + nbPlayer;

            Message chat_nameLobby = new Message(Commande.CREATELOBBY, CommandeType.REQUETE, data, "");
            byte[] buffer_room = chat_nameLobby.GetBytes();
            //clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);

        }

        static void ClientJoinLobby(Player player)
        {
            Console.Write("Nom lobby a rejoindre?: ");
            String nameLobby = Console.ReadLine();


            //Envoie message Server
            String data = nameLobby + "||" + player.getName();

            Message chat_nameLobby = new Message(Commande.JOINLOBBY, CommandeType.REQUETE, data, "");
            byte[] buffer_room = chat_nameLobby.GetBytes();
            //clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);
        }

        

        static void ClientPlayCard(Player player, Card cardPlay)
        {
            String data = player.getName();

            String cardName = cardPlay.GetName();
            CardPlay cardToSend;
            switch (cardName.ToUpper())
            {
                case "GUARD":
                    cardToSend = CardPlay.GUARD;
                    break;
                case "BARON":
                    cardToSend = CardPlay.BARON;
                    break;
                case "PRIEST":
                    cardToSend = CardPlay.PRIEST;
                    break;
                case "HANDMAID":
                    cardToSend = CardPlay.HANDMAID;
                    break;
                case "KING":
                    cardToSend = CardPlay.KING;
                    break;
                case "PRINCE":
                    cardToSend = CardPlay.PRINCE;
                    break;
                case "COUNTESS":
                    cardToSend = CardPlay.COUNTESS;
                    break;
                case "PRINCESS":
                    cardToSend = CardPlay.PRINCESS;
                    break;

                default:
                    Console.WriteLine("Carte non reconnu");
                    break;


            }
        }

        public void close()
        {
            clientTCP.Close();
        }

        //Attend action utilisateur
        static void Action(int action)
        {
            if (createRoom)
            {
                switch (action)
                {
                    case 1:
                        
                        break;
                    case 2:
                        //ClientSendMsg();
                        break;

                }
            }

            else
            {
                switch (action)
                {
                    case 1:
                        ClientCreateLobby();
                        break;

                }
            }
        }
    }

    
}
