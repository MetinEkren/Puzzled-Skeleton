using System;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.Json;

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
            m_Renderer = new Renderer(GameCanvas);

            LoadLevel(m_Save.Level);
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
                    Game.Instance.ActiveScene = new WinMenu(this);
                }
            }

            m_Level.OnEvent(e);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Extra methods
        ////////////////////////////////////////////////////////////////////////////////////
        // Note: Used for setting the level we're on before calling Save(), which uses that value.
        public void SetLevel(uint level) { m_Save.Level = level; }
        
        public void LoadLevel(uint level)
        {
            // Note: +1 for final level
            Debug.Assert(((level <= Assets.LevelCount + 1) && (level != 0)), "Invalid level passed in.");
            
            m_Level = new Level(GameCanvas, m_Renderer, Assets.LevelToPath(level));
            
            m_Save.Level = level;
        }

        public void Save()
        {
            Logger.Info($"Saving to slot {m_SaveSlot}.");

            string path = SavesMenu.GetSaveSlotPath(m_SaveSlot);
            string text = JsonSerializer.Serialize<Save>(m_Save);

            File.WriteAllText(path, text);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Renderer m_Renderer;
        
        private Save m_Save;
        private readonly uint m_SaveSlot;

        private Level m_Level;

        public Save ActiveSave { get { return m_Save; } }

    }

}