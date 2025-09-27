using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Puzzled
{

    ////////////////////////////////////////////////////////////////////////////////////
    // LevelOverlay // Note: Currently being used to test functionality
    ////////////////////////////////////////////////////////////////////////////////////
    public partial class LevelOverlay : UserControl, IScene
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public LevelOverlay()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }
        public LevelOverlay(Save save, uint slot)
        {
            m_Save = save;
            m_SaveSlot = slot;

            InitializeComponent();
            Loaded += OnLoad;
        }
        ~LevelOverlay()
        {
            // Note: For future, don't put anything in destructor, since objects are not destroyed at set moment. (GC moment)
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void OnLoad(object sender, RoutedEventArgs args) // Note: We need to do this after layout pass to make sure sizes are calculated
        {
            m_Level = new Level(GameCanvas, Assets.LevelToPath(m_Save.Level));
            Loaded -= OnLoad;
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
                if (kpe.KeyCode == Key.Enter)
                {
                    Save();
                    Game.Instance.ActiveScene = new WinMenu(this);
                }
            }

            m_Level.OnEvent(e);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Private methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void Save()
        {
            // TODO: ...
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Save m_Save;
        private readonly uint m_SaveSlot;

        private Level m_Level;

    }

}