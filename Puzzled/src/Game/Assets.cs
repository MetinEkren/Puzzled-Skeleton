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

        public static string IdleSheetPath = ResourcesDirectory + "Resources/Sprites/idle.png";
        public static string JumpSheetPath = ResourcesDirectory + "Resources/Sprites/jump.png";
        public static string PainSheetPath = ResourcesDirectory + "Resources/Sprites/pain.png";
        public static string PushSheetPath = ResourcesDirectory + "Resources/Sprites/push.png";
        public static string RunSheetPath = ResourcesDirectory + "Resources/Sprites/run.png";

        ////////////////////////////////////////////////////////////////////////////////////
        // MainMenu/Saves menu
        ////////////////////////////////////////////////////////////////////////////////////
        public static FireableAudio IntroMusic = new FireableAudio(IntroMusicPath, Settings.MasterVolume); // Note: This is so it plays softer on Jorben's PC
        public static LoopAudio MainMenuMusic = new LoopAudio(MainMenuMusicPath, Settings.MasterVolume); // Note: This is so it plays softer on Jorben's PC

        public static Texture MainMenuLogo = new Texture(MainMenuLogoPath);
        public static Texture WhiteTexture = new Texture();

        ////////////////////////////////////////////////////////////////////////////////////
        // Tilesheets
        ////////////////////////////////////////////////////////////////////////////////////
        public static Texture IdleSheet = new Texture(IdleSheetPath);
        public static Texture JumpSheet = new Texture(JumpSheetPath);
        public static Texture PainSheet = new Texture(PainSheetPath);
        public static Texture PushSheet = new Texture(PushSheetPath);
        public static Texture RunSheet = new Texture(RunSheetPath);

    }

}