using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveLetter
{
    class CardEffect
    {
        public static void GuardEffect(Player source)
        {
            //Player can try to guess 1 opponent card, if it's correct, the opponent is eliminated
            Console.WriteLine("Which player you want to guess the card ?");
            String playerName = Console.ReadLine();
            Player target = Game.Instance.getPlayerInRoundByName(playerName);
            Console.WriteLine("Which card do you think it has ?");
            String cardName = Console.ReadLine();
            if(cardName==target.getCard().name && cardName!="Guard")
            {
                Game.Instance.removePlayerOfRound(target);
            }
        }

        public static void PriestEffect(Player source)
        {
            //Player can see 1 opponent deck of its choice
            Console.WriteLine("Which player you want to see the deck ?");
            String playerName = Console.ReadLine();
            Player target = Game.Instance.getPlayerInRoundByName(playerName);
            Console.WriteLine(target.name + " have " + target.getCard().name + " in its hands");
        }

        public static void BaronEffect(Player source)
        {
            //Compare your card with an opponent card, the player with the lowest value is eleminated
            Console.WriteLine("Which player you want to challenge ?");
            String playerName = Console.ReadLine();
            Player target = Game.Instance.getPlayerInRoundByName(playerName);
            if(target.getCard().value > source.getCard().value)
            {
                Game.Instance.removePlayerOfRound(source);
            }
            else if (target.getCard().value < source.getCard().value)
            {
                Game.Instance.removePlayerOfRound(target);
            }
        }

        public static void HandmaidEffect(Player source)
        {
            //Player is protected for 1 round
            source.HMProtected = true;
        }

        public static void PrinceEffect(Player source)
        {
            //Player can discard 1 card from its opponents or from its own deck
            Console.WriteLine("Which player you want to discard 1 card of its deck ?");
            String playerName = Console.ReadLine();
            Player target = Game.Instance.getPlayerInRoundByName(playerName);
            target.discardCard();
            Card card = Game.Instance.play_Deck.Pop();
            target.PickCard(card);
        }

        public static void KingEffect(Player source)
        {
            //Player can switch its deck with another player's deck
            Console.WriteLine("Which player you want to switch the deck with ?");
            String playerName = Console.ReadLine();
            Player target = Game.Instance.getPlayerInRoundByName(playerName);
            Game.Instance.switchPlayersDeck(source, target);
        }

        public static void CountessEffect(Player source)
        {
            //If player got Countess AND (King OR Prince), it gets eliminated
            //This effect is called only when the case is spotted
            Game.Instance.removePlayerOfRound(source);
        }

        public static void PrincessEffect(Player source)
        {
            //Player gets eliminated if this effect is activated
            Game.Instance.removePlayerOfRound(source);
        }
  
    }
}
