using System;
using System.Collections.Generic;
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
        public static string ResourcesDirectory = "../../../Puzzled/"; // Debug & Release path
        //public static string ResourcesDirectory = ""; // Distribution path

        public static string IntroMusicPath = ResourcesDirectory + "Resources/Music/MainMenu_Intro.wav";
        public static string MainMenuMusicPath = ResourcesDirectory + "Resources/Music/MainMenu_Loop.wav";

        public static string MainMenuLogoPath = ResourcesDirectory + "Resources/Textures/Main-Logo.png";

        public static string IdleSheetPath = ResourcesDirectory + "Resources/Sprites/idle.png";
        public static string JumpSheetPath = ResourcesDirectory + "Resources/Sprites/jump.png";
        public static string PainSheetPath = ResourcesDirectory + "Resources/Sprites/pain.png";
        public static string PushSheetPath = ResourcesDirectory + "Resources/Sprites/push.png";
        public static string RunSheetPath = ResourcesDirectory + "Resources/Sprites/run.png";

        public static string TileSheetPath = ResourcesDirectory + "Resources/Sprites/tiles.png";

        public static string Level1Path = ResourcesDirectory + "Resources/Levels/level-1.json";
        public static string Level2Path = ResourcesDirectory + "Resources/Levels/level-2.json";
        public static string Level3Path = ResourcesDirectory + "Resources/Levels/level-3.json";
        public static string Level4Path = ResourcesDirectory + "Resources/Levels/level-4.json";
        public static string Level5Path = ResourcesDirectory + "Resources/Levels/level-5.json";
        public static string Level6Path = ResourcesDirectory + "Resources/Levels/level-6.json";
        public static string Level7Path = ResourcesDirectory + "Resources/Levels/level-7.json";
        public const uint LevelCount = 7;

        public static string LevelToPath(uint level)
        {
            switch (level)
            {
            case 1:     return Level1Path;
            case 2:     return Level2Path;
            case 3:     return Level3Path;
            case 4:     return Level4Path;
            case 5:     return Level5Path;
            case 6:     return Level6Path;
            case 7:     return Level7Path;

            default:
                break;
            }

            return "<INVALID>";
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // MainMenu/Saves menu
        ////////////////////////////////////////////////////////////////////////////////////
        public static FireableAudio IntroMusic = new FireableAudio(IntroMusicPath, Settings.MasterVolume); // Note: This is so it plays softer on Jorben's PC
        public static LoopAudio MainMenuMusic = new LoopAudio(MainMenuMusicPath, Settings.MasterVolume); // Note: This is so it plays softer on Jorben's PC

        public static Texture MainMenuLogo = new Texture(MainMenuLogoPath);
        public static Texture WhiteTexture = new Texture();
        public static Texture BlackTexture = new Texture(new byte[]{ 0, 0, 0, 255 });

        ////////////////////////////////////////////////////////////////////////////////////
        // Tilesheets
        ////////////////////////////////////////////////////////////////////////////////////
        public static Texture IdleSheet = new Texture(IdleSheetPath);
        public static Texture JumpSheet = new Texture(JumpSheetPath);
        public static Texture PainSheet = new Texture(PainSheetPath);
        public static Texture PushSheet = new Texture(PushSheetPath);
        public static Texture RunSheet = new Texture(RunSheetPath);

        public static Texture TileSheet = new Texture(TileSheetPath);

        ////////////////////////////////////////////////////////////////////////////////////
        // Tiles
        ////////////////////////////////////////////////////////////////////////////////////
        private static Dictionary<Texture, 
            Dictionary<(uint x, uint y), CroppedTexture>
        > s_TextureCache = new Dictionary<Texture, Dictionary<(uint x, uint y), CroppedTexture>>();

        public static CroppedTexture GetTexture(Texture texture, uint x, uint y)
        {
            if (!s_TextureCache.ContainsKey(texture))
                s_TextureCache.Add(texture, new Dictionary<(uint x, uint y), CroppedTexture>());

            Dictionary<(uint x, uint y), CroppedTexture> textureCache = s_TextureCache[texture];

            if (textureCache.ContainsKey((x, y)))
                return textureCache[(x, y)];

            textureCache.Add((x, y), new CroppedTexture(texture, new UV(x, y, (Settings.SpriteSize / Settings.Scale), (Settings.SpriteSize / Settings.Scale))));
            return textureCache[(x, y)];
        }

    }

}