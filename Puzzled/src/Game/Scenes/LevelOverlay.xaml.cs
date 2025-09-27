using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Puzzled
{

    ////////////////////////////////////////////////////////////////////////////////////
    // LevelOverlay // Note: Currently being used to test functionality
    ////////////////////////////////////////////////////////////////////////////////////
    public partial class LevelOverlay : UserControl, Scene
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public LevelOverlay()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }
        public LevelOverlay(Save save)
        {
            m_Save = save;

            InitializeComponent();
            Loaded += OnLoad;
        }
        ~LevelOverlay()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void OnLoad(object sender, RoutedEventArgs args) // Note: We need to do this after layout pass to make sure sizes are calculated
        {
            m_Level = new Level(GameCanvas, Assets.LevelToPath(m_Save.Level));
        }

        public void OnUpdate(float deltaTime)
        {
            if (!IsLoaded) return;
            m_Level.OnUpdate(deltaTime);
        }

        public void OnRender()
        {
            if (!IsLoaded) return;
            m_Level.OnRender();
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
            }

            m_Level.OnEvent(e);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Save m_Save;
        private Level m_Level;

    }

}