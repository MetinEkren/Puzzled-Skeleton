using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Puzzled
{

    ////////////////////////////////////////////////////////////////////////////////////
    // Save
    ////////////////////////////////////////////////////////////////////////////////////
    public struct Save
    {
        [JsonInclude]
        public string Name;
        [JsonInclude]
        public uint Level;

        [JsonInclude]
        public List<uint> Scores;
    }

    ////////////////////////////////////////////////////////////////////////////////////
    // SavesMenu
    ////////////////////////////////////////////////////////////////////////////////////
    public partial class SavesMenu : UserControl, Scene
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public SavesMenu()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }
        ~SavesMenu()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void OnLoad(object sender, RoutedEventArgs args) // Note: We need to do this after layout pass to make sure sizes are calculated
        {
            m_Renderer = new Renderer(GameCanvas);

            m_Saves[0] = Load(1);
            Logger.Trace($"Save 1 {{ Name = {m_Saves[0].Name}, Level = {m_Saves[0].Level}, Scores = <NOT IMPLEMENTED> }}");

            m_Saves[1] = Load(2);
            Logger.Trace($"Save 2 {{ Name = {m_Saves[1].Name}, Level = {m_Saves[1].Level}, Scores = <NOT IMPLEMENTED> }}");

            m_Saves[2] = Load(3);
            Logger.Trace($"Save 3 {{ Name = {m_Saves[2].Name}, Level = {m_Saves[2].Level}, Scores = <NOT IMPLEMENTED> }}");
        }

        public void OnUpdate(float deltaTime)
        {
            if (!IsLoaded) return;
        }

        public void OnRender()
        {
            if (!IsLoaded) return;
        }

        public void OnUIRender()
        {
            if (!IsLoaded) return;
        }

        public void OnEvent(Event e)
        {
            if (!IsLoaded) return;

            if (e is KeyPressedEvent kpe)
            {
                if (kpe.KeyCode == System.Windows.Input.Key.Escape)
                {
                    Logger.Info("Pressed Esc, going back to main menu.");
                    Game.Instance.ActiveScene = new MainMenu();
                }
            }

            // Fix Audio lagging when closing
            if (e is WindowCloseEvent)
            {
                Assets.IntroMusic.CloseAll(); // Note: Just in case the intro music was still playing from main menu
                Assets.MainMenuMusic.Stop();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Callbacks
        ////////////////////////////////////////////////////////////////////////////////////
        void SaveSlot1Pressed(object sender, RoutedEventArgs args) { SaveSlotPressed(1); }
        void SaveSlot2Pressed(object sender, RoutedEventArgs args) { SaveSlotPressed(2); }
        void SaveSlot3Pressed(object sender, RoutedEventArgs args) { SaveSlotPressed(3); }

        void SaveSlotPressed(uint slot)
        {
            Logger.Info($"Save slot {slot} being loaded.");

            Load(slot);

            Game.Instance.ActiveScene = new LevelOverlay(m_Saves[slot - 1], slot);
            Assets.MainMenuMusic.Stop();
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Private methods
        ////////////////////////////////////////////////////////////////////////////////////
        private Save Load(uint slot)
        {
            //string directory = Directory.GetCurrentDirectory(); // TODO: Set it to a set directory
            string directory = Assets.ResourcesDirectory + "Resources/Saves/";
            string saveSlotFilename = "save-" + slot + ".json";
            string saveSlotPath = System.IO.Path.Combine(directory, saveSlotFilename);

            string json = File.ReadAllText(saveSlotPath);
            return JsonSerializer.Deserialize<Save>(json);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Save[] m_Saves = new Save[3];
        
        private Renderer m_Renderer;

    }

}