using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveLetter.Network
{

    public enum Commande
    {
        CREATELOBBY, JOINLOBBY, LISTLOBBY, PLAYCARD,
        MSG, SUBSCRIBE, DESUBSCRIBE, LEAVELOBBY,
        ASKLOBBY, ASKLOBBYPORT, FREETHREAD,
        REFRESH, BEGIN, GIVECARD, GO, REMOVEDECKCARD, GIVESTACK,
        PLAYERTURN, PICKCARD, ASKCARD, GUARDEFFECT, PRIESTEFFECT,
        BARONEFFECT, HANDMAIDEEFFECT, PRINCEEFFECT, KINGEFFECT,
        COUNTESSEFFECT, PRINCESSEFECT, GUARDEFFECTRES, PRIESTEFFECTRES,
        BARONEFFECTRES, HANDMAIDEEFFECTRES, PRINCEEFFECTRES, KINGEFFECTRES, COUNTESSEFFECTRES,
        PLAYERDEAD, NEXTTURN, ROUNDOVER, WINNEROFGAME, PLAYERLEAVE, CHECKNAME, DELETENAME,
        SOMEONELEAVE, PLAYCARDVISUAL, PLAYCARDVISUALRES,NOVALIDTARGET,ASKDECK,GAMEISOVER,
        READYTOHAVECARD,GIVEFIRSTCARD, READYTOREMOVECARD
    };


    public enum CommandeType
    { 
        REQUETE, REPONSE
    };

    public enum CardPlay
    {
        GUARD,PRIEST,BARON,HANDMAID,PRINCE,KING,COUNTESS,PRINCESS
    };


    public class Message
    {
        public const int bufferSize = 1500;
        public const int headerSize = 32;

        public Commande commande;               // commande
        public CommandeType commandeType;       // type (Requête/Réponse)
        public int dataSize;                    // taille de la donnée
        public String data;                     // données de la commande
        public String pseudo;                   // pseudo de l'envoyeur 

        public Message  (Commande commande, CommandeType type, String data, String pseudo)
        {
            this.commande = commande;
            this.commandeType = type;
            this.dataSize = data.Length;
            this.data = data;
            this.pseudo = pseudo;
        }


        public Message(byte[] buffer)
        {
            commande = (Commande)buffer[0];
            commandeType = (CommandeType)buffer[1];
            pseudo = Encoding.ASCII.GetString(buffer, 2, 30).TrimEnd(new char[] { '\0' });
            dataSize = BitConverter.ToInt32(buffer, 32);
            data = Encoding.ASCII.GetString(buffer, 36, dataSize);
        }


        public byte[] GetBytes()
        {
            byte[] buffer = new byte[bufferSize];                           // Déclaration du buffer

            buffer[0] = (byte)commande;                                     // Commande
            buffer[1] = (byte)commandeType;                                 // Type de la commande
            Encoding.ASCII.GetBytes(pseudo, 0, pseudo.Length, buffer, 2);   // Pseudo à 30 bits
            byte[] intBuf = BitConverter.GetBytes(dataSize);                // Taille de la data
            buffer[32] = intBuf[0];                                         // Int stocké sur 4bits
            buffer[33] = intBuf[1];
            buffer[34] = intBuf[2];
            buffer[35] = intBuf[3];
            Encoding.ASCII.GetBytes(data, 0, data.Length, buffer, 36);      // Data

            return buffer;
        }


        public static byte[] GetBytes(Commande commande, CommandeType type, String data, String pseudo)
        {
            Message chatCommande = new Message(commande, type, data, pseudo);
            return chatCommande.GetBytes();
        }


    }


}
