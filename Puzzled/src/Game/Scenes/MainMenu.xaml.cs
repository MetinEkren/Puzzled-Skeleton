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
    public partial class MainMenu : UserControl, Scene
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public MainMenu()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }
        ~MainMenu()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void OnLoad(object sender, RoutedEventArgs args) // Note: We need to do this after layout pass to make sure sizes are calculated
        {
            m_Loaded = true;
            m_Renderer = new Renderer(GameCanvas);

            m_GameName = new UI.Text(
                "Puzzled Skeleton",
                64.0f,

                "Courier New",
                new Maths.Vector2(0.0f, 0.0f)
            );

            m_GameName.Position = UI.Utils.GetCenter(UICanvas, m_GameName.UIElement);
            m_GameName.AddToCanvas(UICanvas);
        }

        public void OnUpdate(float deltaTime)
        {
            if (m_Loaded)
            {
                m_GameName.Position = new Maths.Vector2(m_GameName.Position.X, m_GameName.Position.Y - (50.0f * deltaTime));
            }
        }

        public void OnRender()
        {
            // TODO: Render some sort of background?
        }

        public void OnUIRender()
        {
        }

        public void OnEvent(Event e)
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private bool m_Loaded = false; // TODO: Find a better way
        private Renderer m_Renderer;

        private UI.Text m_GameName;
        

    }

}