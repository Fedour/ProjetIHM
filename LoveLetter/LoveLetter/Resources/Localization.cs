using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveLetter
{
    enum Language
    {
        English,
        Français,
        Deutsch
    };

    static class Localization
    {
        public static String selectOnePlayer;

        public static String thinksThat;
        public static String has; 
        public static String[] cardNames = new String[9]; 

        public static void Change(Language language)
        {
            if (language == Language.English)
            {
                selectOnePlayer = "Select a player";

                cardNames[0] = "";
                cardNames[1] = "a guard";
                cardNames[2] = "a priest";
                cardNames[3] = "a baron";
                cardNames[4] = "a handmaiden";
                cardNames[5] = "a prince";
                cardNames[6] = "a king";
                cardNames[7] = "a countess";
                cardNames[8] = "a princess";

                thinksThat = "thinks that";
                has = "has"; 
            }
            else if (language == Language.Français)
            {
                selectOnePlayer = "Sélectionnez un joueur";

                cardNames[0] = "";
                cardNames[1] = "un garde";
                cardNames[2] = "un prêtre";
                cardNames[3] = "un baron";
                cardNames[4] = "une servante";
                cardNames[5] = "un prince";
                cardNames[6] = "un roi";
                cardNames[7] = "une comptesse";
                cardNames[8] = "une princesse";

                thinksThat = "pense que";
                has = "possède";
            }
            else if (language == Language.Deutsch)
            {

            }
        }
    }
}
