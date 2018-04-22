using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoveLetter.Network
{
    public class Client
    {

        static bool createRoom;

        static int port = 16789;

        static string serverIP = Properties.Settings.Default.IpServer;


        // Création du EndPoint serveur


        static Player ClientPlayer;


        //Creation Socket TCP
        static EndPoint serverEP;
        static Socket clientSocket;


        //Constructeur par defaut connexion serveur principal
        public Client(Player client)
        {
            ClientPlayer = client;
            clientSocket = clientSocket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Dgram,
                    ProtocolType.Udp);

            clientSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
            clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);



            serverEP = new IPEndPoint(IPAddress.Parse(serverIP), port);


        }

        //Constructeur connexion port specifique
        public Client(Player client, int serverPort)
        {

            //Creation socket
            ClientPlayer = client;
            clientSocket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Dgram,
                    ProtocolType.Udp);

            clientSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
            clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);


            //Endpoint server
            serverEP = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
        }

        //Envoie d'un message texte
        public void ClientSendMsgText(String texte)
        {
            //Envoie message Server

            Message chat_nameLobby = new Message(Commande.MSG, CommandeType.REQUETE, texte, ClientPlayer.getName());
            clientSocket.SendTo(chat_nameLobby.GetBytes(), serverEP);
        }

        //Demande la liste de tout les joueur d'un lobby
        public String ClientAskPlayerInLobby(String lobby)
        {

            Message chat_nameLobby = new Message(Commande.ASKLOBBY, CommandeType.REQUETE, lobby, ClientPlayer.getName());
            clientSocket.SendTo(chat_nameLobby.GetBytes(), serverEP);

            //Reception Message
            byte[] bufferRes = new byte[Message.bufferSize];
            clientSocket.ReceiveFrom(bufferRes, bufferRes.Length, SocketFlags.None, ref serverEP);
            Message msg = new Message(bufferRes);

            return msg.data;

        }


        //Recois la reponse du serveur
        public Message ClientReceiveMsgText()
        {
            Message res = null;
            try
            {


                byte[] bufferRes = new byte[Message.bufferSize];
                clientSocket.ReceiveFrom(bufferRes, bufferRes.Length, SocketFlags.None, ref serverEP);
                res = new Message(bufferRes);

                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return res;
            }

        }

        //Fonction test creation lobby
        public void ClientCreateLobby()
        {
            Console.Write("Nom lobby ?: ");
            String name = Console.ReadLine();

            Console.Write("Nombre joueur max ?: ");
            int nbPlayer = int.Parse(Console.ReadLine());

            //Envoie message Server

            String data = name + "|" + nbPlayer;

            Message chat_nameLobby = new Message(Commande.CREATELOBBY, CommandeType.REQUETE, data, "");
            byte[] buffer_room = chat_nameLobby.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);


        }

        public void ClientCreateLobby(String name, String nbPlayer)
        {

            //Envoie message Server

            String data = name + "|" + nbPlayer;

            Message chat_nameLobby = new Message(Commande.CREATELOBBY, CommandeType.REQUETE, data, "");
            byte[] buffer_room = chat_nameLobby.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);



        }

        //Verifie si un nom est deja utilisé
        public static Boolean CheckForName(String name)
        {
            Client temp = new Client(new Player("temp"));
            Message chat_nameLobby = new Message(Commande.CHECKNAME, CommandeType.REQUETE, name, name);
            byte[] buffer_room = chat_nameLobby.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);

            byte[] bufferRes = new byte[Message.bufferSize];
            clientSocket.ReceiveFrom(bufferRes, bufferRes.Length, SocketFlags.None, ref serverEP);
            Message msg = new Message(bufferRes);
            temp = null;

            if (msg.data.ToLower()=="true")
            {
                return true;
            }
            else
            {
                return false;
            }
          

        }

        public void ClientFreeName(String name)
        {
            Message chat_nameLobby = new Message(Commande.DELETENAME, CommandeType.REQUETE, name, name);
            byte[] buffer_room = chat_nameLobby.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);
        }
        public void ClientJoinLobby()
        {
            Console.Write("Nom lobby a rejoindre?: ");
            String nameLobby = Console.ReadLine();

            Console.Write("Votre nom ?");
            String name = Console.ReadLine();


            //Envoie message Server
            String data = nameLobby + "|" + name;

            Message chat_nameLobby = new Message(Commande.JOINLOBBY, CommandeType.REQUETE, data, "");
            byte[] buffer_room = chat_nameLobby.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);
        }

        public void SubscribeClient()
        {
            Message chat_nameLobby = new Message(Commande.SUBSCRIBE, CommandeType.REQUETE, "", "");
            byte[] buffer_room = chat_nameLobby.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);
        }

        public void ClientJoinLobby(String lobby)
        {
            String data = lobby + "|" + ClientPlayer.getName();
            Console.WriteLine("Join Lobby" + data);

            Message chat_nameLobby = new Message(Commande.JOINLOBBY, CommandeType.REQUETE, data, "");
            byte[] buffer_room = chat_nameLobby.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);

        }

        public void ClientLeaveLobby(String lobby)
        {
            String data = lobby + "|" + ClientPlayer.getName();
            Console.WriteLine(data);

            Message chat_nameLobby = new Message(Commande.LEAVELOBBY, CommandeType.REQUETE, data, ClientPlayer.getName());
            byte[] buffer_room = chat_nameLobby.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);
        }


        public string ClientAskLobby()
        {
            String res = "";

            Message chat_nameLobby = new Message(Commande.LISTLOBBY, CommandeType.REQUETE, "", "");
            byte[] buffer_room = chat_nameLobby.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);


            //Attente de la reponse
            byte[] bufferRes = new byte[Message.bufferSize];


            Console.WriteLine("Message ask lobby en attente");
            clientSocket.ReceiveFrom(bufferRes, bufferRes.Length, SocketFlags.None, ref serverEP);

            Console.WriteLine("Message ask lobby recu");

            Message msg = new Message(bufferRes);
            Console.WriteLine(msg.data);
            String dataMsg = msg.data;


            return dataMsg;
        }


        public int ClientAskLobbyPort(String lbName)
        {
            Message chat_AskLobby = new Message(Commande.ASKLOBBYPORT, CommandeType.REQUETE, lbName, "");
            byte[] buffer_room = chat_AskLobby.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);

            byte[] bufferRes = new byte[Message.bufferSize];


            Console.WriteLine("Message askPort en attente");
            clientSocket.ReceiveFrom(bufferRes, bufferRes.Length, SocketFlags.None, ref serverEP);
            Message msg = new Message(bufferRes);
            Console.WriteLine("Message askPort recu");
            Console.WriteLine("Ask port recu " + msg.data);
            return 16790;



        }
        public void ClientUnsubscribe()
        {
            Message chat_Unsubscribe = new Message(Commande.DESUBSCRIBE, CommandeType.REQUETE, "", ClientPlayer.getName());
            byte[] buffer_room = chat_Unsubscribe.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);

        }

        public void ClientBeginGame()
        {
            Message chat_askBegin = new Message(Commande.BEGIN, CommandeType.REQUETE, "", ClientPlayer.getName());
            byte[] buffer_room = chat_askBegin.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);

        }

        public void ClientReadyBegin()
        {
            Message chat_Begin = new Message(Commande.GO, CommandeType.REQUETE, "", ClientPlayer.getName());
            byte[] buffer_room = chat_Begin.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);
        }

        public void ClientReadyToHaveCard()
        {
            Message m_readyCard = new Message(Commande.READYTOHAVECARD, CommandeType.REQUETE, "", ClientPlayer.getName());
            byte[] buffer_room = m_readyCard.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);
        }

        public void ClientReadyToRemoveCard()
        {
            Message m_readyRemove = new Message(Commande.READYTOREMOVECARD, CommandeType.REQUETE, "", ClientPlayer.getName());
            byte[] buffer_room = m_readyRemove.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);
        }

        public Card ClientAskCard()
        {


            Message m_askCard = new Message(Commande.ASKCARD, CommandeType.REQUETE, "", ClientPlayer.getName());
            byte[] buffer_room = m_askCard.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);

            byte[] bufferRes = new byte[Message.bufferSize];
            Console.WriteLine("Before");
            clientSocket.ReceiveFrom(bufferRes, bufferRes.Length, SocketFlags.None, ref serverEP);
            Console.WriteLine("After");
            Message msg = new Message(bufferRes);
            String[] dataArray = msg.data.Split('|');
            Card c = Game.Instance.CreatCardByGame(dataArray[1]);

            return c;
        }

        public void ClientPickCard(Card c)
        {
            Message m_PickCard = new Message(Commande.PICKCARD, CommandeType.REQUETE, c.GetName(), ClientPlayer.getName());
        
            byte[] buffer_room = m_PickCard.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);
        }




        public void ClientPlayCard(Player player, Card cardPlay)
        {
            String data = player.getName();

            Message m_cardPlay = new Message(Commande.PLAYCARD, CommandeType.REQUETE, cardPlay.GetName(), ClientPlayer.getName());
            byte[] buffer_room = m_cardPlay.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);


        }

        public String getName()
        {
            return ClientPlayer.getName();
        }




        public void ClientFreeThread()
        {

            Message freeThread = new Message(Commande.FREETHREAD, CommandeType.REQUETE, "", "");
            byte[] buffer_room = freeThread.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);

        }

  

        public void ClientFreeTrheadGameForm()
        {
            Message freeThread = new Message(Commande.FREETHREAD, CommandeType.REQUETE, "gameform", "");
            byte[] buffer_room = freeThread.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);
        }

        public void ClientChoosePlayer(Player p)
        {
            Message choosePlayer = new Message(Commande.FREETHREAD, CommandeType.REQUETE, "", "");
            byte[] buffer_room = choosePlayer.GetBytes();
            clientSocket.SendTo(buffer_room, 0, buffer_room.Length, SocketFlags.None, serverEP);
        }

        public Boolean ClientGuardEffect(Player source, Player target, Card c)
        {
            Message guardEffect = new Message(Commande.GUARDEFFECT, CommandeType.REQUETE,source.getName()+"|"+ target.getName() + "|" + c.getValue(), "");
            byte[] buffer_room = guardEffect.GetBytes();
            clientSocket.SendTo(buffer_room, serverEP);

            //Attente reponse
            byte[] bufferRes = new byte[Message.bufferSize];
            clientSocket.ReceiveFrom(bufferRes, bufferRes.Length, SocketFlags.None, ref serverEP);
            Message res = new Message(bufferRes);

            if (res.data.ToLower() == "yes")
                return true;
            else
                return false;
        }

        public String ClientPriestEffect(Player source,Player target)
        {
            List<Card> res = new List<Card>();
            Message priestEffect = new Message(Commande.PRIESTEFFECT, CommandeType.REQUETE, target.getName(), source.getName());
            byte[] buffer_room = priestEffect.GetBytes();
            clientSocket.SendTo(buffer_room, serverEP);

            //Attente reponse
            byte[] bufferRes = new byte[Message.bufferSize];
            clientSocket.ReceiveFrom(bufferRes, bufferRes.Length, SocketFlags.None, ref serverEP);
            Message resMessage = new Message(bufferRes);


            
            return resMessage.data;
        }

        public String ClientBaronEffect(Player source, Player target)
        {
            Message BaronEffect = new Message(Commande.BARONEFFECT, CommandeType.REQUETE, target.getName(), source.getName());
            byte[] buffer_room = BaronEffect.GetBytes();
            clientSocket.SendTo(buffer_room, serverEP);

            //Attente reponse
            byte[] bufferRes = new byte[Message.bufferSize];
            clientSocket.ReceiveFrom(bufferRes, bufferRes.Length, SocketFlags.None, ref serverEP);
            Message res = new Message(bufferRes);


            return res.data;
        }

        public Player ClientHandmainEffect(Player target)
        {
            Message HandMaidEffect = new Message(Commande.HANDMAIDEEFFECT, CommandeType.REQUETE, target.getName(), "");
            byte[] buffer_room = HandMaidEffect.GetBytes();
            clientSocket.SendTo(buffer_room, serverEP);

            //Attente reponse
            byte[] bufferRes = new byte[Message.bufferSize];
            clientSocket.ReceiveFrom(bufferRes, bufferRes.Length, SocketFlags.None, ref serverEP);
            Message res = new Message(bufferRes);

         
            return target;
        }

        public Player ClientPrinceEffect(Player source, Player target)
        {
            Message PrinceEffect = new Message(Commande.PRINCEEFFECT, CommandeType.REQUETE, target.getName(), source.getName());
            byte[] buffer_room = PrinceEffect.GetBytes();
            clientSocket.SendTo(buffer_room, serverEP);


            return null;
        }

        public void ClientPlayCardVisual(Player source,Card c)
        {
            Message PlayerVisualy = new Message(Commande.PLAYCARDVISUAL, CommandeType.REQUETE, c.getValue().ToString(), source.getName());
            byte[] buffer_room = PlayerVisualy.GetBytes();
            clientSocket.SendTo(buffer_room, serverEP);
        }

        public Player ClientKingEffect(Player source, Player target)
        {
       
            Message KingEffect = new Message(Commande.KINGEFFECT, CommandeType.REQUETE, target.getName(), source.getName());
            byte[] buffer_room = KingEffect.GetBytes();
            clientSocket.SendTo(buffer_room, serverEP);


            return null;
        }

        public void ClientCountessEffect(Player source)
        {

            Message KingEffect = new Message(Commande.COUNTESSEFFECT, CommandeType.REQUETE, source.getName(), source.getName());
            byte[] buffer_room = KingEffect.GetBytes();
            clientSocket.SendTo(buffer_room, serverEP);

        }

        public void ClientPrincessEffect(Player source)
        {
            Message PrincessEffect = new Message(Commande.PRINCESSEFECT, CommandeType.REQUETE, source.getName(), source.getName());
            byte[] buffer_room = PrincessEffect.GetBytes();
            clientSocket.SendTo(buffer_room, serverEP);
        }

        public void CountessEffect()
        {
            Message Countess = new Message(Commande.COUNTESSEFFECT, CommandeType.REQUETE, "","");
            byte[] buffer_room = Countess.GetBytes();
            clientSocket.SendTo(buffer_room, serverEP);
        }

        public void ClientNoTarget()
        {
            Message NoTarget = new Message(Commande.NOVALIDTARGET, CommandeType.REQUETE,"","");
            byte[] buffer_room = NoTarget.GetBytes();
            clientSocket.SendTo(buffer_room, serverEP);
        }

        public void ClientAskDeck()
        {
            Message AskDeck = new Message(Commande.ASKDECK, CommandeType.REQUETE, "", "");
            byte[] buffer_room = AskDeck.GetBytes();
            clientSocket.SendTo(buffer_room, serverEP);
        }

        public void ClientLeaveGame()
        {
            Message Leave = new Message(Commande.PLAYERLEAVE, CommandeType.REQUETE, "", "");
            byte[] buffer_room = Leave.GetBytes();
            clientSocket.SendTo(buffer_room, serverEP);
        }


        //Ferme socket Client
        public void CloseSocket()
        {

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            clientSocket.Dispose(); ;

        }

        public void ShutDown()
        {
            clientSocket.Dispose();
        }
   

    }


}