using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Puzzled
{

    ////////////////////////////////////////////////////////////////////////////////////
    // PauseMenu // Note: Currently being used to test functionality
    ////////////////////////////////////////////////////////////////////////////////////
    public partial class PauseMenu : UserControl, IScene
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public PauseMenu()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }
        public PauseMenu(LevelOverlay instance)
        {
            m_Level = instance;

            InitializeComponent();
            Loaded += OnLoad;
        }
        ~PauseMenu()
        {
            // Note: For future, don't put anything in destructor, since objects are not destroyed at set moment. (GC moment)
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void OnLoad(object sender, RoutedEventArgs args) // Note: We need to do this after layout pass to make sure sizes are calculated
        {
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
                    Game.Instance.ActiveScene = m_Level;
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private LevelOverlay m_Level;

    }

}