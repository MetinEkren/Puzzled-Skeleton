using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Puzzled
{

    //////////////////////////////////////////////////////////////////////////////////
    // JSONUserSettings
    //////////////////////////////////////////////////////////////////////////////////
    public struct JSONUserSettings
    {
        [JsonInclude]
        public uint SFXVolume;

        [JsonInclude]
        public uint MusicVolume;
    }

    //////////////////////////////////////////////////////////////////////////////////
    // UserSettings
    //////////////////////////////////////////////////////////////////////////////////
    public class UserSettings
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Static methods
        ////////////////////////////////////////////////////////////////////////////////////
        public static readonly string s_SettingsPath = Assets.ResourcesDirectory + "Resources/Settings/settings.json";

        ////////////////////////////////////////////////////////////////////////////////////
        // Audio
        ////////////////////////////////////////////////////////////////////////////////////
        private static uint s_SFXVolume;
        private static uint s_MusicVolume;
        public static uint SFXVolume
        {
            get { return s_SFXVolume; }
            set
            {
                // Note: Currently all audio files must manually be added... // FUTURE TODO: ...
                s_SFXVolume = value;
                Assets.JumpSound.Volume = value;
                Assets.KeyPickupSound.Volume = value;
            }
        }
        public static uint MusicVolume
        {
            get { return s_MusicVolume; }
            set
            {
                // Note: Currently all audio files must manually be added... // FUTURE TODO: ...
                s_MusicVolume = value;
                Assets.IntroMusic.Volume = value;
                Assets.MainMenuMusic.Volume = value;
                Assets.LevelMusic.Volume = value;
                Assets.WinMenuMusic.Volume = value;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Static methods
        ////////////////////////////////////////////////////////////////////////////////////
        public static void Load()
        {
            string json = File.ReadAllText(s_SettingsPath);
            JSONUserSettings settings =  JsonSerializer.Deserialize<JSONUserSettings>(json);

            Logger.Info($"Loaded UserSettings from \"{s_SettingsPath}\". SFXVolume = {settings.SFXVolume}, MusicVolume = {settings.MusicVolume}.");

            SFXVolume = settings.SFXVolume;
            MusicVolume = settings.MusicVolume;
        }

        public static void Save()
        {
            Logger.Info($"Saving UserSettings to \"{s_SettingsPath}\".");
            string text = JsonSerializer.Serialize<JSONUserSettings>(new JSONUserSettings
            { 
                SFXVolume = s_SFXVolume,
                MusicVolume = s_MusicVolume
            });

            File.WriteAllText(s_SettingsPath, text);
        }
    }

}