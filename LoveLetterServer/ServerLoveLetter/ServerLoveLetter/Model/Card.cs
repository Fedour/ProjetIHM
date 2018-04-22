using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveLetter
{
    public class Card
    {
        //Attribute
        public int value;
        public String name;
        public String texte;
        

        public static List<Card> cardsType = new List<Card> { new Card(1, "Guard"),
                                                              new Card(2, "Priest"),
                                                              new Card(3, "Baron"),
                                                              new Card(4, "Handmaiden"),
                                                              new Card(5, "Prince"),
                                                              new Card(6, "King"),
                                                              new Card(7, "Countess"),
                                                              new Card(8, "Princess")};


        //Methods
        //Constructor
        public Card(int value, String name, String texte)
        {
            this.value = value;
            this.name = name;
            this.texte = texte;
        
        }

        //constructor used to instantiate and store card in combobox ( we don't need card effect and card picture )
        public Card(int value, String name)
        {
            this.value = value;
            this.name = name;
            
        }

        public String GetName()
        {
            return this.name;
        }

        public int getValue()
        {
            return this.value;
        }

        /*
        public void Effect(Player source, Player target, int cardValue)//Find the right effect according to card type
        {
            switch (this.value)
            {
                case 1:
                    CardEffect.GuardEffect(source, target, cardValue);
                    break;
                case 2:
                    CardEffect.PriestEffect(source, target);
                    break;
                case 3:
                    CardEffect.BaronEffect(source, target);
                    break;
                case 4:
                    CardEffect.HandmaidEffect(source);
                    break;
                case 5:
                    CardEffect.PrinceEffect(source, target);
                    break;
                case 6:
                    CardEffect.KingEffect(source, target);
                    break;
                case 7:
                    CardEffect.CountessEffect(source);
                    break;
                case 8:
                    CardEffect.PrincessEffect(source);
                    break;
            }
            
        }
        */
    }
}
