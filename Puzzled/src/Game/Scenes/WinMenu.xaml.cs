using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Puzzled
{

    ////////////////////////////////////////////////////////////////////////////////////
    // LevelOverlay // Note: Currently being used to test functionality
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
            m_Level.SetLevel(m_Level.ActiveSave.Level + 1);
            m_Level.Save();
            
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

            if (e is KeyPressedEvent kpe) // TODO: Remove
            {
                if (kpe.KeyCode == Key.Escape)
                {
                    Game.Instance.ActiveScene = new SavesMenu();
                }
                if (kpe.KeyCode == Key.Enter)
                {
                    Game.Instance.ActiveScene = m_Level;
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Callbacks
        ////////////////////////////////////////////////////////////////////////////////////
        void NextLevelPressed(object sender, RoutedEventArgs args)
        {
            if (m_Level.ActiveSave.Level > Assets.LevelCount) // TODO: Remove the ifs // Note: They are currently here for checking logic and seeing output.
            {
                Logger.Info($"Going to final win menu.");

                // Note: Even when it's the final menu it should load a level, since the final win menu is a level.
                m_Level.LoadLevel(m_Level.ActiveSave.Level);

                Game.Instance.ActiveScene = m_Level;
            }
            else
            {
                Logger.Info($"Going to next level, {m_Level.ActiveSave.Level}.");
                m_Level.LoadLevel(m_Level.ActiveSave.Level);
                Game.Instance.ActiveScene = m_Level;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private LevelOverlay m_Level;

    }

}