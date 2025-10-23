using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

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

            Camera = new VerticalCamera(Level.Player);

            m_StopWatch = new CustomStopWatch();

            // Connect stopwatch updates to the overlay & start
            m_StopWatch.TimeUpdated = UpdateStopwatchDisplay;
            m_StopWatch.Start();

            Loaded -= OnLoad;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!IsLoaded) return;
            if (Paused) return;

            Level.OnUpdate(deltaTime);
            Camera.Update();
        }

        public void UpdateStopwatchDisplay(string time)
        {
            Dispatcher.Invoke(() => StopwatchLabel.Content = time);
        }

        public void OnRender()
        {
            if (!IsLoaded) return;

            m_Renderer.Begin();
            Level.OnRender();

            // Note: Darkens de the background on pause
            if (Paused) // Note: We use the camera offset to make sure it's fully in screen
                m_Renderer.AddQuad(new Maths.Vector2(-Camera.XOffset, -Camera.YOffset), new Maths.Vector2(Game.Instance.Window.Width, Game.Instance.Window.Height), Assets.BlackTexture, 40);
            
            m_Renderer.End(Camera);
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
                    if (Paused == false) 
                    {
                        PauseOverlay.Content = new LevelOverlay_Pauze(this, m_StopWatch);
                        Paused = true;
                        m_StopWatch.Pauze();
                    }
                    else if (Paused == true)
                    {
                        PauseOverlay.Content = null; // This removes the overlay
                        Paused = false;
                        m_StopWatch.Start();
                    }
                }
                if (kpe.KeyCode == Key.Enter) // TODO: Change to win condition
                {
                    if (m_Save.Level == Assets.LevelCount) // Finish the game
                    {
                        Game.Instance.ActiveScene = new MainMenu();
                    }
                    else // Win a level
                    {
                        ++m_Save.Level;
                        Save();
                        
                        Game.Instance.ActiveScene = new WinMenu(this);
                    }
                }
            }

            Level.OnEvent(e);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Extra method
        ////////////////////////////////////////////////////////////////////////////////////
        public void LoadLevel(uint level)
        {
            // Note: +1 for final level
            Debug.Assert(((level <= Assets.LevelCount + 1) && (level != 0)), "Invalid level passed in.");
            
            Level = new Level(GameCanvas, m_Renderer, Assets.LevelToPath(level));
            
            m_Save.Level = level;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Private methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void Save()
        {
            Logger.Info($"Saving to slot {m_SaveSlot}.");

            string path = Assets.GetSaveSlotPath(m_SaveSlot);
            string text = JsonSerializer.Serialize<Save>(m_Save);

            File.WriteAllText(path, text);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Renderer m_Renderer;
        
        private Save m_Save;
        private readonly uint m_SaveSlot;

        public Level Level;
        public VerticalCamera Camera;

        public bool Paused = false;

        private Puzzled.CustomStopWatch m_StopWatch;
        public Save ActiveSave { get { return m_Save; } set { m_Save = value; } }

    }

}