using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoveLetter
{
   public class Card
    {
        //Attribute
        public int value;
        public String name;
        public String texte;
        public System.Drawing.Bitmap picture;

        public static List<Card> cardsType = new List<Card> { new Card(1, Properties._string.Guard,Properties.Resources.guard),
                                                              new Card(2, Properties._string.Priest,Properties.Resources.priest),
                                                              new Card(3, Properties._string.Baron,Properties.Resources.baron),
                                                              new Card(4, Properties._string.Handmaid,Properties.Resources.handmaid),
                                                              new Card(5, Properties._string.Prince,Properties.Resources.prince),
                                                              new Card(6, Properties._string.King,Properties.Resources.king),
                                                              new Card(7, Properties._string.Countess,Properties.Resources.countess),
                                                              new Card(8, Properties._string.Princess,Properties.Resources.princess)};


        //Methods
        //Constructor
        public Card(int value, String name, String texte, System.Drawing.Bitmap p)
        {
            this.value = value;
            this.name = name;
            this.texte = texte;
            this.picture = p;
        }

        //constructor used to instantiate and store card in combobox ( we don't need card effect and card picture )
        public Card(int value, String name, System.Drawing.Bitmap p)
        {
            this.value = value;
            this.name = name;
            this.picture = p;
        }

        public String GetName()
        {
            return this.name;
        }
        
        public int getValue()
        {
            return this.value;
        }

        public void Effect(Player source,Player target, int cardValue)//Find the right effect according to card type
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Properties.Settings.Default.currentCultureName);
            switch (this.value){
                case 1:
                    CardEffect.GuardEffect(source, target, cardValue);
                    break;
                case 2:
                    CardEffect.PriestEffect(source,target);
                    break;
                case 3:
                    CardEffect.BaronEffect(source,target);
                    break;
                case 4:
                    CardEffect.HandmaidEffect(source);
                    break;
                case 5:
                    CardEffect.PrinceEffect(source,target);
                    break;
                case 6:
                    CardEffect.KingEffect(source,target);
                    break;
                case 7:
                    CardEffect.CountessEffect(source);
                    break;
                case 8:
                    CardEffect.PrincessEffect(source);
                    break;
            }
        }

        public void MultiPlayerEffect(Player source, Player target, int cardValue,int port,Network.Client c)
        {
            switch (this.value)
            {
                case 1:
                    CardEffectMullti.GuardEffect(source, target, cardValue,port,c);
                    break;
                case 2:
                    CardEffectMullti.PriestEffect(source, target,c);
                    break;
                case 3:
                    CardEffectMullti.BaronEffect(source, target,c);
                    break;
                case 4:
                    CardEffectMullti.HandmaidEffect(source, c);
                    break;
                case 5:
                    CardEffectMullti.PrinceEffect(source, target,c);
                    break;
                case 6:
                    CardEffectMullti.KingEffect(source, target,c);
                    break;
                case 7:
                    CardEffectMullti.CountessEffect(source,c);
                    break;
                case 8:
                    CardEffectMullti.PrincessEffect(source,port,c);
                    break;
            }
        }

        public static String getCardNameByValue(int value)
        {
            String s = "";
            foreach (Card c in Card.cardsType)
            {
                if(c.value == value)
                {
                    s = c.name;
                }
            }

            return s;
        }

        public static Card getCardByValue(int value)
        {
            foreach (Card c in Card.cardsType)
            {
                if (c.value == value)
                {
                    return c;
                }
            }
            return null;
        }
    }
}
