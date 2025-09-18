using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Puzzled
{

    //////////////////////////////////////////////////////////////////////////////////
    // Assets
    //////////////////////////////////////////////////////////////////////////////////
    public class Assets
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Paths
        ////////////////////////////////////////////////////////////////////////////////////
        public static string ResourcesDirectory = "../../../Puzzled/";

        public static string IntroMusicPath = ResourcesDirectory + "Resources/Music/MainMenu_Intro.wav";
        public static string MainMenuMusicPath = ResourcesDirectory + "Resources/Music/MainMenu_Loop.wav";

        public static string MainMenuLogoPath = ResourcesDirectory + "Resources/Textures/Main-Logo.png";

        ////////////////////////////////////////////////////////////////////////////////////
        // MainMenu/Saves menu
        ////////////////////////////////////////////////////////////////////////////////////
        public static FireableAudio IntroMusic = new FireableAudio(IntroMusicPath);
        public static LoopAudio MainMenuMusic = new LoopAudio(MainMenuMusicPath);

        public static Texture MainMenuLogo = new Texture(MainMenuLogoPath);

    }

}