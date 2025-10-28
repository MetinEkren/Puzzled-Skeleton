using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Puzzled
{

    ////////////////////////////////////////////////////////////////////////////////////
    // SavesMenu
    ////////////////////////////////////////////////////////////////////////////////////
    public partial class SavesMenu : UserControl, IScene
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public SavesMenu()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }
        public SavesMenu(MainMenu mainMenu)
        {
            m_MainMenu = mainMenu;

            InitializeComponent();
            Loaded += OnLoad;
        }
        ~SavesMenu()
        {
            // Note: For future, don't put anything in destructor, since objects are not destroyed at set moment. (GC moment)
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void OnLoad(object sender, RoutedEventArgs args) // Note: We need to do this after layout pass to make sure sizes are calculated
        {
            m_Renderer = new Renderer(GameCanvas);

            // Set quote
            {
                List<string> quotes = new List<string>(File.ReadAllLines(Assets.SaveQuotesPath));

                Random rnd = new Random();
                int i = rnd.Next(quotes.Count);
                QuoteText.Text = quotes[i];
            }

            // Load saves
            {
                Save save1 = Assets.LoadSave(1);
                Logger.Trace($"Save 1 {{ Name = {save1.Name}, Level = {save1.Level}, Scores = {((save1.Scores.Count != 0) ? string.Join(", ", save1.Scores.ToArray()) : "<NO SCORES>")} }}");
                
                Save save2 = Assets.LoadSave(2);
                Logger.Trace($"Save 2 {{ Name = {save2.Name}, Level = {save2.Level}, Scores = {((save2.Scores.Count != 0) ? string.Join(", ", save2.Scores.ToArray()) : "<NO SCORES>")} }}");
                
                Save save3 = Assets.LoadSave(3);
                Logger.Trace($"Save 3 {{ Name = {save3.Name}, Level = {save3.Level}, Scores = {((save3.Scores.Count != 0) ? string.Join(", ", save3.Scores.ToArray()) : "<NO SCORES>")} }}");
            }

            Loaded -= OnLoad;

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
                if (kpe.KeyCode == Key.Escape)
                {
                    Logger.Info("Pressed Esc, going back to main menu.");
                    Game.Instance.ActiveScene = ((m_MainMenu != null) ? m_MainMenu : new MainMenu());
                }
                if (kpe.KeyCode == Key.OemPlus) // Debug function to reset saves
                {
                    // Reset saves
                    {
                        // There are 3 saves (hardcoded = bad) ;)
                        for (uint i = 1; i <= 3; i++)
                        {
                            Save save = Assets.LoadSave(i);
                            save.Level = 1;

                            string path = Assets.GetSaveSlotPath(i);
                            string text = JsonSerializer.Serialize<Save>(save);

                            File.WriteAllText(path, text);
                        }

                        MessageBox.Show("[DEBUG] RESET LEVELS.");
                    }
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
            
            Game.Instance.ActiveScene = new LevelOverlay(Assets.LoadSave(slot), slot);
            
            Assets.IntroMusic.CloseAll();
            Assets.MainMenuMusic.Stop();
        }

        void SaveSlot1StartHover(object sender, RoutedEventArgs args) { SaveButton1.Opacity = 0; SaveButton1Hover.Opacity = 1; }
        void SaveSlot2StartHover(object sender, RoutedEventArgs args) { SaveButton2.Opacity = 0; SaveButton2Hover.Opacity = 1; }
        void SaveSlot3StartHover(object sender, RoutedEventArgs args) { SaveButton3.Opacity = 0; SaveButton3Hover.Opacity = 1; }
        void SaveSlot1StopHover(object sender, RoutedEventArgs args) { SaveButton1.Opacity = 1; SaveButton1Hover.Opacity = 0; }
        void SaveSlot2StopHover(object sender, RoutedEventArgs args) { SaveButton2.Opacity = 1; SaveButton2Hover.Opacity = 0; }
        void SaveSlot3StopHover(object sender, RoutedEventArgs args) { SaveButton3.Opacity = 1; SaveButton3Hover.Opacity = 0; }

        void BackButtonPressed(object sender, RoutedEventArgs args)
        {
            Game.Instance.ActiveScene = new MainMenu();
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private MainMenu m_MainMenu = null;
        
        private Renderer m_Renderer;

    }

}