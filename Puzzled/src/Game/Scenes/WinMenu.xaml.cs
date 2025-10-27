using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;

namespace Puzzled
{

    ////////////////////////////////////////////////////////////////////////////////////
    // WinMenu // Note: Currently being used to test functionality
    ////////////////////////////////////////////////////////////////////////////////////
    public partial class WinMenu : UserControl, IScene
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public WinMenu()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }
        public WinMenu(LevelOverlay instance)
        {
            m_Level = instance;

            InitializeComponent();
            Loaded += OnLoad;
        }
        ~WinMenu()
        {
            // Note: For future, don't put anything in destructor, since objects are not destroyed at set moment. (GC moment)
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void OnLoad(object sender, RoutedEventArgs args) // Note: We need to do this after layout pass to make sure sizes are calculated
        {
            m_Renderer = new Renderer(UICanvas);
            
            Save save1 = Assets.LoadSave(1); // Load all info from LoadSave1 and set it in save1
            Save save2 = Assets.LoadSave(2);
            Save save3 = Assets.LoadSave(3);

            List<Save> saves = new List<Save>(); // Makes a list for the saves

            // First - 1 is to convert get the previous level (which we just won), second -1 is to convert level to index
            if (save1.Scores[(int)m_Level.ActiveSave.Level - 1 - 1] != 0)  // Check if score is not 0 and add to list
            {
                saves.Add(save1); 
            }
            if (save2.Scores[(int)m_Level.ActiveSave.Level - 1 - 1] != 0) 
            {
                saves.Add(save2);
            }
            if (save3.Scores[(int)m_Level.ActiveSave.Level - 1 - 1] != 0) 
            {
                saves.Add(save3);
            }

            saves.Sort(new SortSaveScoreHelper(m_Level.ActiveSave.Level - 1 - 1)); // Sort the list based on scores for the previous level (which you just played)

            // Set the top 3 scores in the UI
            if (saves.Count >= 1) // Check if there is a first place
            {
                // Set name and score for first place from sorted list
                PlayerName1.Text = saves[0].Name; 
                PlayerScore1.Text = saves[0].Scores[(int)m_Level.ActiveSave.Level - 1 - 1].ToString();
            }
            if (saves.Count >= 2) // Check if there is a second place
            {
                // Set name and score for second place from sorted list
                PlayerName2.Text = saves[1].Name; 
                PlayerScore2.Text = saves[1].Scores[(int)m_Level.ActiveSave.Level - 1 - 1].ToString();
            }
            if (saves.Count == 3) // Check if there is a third place
            {
                // Set name and score for third place from sorted list
                PlayerName3.Text = saves[2].Name; 
                PlayerScore3.Text = saves[2].Scores[(int)m_Level.ActiveSave.Level - 1 - 1].ToString();
            }

            // Play the game music in the win menu
            if (Assets.LevelMusic.IsPlaying())
                Assets.LevelMusic.Stop();
            Assets.WinMenuMusic.Start();

            Loaded -= OnLoad;
        }
        public void OnUpdate(float deltaTime)
        {
            if (!IsLoaded) return;
            m_IdleAnimation.Update(deltaTime);
        }

        // Is not used in this scene
        public void OnRender()
        {
            if (!IsLoaded) return;
        }

        // This is to render the idle animation of the character in the win menu
        public void OnUIRender()
        {
            if (!IsLoaded) return;
            m_Renderer.Begin();
            m_Renderer.AddQuad(new Maths.Vector2(570f, 0.0f), new Maths.Vector2(200f, 200f), m_IdleAnimation.GetCurrentTexture(), true);
            m_Renderer.End();
        }

        // Is not used in this scene
        public void OnEvent(Event e)
        {
            if (!IsLoaded) return;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Callbacks
        ////////////////////////////////////////////////////////////////////////////////////
        
        // Load the next level when the button is pressed
        void NextLevelPressed(object sender, RoutedEventArgs args)
        {
            Logger.Info($"Going to next level, {m_Level.ActiveSave.Level}.");
            m_Level.LoadLevel(m_Level.ActiveSave.Level);
            Game.Instance.ActiveScene = m_Level;
        }

        // Go back to main menu when the button is pressed
        void BackToMainMenu(object sender, RoutedEventArgs args)
        {
            Logger.Info("Going back to main menu.");
            Game.Instance.ActiveScene = new MainMenu();
        }

        // Restart the current level when the button is pressed
        void RestartGame(object sender, RoutedEventArgs args)
        {
            Logger.Info($"Going to next level, {m_Level.ActiveSave.Level}.");

            m_Level.ActiveSave = new Save
            {
                Name = m_Level.ActiveSave.Name,
                Level = m_Level.ActiveSave.Level - 1,
                Scores = m_Level.ActiveSave.Scores
            }; // Restart current level

            m_Level.LoadLevel(m_Level.ActiveSave.Level);
            Game.Instance.ActiveScene = m_Level;
        }

        // Go to options menu when the button is pressed
        void OptionsMenu(object sender, RoutedEventArgs args)
        {
            Logger.Info("Going to option menu");
            Game.Instance.ActiveScene = new OptionsMenu(this);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Renderer m_Renderer;

        private LevelOverlay m_Level;
        
        // Load image for idle animation
        private Animation m_IdleAnimation = new Animation(Assets.IdleSheet, (Settings.SpriteSize / Settings.Scale), Settings.IdleAdvanceTime);

    }

}