using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveLetter
{
    public class AI
    {
        enum Card { Unknown, Garde, Pretre, Baron, Servante, Prince, Roi, Comtesse, Princesse };

        public class Decision
        {
            public int chosenCard;
            public int guessPlayer;
            public int guessCard;
        }

        private Player player;
        int difficulty;
        Card left;
        Card right;
        private Card[] seen = new Card[4];

        public AI(Player player)
        {
            this.player = player;
        }

        //finds a random active player
        private int RandomValidPlayer()
        {
            Random random = new Random();

            int index;

            do
            {
                index = random.Next(0, ServerLoveLetter.Properties.Settings1.Default.nbrPlayers - 1); // a changer non ? c'est pas plutot rnd(0, nbrDeJoueursActif-1)  ?
            }
            while ((index == player.indexPlayer) || (!Game.Instance.players[index].isActive));

            return index;
        }

        //checks if the ai has a specific card
        private bool Have(Card card)
        {
            if (left == card) return true;
            if (right == card) return true;
            return true;
        }

        //tells which hand has the card
        private int Hand(Card card)
        {
            if (left == card) return 0;
            else return 1;
        }

        //finds an active player with a specific card
        private bool OtherPlayerHas(Card card)
        {
            for (int i = 0; i < 4; i++)
            {
                if ((i != player.indexPlayer) && (Game.Instance.players[i].isActive) && (seen[i] == card))
                {
                    return true;
                }
            }
            return false;
        }

        //tells which player has the card
        private int PlayerWith(Card card)
        {
            for (int i = 0; i < 4; i++)
            {
                if ((i != player.indexPlayer) && (Game.Instance.players[i].isActive) && (seen[i] == card))
                {
                    return i;
                }
            }
            return -1;
        }

        //IA simple
        public Decision Decide_Simple()
        {
            Decision decision = new Decision();

            left = (Card)player.getCards().ElementAt(0).value;
            right = (Card)player.getCards().ElementAt(1).value;
            int numJoueur = player.indexPlayer;
            int nbJoueurs = ServerLoveLetter.Properties.Settings1.Default.nbrPlayers;

            if ((left == Card.Comtesse || right == Card.Comtesse) && (left == Card.Roi || left == Card.Prince || right == Card.Roi || right == Card.Prince)) //si on a la comtesse et un prince ou un roi, on est obligé de jouer la comtesse
            {
                decision.chosenCard = (left == Card.Comtesse) ? 0 : 1;
            }
            else
            {
                Card card;
                if (left < right) //l'IA joue la carte de plus petite valeur
                {
                    decision.chosenCard = 0;
                    card = left;
                }
                else
                {
                    decision.chosenCard = 1;
                    card = right;
                }

                if (card == Card.Garde) //Si l'IA joue le garde
                {
                    int rdm1 = RandomValidPlayer();
                    int rdm2 = new Random().Next(2, 9);
                    //devine (rdm2) la carte du joueur rdm1 au hasard
                }
                else if (card == Card.Pretre) //Si l'IA joue le pretre 
                {
                    int rdm = RandomValidPlayer();
                    //ia simple donc oublie les cartes qu'elle a vue
                }
                else if (card == Card.Baron) //Si l'IA joue le baron 
                {
                    Random random = new Random();
                    int rdm = RandomValidPlayer();
                    //compare sa carte avec le joueur rdm, le moins elevé perd
                }
                else if (card == Card.Servante) //Si l'IA joue la servante 
                {
                    //l'ia est protégée au prochain tour
                }
                else if (card == Card.Prince) //Si l'IA joue le prince 
                {
                    Random random = new Random();
                    int rdm = random.Next(0, nbJoueurs);
                    //le joueur rdm jette sa carte et pioche
                }
                else if (card == Card.Roi) //Si l'IA joue le roi
                {
                    Random random = new Random();
                    int rdm = RandomValidPlayer();
                    //echange sa main avec le joueur rdm 
                }
                else if (card == Card.Comtesse) //Si l'IA joue la comtesse 
                {
                    //pas d'effet
                }
                else if (card == Card.Princesse) //Si l'IA joue la princesse 
                {
                    //l'ia perd
                }
            }

            return decision;
        }

        public Decision Decide_Normal()
        {
            Decision decision = new Decision();

            Card left = (Card)player.getCards().ElementAt(0).value;
            Card right = (Card)player.getCards().ElementAt(1).value;
            int numJoueur = player.indexPlayer;
            int nbJoueurs = ServerLoveLetter.Properties.Settings1.Default.nbrPlayers;

            if ((left == Card.Comtesse || right == Card.Comtesse) && (left == Card.Roi || left == Card.Prince || right == Card.Roi || right == Card.Prince))
            {
                decision.chosenCard = (left == Card.Comtesse) ? 0 : 1;
            }
            else
            {
                if (Have(Card.Prince) && OtherPlayerHas(Card.Princesse)) //Si l'IA a un prince et qu'un autre joueur a une princesse, on peut éliminer ce joueur
                {
                    decision.chosenCard = Hand(Card.Prince);
                    decision.guessPlayer = PlayerWith(Card.Princesse);
                    decision.guessCard = (int)Card.Princesse;
                }
                else if (Have(Card.Baron) && OtherPlayerHas(Card.Garde)) //Si l'IA a un baron et qu'un autre joueur a un garde
                {
                    decision.chosenCard = Hand(Card.Baron);
                    decision.guessPlayer = PlayerWith(Card.Garde);
                }
                else if (Have(Card.Baron) && OtherPlayerHas(Card.Pretre)) //Si l'IA a un baron et qu'un autre joueur a un pretre
                {
                    decision.chosenCard = Hand(Card.Baron);
                    decision.guessPlayer = PlayerWith(Card.Pretre);
                }
                else if (Have(Card.Pretre) && OtherPlayerHas(Card.Unknown)) //Si on ne sait rien on cherche a connaitre la main d'un adversaire
                {
                    decision.chosenCard = Hand(Card.Pretre);
                    decision.guessPlayer = PlayerWith(Card.Unknown);
                }
                else if (Have(Card.Roi) && !Have(Card.Garde)) //Une autre façon de connaitre la main de l'adversaire
                {
                    decision.chosenCard = Hand(Card.Roi);

                    if (OtherPlayerHas(Card.Unknown))
                    {
                        decision.guessPlayer = PlayerWith(Card.Unknown);
                    }
                    else
                    {
                        decision.guessPlayer = RandomValidPlayer();
                    }

                    seen[decision.guessPlayer] = (decision.chosenCard == 0) ? right : left;
                }
                else //si on ne peut rien faire on joue au hasard
                {
                    decision = Decide_Simple();
                }
            }

            return decision;
        }

        public Decision Decide_Hard()
        {
            Decision decision = new Decision();

            return decision;
        }

        public Decision Decide()
        {
            if (difficulty == 1)
            {
                return Decide_Normal();
            }
            else
            {
                return Decide_Simple();
            }
        }

        //Events

        //must be called when the round starts
        public void RoundStarts()
        {
            for (int i = 0; i < 4; i++)
                seen[i] = Card.Unknown;
        }

        //must be called when the AI sees the card of a player
        public void RevealCard(int playerIndex, int card)
        {
            seen[playerIndex] = (Card)card;
        }

        //must be called when any players plays a card
        public void CardPlayed(int playerIndex, int card)
        {
            if (seen[playerIndex] == (Card)card)
            {
                seen[playerIndex] = Card.Unknown;
            }
        }


    }
}
