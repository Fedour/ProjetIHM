using LoveLetter.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoveLetter
{
    class CardEffect
    {
        public static void GuardEffect(Player source, Player target, int cardValue)
        {
            //copy list of player and remove the current playing player AND protectd player
            List<Player> availablePlayer = Player.getCurrentNonProtectedPlayer();
            availablePlayer.Remove(GameController.currentPlayer);

            //check that there is atleast one player that the source player can choose
            if (availablePlayer.Count > 0)
            {
                //Player can try to guess 1 opponent card, if it's correct, the opponent is eliminated
                if (source.isHuman) // source is human , so he needs to interact with interface
                {
                    int targetNum = GameController.gameView.selectPlayer(Properties._string.Select1Player, false, availablePlayer);

                    cardValue = 0; //default value in case user would left 

                    for (int i = 0; i < availablePlayer.Count; i++)
                    {
                        if (availablePlayer.ElementAt(i).indexPlayer == targetNum)
                        {
                            target = availablePlayer.ElementAt(i);
                        }
                    }

                    //get all the type of cards excepting the guard
                    List<Card> cardsWithoutGuard = new List<Card>(Card.cardsType);
                    cardsWithoutGuard.RemoveAt(0);
                    cardValue = int.Parse(GameController.gameView.selectedTypeOfCards(Properties._string.ChooseATypeOfCard, cardsWithoutGuard));
                }
                else
                {
                    //GameController.gameView.showMessage(String.Format(Properties._string.ThinksThatHas, source.name , target.name , Properties._string.ResourceManager.GetString(Card.getCardNameByValue(cardValue)),Properties.Settings.Default.showMessage),Properties.Settings.Default.showMessage);
                    GameController.gameView.showMessage(String.Format(Properties._string.ThinksThatHas, source.name, target.name, Card.getCardNameByValue(cardValue), Properties.Settings.Default.showMessage), Properties.Settings.Default.showMessage);
                }

                //check if the target has the choosed card type
                Boolean isRight = false;
                foreach (Card c in target.player_Deck)
                {
                    if (c.value == cardValue)
                    {
                        isRight = true;
                    }
                }

                if (isRight)
                {
                    GameController.gameView.showMessage(Properties._string.WellGuessed, Properties.Settings.Default.showMessage);
                    //remove the targeted player from the round
                    GameController.killPlayer(target);
                }
                else
                {
                    GameController.gameView.showMessage(string.Format(Properties._string.DoesNotHaveTheCard, target.name, Card.getCardNameByValue(cardValue)), Properties.Settings.Default.showMessage);
                }
            }
            else
            {
                GameController.gameView.showMessage(Properties._string.ThereAreNoAvalaibleTarget, Properties.Settings.Default.showMessage);
            }
        }

        internal static void CountessEffect(Player source)
        {
        }

        public static void PriestEffect(Player source, Player target)
        {
            //copy list of player and remove the current playing player AND protectd player
            List<Player> availablePlayer = Player.getCurrentNonProtectedPlayer();
            availablePlayer.Remove(GameController.currentPlayer);

            //check that there is atleast one player that the source player can choose
            if (availablePlayer.Count > 0)
            {
                if (source.isHuman) // source player needs to interact with interface
                {
                    int targetNum = GameController.gameView.selectPlayer(Properties._string.Select1Player, false, availablePlayer);

                    for (int i = 0; i < availablePlayer.Count; i++)
                    {
                        if (availablePlayer.ElementAt(i).indexPlayer == targetNum)
                        {
                            target = availablePlayer.ElementAt(i);
                        }
                    }
                    //get all the type of cards excepting the guard
                    GameController.gameView.showCardsHand(String.Format(Properties._string.HeresTheHandOfYourTarget,target.name), target);
                }
                else
                {
                    source.ai.RevealCard(target.indexPlayer, target.player_Deck[0].value);

                    GameController.gameView.showMessage(String.Format(Properties._string.UseAPriestToSee, source.name, target.name), Properties.Settings.Default.showMessage); 
                }
            }
            else
            {
                GameController.gameView.showMessage(Properties._string.ThereAreNoAvalaibleTarget, Properties.Settings.Default.showMessage);
            }

            
        }

        public static void BaronEffect(Player source, Player target)
        {

            //copy list of player and remove the current playing player
            List<Player> availablePlayer = Player.getCurrentNonProtectedPlayer();
            availablePlayer.Remove(GameController.currentPlayer);

            if (availablePlayer.Count>0) //if there is atleast one target in the target list
            {
                if (source.isHuman) //source player needs to interact
                {
                    int targetNum = GameController.gameView.selectPlayer(Properties._string.Select1Player, false, availablePlayer);

                    target = Game.Instance.players.ElementAt(targetNum);
                   
                }

                //show the comparison of hands if source or target is human
                if(source.isHuman || target.isHuman)
                {
                    GameController.gameView.compareCardsHand(String.Format(Properties._string.ComparingHandsOfAnd,source.name,target.name), source, target);
                }

                if (source.player_Deck.ElementAt(0).value > target.player_Deck.ElementAt(0).value)
                {
                    GameController.gameView.showMessage(String.Format(Properties._string.WonTheBattle, source.name, target.name), Properties.Settings.Default.showMessage);
                    GameController.killPlayer(target);
                }
                else if (source.player_Deck.ElementAt(0).value < target.player_Deck.ElementAt(0).value)
                {
                    GameController.gameView.showMessage(String.Format(Properties._string.WonTheBattle, target.name, source.name), Properties.Settings.Default.showMessage);
                    GameController.killPlayer(source);
                }
                else
                {
                    GameController.gameView.showMessage(Properties._string.Draw, Properties.Settings.Default.showMessage);
                }
            }
            else
            {
                GameController.gameView.showMessage(Properties._string.ThereAreNoAvalaibleTarget, Properties.Settings.Default.showMessage);
            }
        }

        public static void HandmaidEffect(Player source)
        {
            //Player is protected for 1 round
            source.HMProtected = true;
            GameController.gameView.showMessage(String.Format(Properties._string.IsProtectedForOneTurn,source.name ), Properties.Settings.Default.showMessage);
        }

        public static void PrinceEffect(Player source, Player target)
        {
            //copy list of player and remove the protected player
            List<Player> availablePlayer = Player.getCurrentNonProtectedPlayer();
            if (availablePlayer.Count > 0) //if there is atleast one target in the target list
            {
                if (source.isHuman) //source player needs to interact
                {
                    int targetNum = GameController.gameView.selectPlayer(Properties._string.Select1Player, true, availablePlayer);
                    for (int i = 0; i < availablePlayer.Count; i++)
                    {
                        if (availablePlayer.ElementAt(i).indexPlayer == targetNum)
                        {
                            target = availablePlayer.ElementAt(i);
                        }
                    }
                }
                //pick a new card and then call the visual effect as the target wanted to play a card (only visual effect so the card is removed from the hand)
                if (Game.Instance.play_Deck.Count > 0)
                {
                    Card deletedCard = target.player_Deck.ElementAt(0);
                    GameController.PickCard(target);
                    GameController.gameView.PlayCard(deletedCard, target);
                    target.deadCard.Add(deletedCard);
                    GameController.lastDiscardedCard = deletedCard;
                    target.player_Deck.Remove(deletedCard);
                    GameController.gameView.showMessage(String.Format(Properties._string.HadToDiscard, target.name), Properties.Settings.Default.showMessage);

                    //princess is discarded 
                    if (deletedCard.value == 8)
                    {                       
                        GameController.killPlayer(target);
                    }
                }
                else
                {
                    MessageBox.Show(Properties._string.TheGameDeckIsEmpty);
                }
            }
            else
            {
                GameController.gameView.showMessage(Properties._string.ThereAreNoAvalaibleTarget,Properties.Settings.Default.showMessage);
            }
        }

        public static void KingEffect(Player source, Player target)
        {
            //copy list of player and remove the current playing player
            List<Player> availablePlayer = Player.getCurrentNonProtectedPlayer();
            availablePlayer.Remove(GameController.currentPlayer);

            if(availablePlayer.Count>0)
            {
                if (source.isHuman) // user needs to choose a target
                {
                    int targetNum = GameController.gameView.selectPlayer(Properties._string.Select1Player, true, availablePlayer);
                    target = Game.Instance.players.ElementAt(targetNum);
                }

                if (!source.isHuman)
                    source.ai.RevealCard(target.indexPlayer, target.player_Deck[0].value);

                if (!target.isHuman)
                    target.ai.RevealCard(source.indexPlayer, source.player_Deck[0].value);

                //switch decks
                List<Card> tmp = new List<Card>(source.player_Deck);
                source.player_Deck = new List<Card>(target.player_Deck);
                target.player_Deck = new List<Card>(tmp);

                if (source.isHuman) // if the source player is human he must see the changes
                {
                    GameController.gameView.updateHand(source);
                }
                else if(target.isHuman)
                {
                    GameController.gameView.updateHand(target);
                }
                else
                {
                    GameController.gameView.showMessage(String.Format(Properties._string.SwitchHands, source.name, target.name), Properties.Settings.Default.showMessage);
                }
            }
            else
            {
                GameController.gameView.showMessage(Properties._string.ThereAreNoAvalaibleTarget, Properties.Settings.Default.showMessage);
            }
        }



        public static Boolean MustPlayCountess(Player source)
        {
            Boolean res = false;

            foreach (Card c in source.player_Deck.ToList())
            {
                if (c.value == 7)
                {
                    foreach (Card card in source.player_Deck.ToList())
                    {
                        if ((card.value == 5 || card.value == 6) && res==false)
                        {
                            res = true;
                            if(!source.isHuman)
                            {
                                 GameController.gameView.PlayCard(c, source);
                                 source.playCard(card);
                                //GameController.PlayCard(c, source);
                            }
                            else
                            {
                                GameController.PlayCard(c, source);
                            }

                        }
                    }
                }
            }

            return res;
        }
        

        public static void PrincessEffect(Player source)
        {
            //Player gets eliminated if this effect is activated
            GameController.killPlayer(source);
        }

    }
}