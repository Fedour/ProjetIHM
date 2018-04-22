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
        public static String multiplayerLocal;
        public static String multiplayerNetwork;
        public static String settings;
        public static String singlePlayer; 

        public static void Change(Language language)
        {
            if (language == Language.English)
            {
                multiplayerLocal = "Multiplayer Local";
                multiplayerNetwork = "Multiplayer Network";
                settings = "Settings";
                singlePlayer = "Single Player"; 
            }
            else if (language == Language.Français)
            {
                multiplayerLocal = "Multijoueur local";
                multiplayerNetwork = "Multijoueur en réseau";
                settings = "Paramètres";
                singlePlayer = "Solo";
            }
            else if (language == Language.Deutsch)
            {
                
            }
        }
    }
}
