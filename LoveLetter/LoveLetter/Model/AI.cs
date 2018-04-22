using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveLetter
{
    public class AI
    {
        //5 garde
        //1 roi
        //1 comtesse
        //1 princesse
        //2 pour les autres
        public enum Card { Unknown, Garde, Pretre, Baron, Servante, Prince, Roi, Comtesse, Princesse };

        public class Decision
        {
            public int chosenCard;
            public int guessPlayer;
            public int guessCard;
            public bool found;
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

        //checks if someone can be targeted
        private bool AtLeastOneValidTarget()
        {
            for (int i = 0; i < Properties.Settings.Default.nbrPlayers; i++)
            {
                if ((i != player.indexPlayer) && (Game.Instance.players[i].isActive) && (!Game.Instance.players[i].HMProtected))
                {
                    return true; 
                }
            }

            return false; 
        }

        //finds a random active player
        private int RandomValidPlayer()
        {
            Random random = new Random();

            int index = 0;

            if (AtLeastOneValidTarget())
            {
                do
                {
                    index = random.Next(0, Properties.Settings.Default.nbrPlayers);
                }
                while ((index == player.indexPlayer) || (!Game.Instance.players[index].isActive) || (Game.Instance.players[index].HMProtected));
            }

            return index;
        }

        //checks if the ai has a specific card
        private bool Have(Card card)
        {
            if (left == card) return true;
            if (right == card) return true;
            return false;
        }

        //checks the ai's other card 
        private Card Other(Card card)
        {
            if (left == right) return left;
            else
            {
                if (card == left)
                    return right;
                else
                    return left;
            }
        }


        //tells which hand has the card
        private int Hand(Card card)
        {
            if (left == card) return 0;
            else return 1;
        }

        //tells which player has the card
        private int PlayerWith(Card card)
        {
            for (int i = 0; i < Properties.Settings.Default.nbrPlayers; i++)
            {
                if ((i != player.indexPlayer) && (Game.Instance.players[i].isActive) && (!Game.Instance.players[i].HMProtected) && (seen[i] == card))
                {
                    return i;
                }
            }
            return -1;
        }

        //tells which player has the card
        private int PlayerWithBelow(Card card)
        {
            for (int i = 0; i < Properties.Settings.Default.nbrPlayers; i++)
            {
                if ((i != player.indexPlayer) && (Game.Instance.players[i].isActive) && (!Game.Instance.players[i].HMProtected) && (seen[i] < card) && (seen[i] != Card.Unknown))
                {
                    return i;
                }
            }
            return -1;
        }
        
        private Tuple<int, Card> PlayerWithCardSeenCardThatIsntGuard()
        {
            for (int i = 0; i < Properties.Settings.Default.nbrPlayers; i++)
            {
                if ((i != player.indexPlayer) && (Game.Instance.players[i].isActive) && (!Game.Instance.players[i].HMProtected) && (seen[i] != Card.Unknown) && (seen[i] != Card.Garde))
                {
                    return Tuple.Create(i, seen[i]); 
                }
            }
            return Tuple.Create(-1, Card.Unknown); 
        }

        //IA simple
        public Decision Decide_Simple()
        {
            Decision decision = new Decision();

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
                decision.guessPlayer = RandomValidPlayer();
                decision.guessCard = new Random().Next(2, 9);
                //devine (rdm2) la carte du joueur rdm1 au hasard
            }
            else if (card == Card.Pretre) //Si l'IA joue le pretre 
            {
                decision.guessPlayer = RandomValidPlayer();
                //ia simple donc oublie les cartes qu'elle a vue
            }
            else if (card == Card.Baron) //Si l'IA joue le baron 
            {
                Random random = new Random();
                decision.guessPlayer = RandomValidPlayer();
                //compare sa carte avec le joueur rdm, le moins elevé perd
            }
            else if (card == Card.Servante) //Si l'IA joue la servante 
            {
                //l'ia est protégée au prochain tour
            }
            else if (card == Card.Prince) //Si l'IA joue le prince 
            {
                Random random = new Random();
                decision.guessPlayer = random.Next(0, Properties.Settings.Default.nbrPlayers);
                //le joueur rdm jette sa carte et pioche
            }
            else if (card == Card.Roi) //Si l'IA joue le roi
            {
                Random random = new Random();
                decision.guessPlayer = RandomValidPlayer();
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

            return decision;
        }

        public Decision Decide_Normal()
        {
            Decision decision = new Decision();
            decision.found = false;

            if (!decision.found)
            {
                if (Have(Card.Garde))
                {
                    Tuple<int, Card> target = PlayerWithCardSeenCardThatIsntGuard();
                    if (target.Item1 != -1)
                    {
                        decision.chosenCard = Hand(Card.Garde);
                        decision.guessPlayer = target.Item1;
                        decision.guessCard = (int)target.Item2; 
                        decision.found = true;
                    }
                }
            }

            if (!decision.found)
            {
                if (Have(Card.Prince) && (PlayerWith(Card.Princesse) != -1)) //Si l'IA a un prince et qu'un autre joueur a une princesse, on peut éliminer ce joueur
                {
                    decision.chosenCard = Hand(Card.Prince);
                    decision.guessPlayer = PlayerWith(Card.Princesse);
                    decision.guessCard = (int)Card.Princesse;
                    decision.found = true;
                }
            }

            if (!decision.found)
            {
                if (Have(Card.Baron) && (PlayerWithBelow(Other(Card.Baron)) != -1)) //Si l'IA a un baron et qu'un joueur a une carte inférieure à notre autre carte
                {
                    decision.chosenCard = Hand(Card.Baron);
                    decision.guessPlayer = PlayerWithBelow(Other(Card.Baron));
                    decision.found = true;
                }
            }

            if (!decision.found)
            {
                if (Have(Card.Pretre) && (PlayerWith(Card.Unknown) != -1)) //Si on ne sait rien on cherche a connaitre la main d'un adversaire
                {
                    decision.chosenCard = Hand(Card.Pretre);
                    decision.guessPlayer = PlayerWith(Card.Unknown);
                    decision.found = true;
                }
            }

            if (!decision.found)
            {
                if (Have(Card.Roi) && !Have(Card.Garde)) //Une autre façon de connaitre la main de l'adversaire
                {
                    decision.chosenCard = Hand(Card.Roi);

                    if (PlayerWith(Card.Unknown) != -1)
                    {
                        decision.guessPlayer = PlayerWith(Card.Unknown);
                    }
                    else
                    {
                        decision.guessPlayer = RandomValidPlayer();
                    }

                    seen[decision.guessPlayer] = (decision.chosenCard == 0) ? right : left;
                    decision.found = true;
                }
            }

            return decision;
        }

        public class CardStatistics
        {
            public Card card;
            public bool canBe;
        }

        //1 slot per card: true if the player may possess it
        private CardStatistics[,] statistics0 = new CardStatistics[4, 16];

        //probability of each card for each player
        private int[,] statistics1 = new int[4, 9];

        void CanBe(int player, Card card)
        {
            for (int i = 0; i < 16; i++)
            {
                if ((statistics0[player, i].card == card) && (!statistics0[player, i].canBe))
                {
                    statistics0[player, i].canBe = true;
                    return;
                }
            }
        }

        void CannotBe(int player, Card card)
        {
            for (int i = 0; i < 16; i++)
            {
                if ((statistics0[player, i].card == card) && (statistics0[player, i].canBe))
                {
                    statistics0[player, i].canBe = false;
                    return;
                }
            }
        }

        int CountPossibilities(int player, Card card)
        {
            int count = 0;
            for (int i = 0; i < 16; i++)
            {
                if (statistics0[player, i].canBe && (statistics0[player, i].card == card))
                    count++;
            }
            return count;
        }


        int CountPossibilitiesBelow(int player, Card card)
        {
            int count = 0;
            for (int i = 0; i < 16; i++)
            {
                if (statistics0[player, i].canBe && (statistics0[player, i].card < card))
                    count++;
            }
            return count;
        }

        int CountPossibilities(int player)
        {
            int count = 0;
            for (int i = 0; i < 16; i++)
            {
                if (statistics0[player, i].canBe)
                    count++;
            }
            return count;
        }

        //tells which player has the card
        private int PlayerLikelyWith(Card card)
        {
            for (int i = 0; i < Properties.Settings.Default.nbrPlayers; i++)
            {
                if ((i != player.indexPlayer) && (Game.Instance.players[i].isActive) && (!Game.Instance.players[i].HMProtected))
                {
                    float count = (float)(CountPossibilities(i, card));
                    float total = (float)(CountPossibilities(i));

                    if (0.5 < (count / total))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        //tells which player has the card
        private int PlayerLikelyWithBelow(Card card)
        {
            for (int i = 0; i < Properties.Settings.Default.nbrPlayers; i++)
            {
                if ((i != player.indexPlayer) && (Game.Instance.players[i].isActive) && (!Game.Instance.players[i].HMProtected))
                {
                    float count = (float)(CountPossibilitiesBelow(i, card));
                    float total = (float)(CountPossibilities(i));

                    if (0.5 < (count / total))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private Tuple<int, Card> PlayerWithMostLikelyCardThatIsntGuard()
        {
            int target = -1;
            int probability = 0;
            Card card = Card.Unknown; 

            for (int i = 0; i < Properties.Settings.Default.nbrPlayers; i++)
            {
                if ((i != player.indexPlayer) && (Game.Instance.players[i].isActive) && (!Game.Instance.players[i].HMProtected))
                {
                    for (int j=2;j<9;j++)
                    {
                        if (probability < statistics1[i, j])
                        {
                            target = i;
                            probability = statistics1[i, j];
                            card = (Card)j; 
                        }
                    }
                } 
            }

            return Tuple.Create(target, card); 
        }

        public Decision Decide_Hard()
        {
            Decision decision = new Decision();
            decision.found = false;

            for (int i = 0; i < Properties.Settings.Default.nbrPlayers; i++)
            {
                if ((i != player.indexPlayer) && (Game.Instance.players[i].isActive))
                {
                    for (int c = 0; c < 16; c++)
                        statistics0[i, c] = new CardStatistics(); 

                    for (int c = 0; c < 5; c++)
                        statistics0[i, c].card = Card.Garde;
                    for (int c = 5; c < 7; c++)
                        statistics0[i, c].card = Card.Pretre;
                    for (int c = 7; c < 9; c++)
                        statistics0[i, c].card = Card.Baron;
                    for (int c = 9; c < 11; c++)
                        statistics0[i, c].card = Card.Servante;
                    for (int c = 11; c < 13; c++)
                        statistics0[i, c].card = Card.Prince;
                    statistics0[i, 13].card = Card.Roi;
                    statistics0[i, 14].card = Card.Comtesse;
                    statistics0[i, 15].card = Card.Princesse;

                    if (seen[i] != Card.Unknown)
                    {
                        //we know the card

                        for (int c = 0; c < 16; c++)
                            statistics0[i, c].canBe = false;

                        CanBe(i, seen[i]);
                    }
                    else
                    {
                        //exclude cards of other players & removed cards
                        for (int j = 0; j < Properties.Settings.Default.nbrPlayers; j++)
                        {
                            if ((i != j) && Game.Instance.players[j].isActive)
                            {
                                if (seen[j] != Card.Unknown)
                                {
                                    CannotBe(i, seen[j]); 
                                }
                            }

                            foreach (LoveLetter.Card card in Game.Instance.players[j].deadCard)
                            {
                                CannotBe(i, (Card)card.value);
                            }

                            foreach (LoveLetter.Card card in Game.Instance.removedCards)
                            {
                                CannotBe(i, (Card)card.value);
                            }
                        }
                    }

                    for (int j = 1; j < 9; j++)
                        statistics1[i, j] = 0; 

                    for (int j = 0; j < 16; j++)
                    {
                        if (statistics0[i, j].canBe)
                            statistics1[i, (int)statistics0[i, j].card]++;
                    }
                }
            }

            if (Have(Card.Prince) && (PlayerLikelyWith(Card.Princesse) != -1)) //Si l'IA a un prince et qu'un autre joueur a une princesse, on peut éliminer ce joueur
            {
                decision.chosenCard = Hand(Card.Prince);
                decision.guessPlayer = PlayerLikelyWith(Card.Princesse);
                decision.guessCard = (int)Card.Princesse;
                decision.found = true;
            }
            else if (Have(Card.Baron) && (PlayerLikelyWithBelow(Other(Card.Baron)) != -1)) //Si l'IA a un baron et qu'un joueur a une carte inférieure à notre autre carte
            {
                decision.chosenCard = Hand(Card.Baron);
                decision.guessPlayer = PlayerLikelyWithBelow(Other(Card.Baron));
                decision.found = true;
            }

            if (!decision.found)
            {
                if (Have(Card.Garde))
                {
                    Tuple<int, Card> target = PlayerWithMostLikelyCardThatIsntGuard();
                    if (target.Item1 != -1)
                    {
                        decision.chosenCard = Hand(Card.Garde); 
                        decision.guessPlayer = target.Item1;
                        decision.guessCard = (int)target.Item2;
                        decision.found = true;
                    }
                }
            } 

            return decision;
        }

        public Decision Decide()
        {
            left = (Card)player.getCards().ElementAt(0).value;
            right = (Card)player.getCards().ElementAt(1).value;

            if ((left == Card.Comtesse || right == Card.Comtesse) && (left == Card.Roi || left == Card.Prince || right == Card.Roi || right == Card.Prince))
            {
                Decision decision = new Decision(); 
                decision.chosenCard = (left == Card.Comtesse) ? 0 : 1;
                return decision; 
            }

            difficulty = Properties.Settings.Default.difficulty; 

            if (difficulty == 0)
            {
                return Decide_Simple(); 
            }
            else if (difficulty == 1)
            {
                Decision decision = Decide_Normal();

                if (!decision.found)
                    decision = Decide_Simple();

                return decision;
            }
            else
            {
                Decision decision = Decide_Normal();

                if (!decision.found)
                    decision = Decide_Hard();

                if (!decision.found)
                    decision = Decide_Simple();

                return decision; 
            }
        }

        //Events

        //must be called when the round starts
        public void RoundStarts()
        {
            for (int i=0;i<4;i++)
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
