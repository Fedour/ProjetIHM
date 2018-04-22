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
    class CardEffectMullti
    {


        public static void GuardEffect(Player source, Player target, int cardValue, int port,Network.Client c)
        {
            
            //copy list of player and remove the current playing player AND protectd player
            List<Player> availablePlayer = Player.getCurrentNonProtectedPlayer();
            availablePlayer.Remove(GameControllerMulti.currentPlayer);

            //check that there is atleast one player that the source player can choose
            if (availablePlayer.Count > 0)
            {
                //Player can try to guess 1 opponent card, if it's correct, the opponent is eliminated
                int targetNum = GameControllerMulti.gameView.selectPlayer(Localization.selectOnePlayer, false, availablePlayer);
                target = Game.Instance.getPlayerByIndex(targetNum);

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
                cardValue = int.Parse(GameControllerMulti.gameView.selectedTypeOfCards(Properties._string.ChooseATypeOfCard, cardsWithoutGuard));
                
                Card choose = Card.getCardByValue(cardValue);
                
                Boolean res = c.ClientGuardEffect(source,target, choose);
               

               
            }
            else
            {
                MessageBox.Show(Properties._string.ThereAreNoAvalaibleTarget);
                c.ClientNoTarget();
            }


        }


        public static void CountessEffect(Player source,Network.Client c)
        {
            c.ClientCountessEffect(source);
        }

        public static void PriestEffect(Player source, Player target, Network.Client c)
        {

            //copy list of player and remove the current playing player AND protectd player
            List<Player> availablePlayer = Player.getCurrentNonProtectedPlayer();
            availablePlayer.Remove(GameControllerMulti.currentPlayer);
            
       

            //check that there is atleast one player that the source player can choose
            if (availablePlayer.Count > 0)
            {
             
                int targetNum = GameControllerMulti.gameView.selectPlayer(Properties._string.Select1Player, false, availablePlayer);
                target = Game.Instance.getPlayerByIndex(targetNum);

                for (int i = 0; i < availablePlayer.Count; i++)
                {
                    if (availablePlayer.ElementAt(i).indexPlayer == targetNum)
                    {
                        target = availablePlayer.ElementAt(i);
                    }
                }
                GameControllerMulti.gameView.showCardsHand(String.Format(Properties._string.HeresTheHandOfYourTarget, target.name), target);
                String res = c.ClientPriestEffect(source, target);
            }
            else
            {
                MessageBox.Show(Properties._string.ThereAreNoAvalaibleTarget);
                c.ClientNoTarget();
            }


        }

        public static void BaronEffect(Player source, Player target,Network.Client c)
        {

            //copy list of player and remove the current playing player
            List<Player> availablePlayer = Player.getCurrentNonProtectedPlayer();
            availablePlayer.Remove(GameControllerMulti.currentPlayer);

            if (availablePlayer.Count>0) //if there is atleast one target in the target list
            {
                int targetNum = 0;
                if (source.isHuman) //source player needs to interact
                {
                    targetNum = GameControllerMulti.gameView.selectPlayer(Properties._string.Select1Player, false, availablePlayer);

                    target = Game.Instance.getPlayerByIndex(targetNum);

                }
                target = Game.Instance.getPlayerByIndex(targetNum);
                String res = c.ClientBaronEffect(source, target);
              

         

            }
            else
            {
                MessageBox.Show(Properties._string.ThereAreNoAvalaibleTarget);
                c.ClientNoTarget();
            }
        }

        public static void HandmaidEffect(Player source, Network.Client client)
        {
            try
            {
                //Player is protected for 1 round
                
                Player p = client.ClientHandmainEffect(source);
                GameControllerMulti.gameView.showMessage(String.Format(Properties._string.IsProtectedForOneTurn, p.name), Properties.Settings.Default.showMessage);
           


                source.HMProtected = true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e+"");
            }
      
            

        }

        public static void PrinceEffect(Player source, Player target, Network.Client c)
        {
            if (source.isHuman) // user needs to choose a target
            {
            
                List<Player> availablePlayer = Player.getCurrentNonProtectedPlayer();

                if (availablePlayer.Count > 0) 
                {
                
                    int targetNum = GameControllerMulti.gameView.selectPlayer(Properties._string.Select1Player, true, availablePlayer);
                    target = Game.Instance.getPlayerByIndex(targetNum);

                      if (Game.Instance.play_Deck.Count > 0)
                      {
                        //Envoie message serveur
                        Player res = c.ClientPrinceEffect(source, target);
                      }                
                }
                else
                {
                    MessageBox.Show(Properties._string.TheGameDeckIsEmpty);
                }
            }

        }

        public static void KingEffect(Player source, Player target, Network.Client c)
        {
            //copy list of player and remove the current playing player
            List<Player> availablePlayer = Player.getCurrentNonProtectedPlayer();
            availablePlayer.Remove(GameControllerMulti.currentPlayer);

            if(availablePlayer.Count>0)
            {
               
                int targetNum = GameControllerMulti.gameView.selectPlayer(Properties._string.Select1Player, true, availablePlayer);
                target = Game.Instance.players.ElementAt(targetNum);

                
                Player res = c.ClientKingEffect(source, target);


                GameControllerMulti.gameView.showMessage(String.Format(Properties._string.SwitchHands, source.name, target.name), Properties.Settings.Default.showMessage);

                List<Card> tmp = new List<Card>(source.player_Deck);
                source.player_Deck = new List<Card>(target.player_Deck);
                target.player_Deck = new List<Card>(tmp);

                GameControllerMulti.gameView.updateHand(source);
                //GameControllerMulti.gameView.updateHand(target);

            }
            else
            {
                MessageBox.Show(Properties._string.ThereAreNoAvalaibleTarget);
                c.ClientNoTarget();
            }

        }


        //look if there is a countess in the hand and a princeOrking , if so return the countess if not return null
        public static Boolean MustPlayCountess(Player source,Network.Client cli)
        {
            Boolean res = false;

            foreach (Card c in source.player_Deck.ToList())
            {
                if (c.value == 7)
                {
                    foreach (Card card in source.player_Deck.ToList())
                    {
                        if ((card.value == 5 || card.value == 6) && res == false)
                        {
                            GameControllerMulti.CountessBlock = true;
                            MessageBox.Show(String.Format(Properties._string.MustPlayCountess));
                        }
                    }
                }
            }

            return res;
        }

        public static void PrincessEffect(Player source, int port,Network.Client c)
        {      
            
            c.ClientPrincessEffect(source);
            GameControllerMulti.killPlayer(source);
            
        
        }

    }
}