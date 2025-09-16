using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Puzzled
{

    ////////////////////////////////////////////////////////////////////////////////////
    // MainMenu
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
            m_Loaded = true;
            m_Renderer = new Renderer(GameCanvas);
        }

        public void OnUpdate(float deltaTime)
        {
            if (!m_Loaded) return;
        }

        public void OnRender()
        {
            if (!m_Loaded) return;
        }

        public void OnUIRender()
        {
            if (!m_Loaded) return;
        }

        public void OnEvent(Event e)
        {
            if (!m_Loaded) return;

            if (e is KeyPressedEvent kpe)
            {
                if (kpe.KeyCode == System.Windows.Input.Key.Escape)
                {
                    Game.Instance.ActiveScene = new MainMenu();
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Callbacks
        ////////////////////////////////////////////////////////////////////////////////////
        void SaveSlot1Pressed(object sender, RoutedEventArgs args)
        {

        }

        void SaveSlot2Pressed(object sender, RoutedEventArgs args)
        {

        }

        void SaveSlot3Pressed(object sender, RoutedEventArgs args)
        {

        } 

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private bool m_Loaded = false; // TODO: Find a better way
        private Renderer m_Renderer;

    }

}