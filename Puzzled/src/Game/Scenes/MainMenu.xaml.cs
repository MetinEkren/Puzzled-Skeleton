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

            // Game name
            {
                m_GameName = new UI.Text(
                    "Puzzled Skeleton",
                    64.0f,

                    "Courier New",
                    new Maths.Vector2(0.0f, 0.0f)
                );

                Maths.Vector2 center = UI.Utils.GetCenter(UICanvas, m_GameName.UIElement);
                m_GameName.Position = new Maths.Vector2(center.X, Game.Instance.Window.Height);
                m_GameName.AddToCanvas(UICanvas);
            }

            // Press start
            {
                m_PressStart = new UI.Text(
                    "Press Start",
                    32.0f,

                    "Courier New",
                    new Maths.Vector2(0.0f, 0.0f)
                );

                Maths.Vector2 center = UI.Utils.GetCenter(UICanvas, m_PressStart.UIElement);
                m_PressStart.Position = new Maths.Vector2(center.X, center.Y + 200.0f);
                m_PressStart.AddToCanvas(UICanvas);
            }
        }

        public void OnUpdate(float deltaTime)
        {
            // UI updates
            if (m_Loaded && m_GameName.Position.Y > 157.0f) // TODO: Remove magic number
            {
                m_GameName.Position = new Maths.Vector2(m_GameName.Position.X, m_GameName.Position.Y - (m_UIVelocity * deltaTime));
            }

            // Note: Ugly ass code // Note 2: Proof of concept
            m_CurrentFlashTimer += deltaTime;
            if (m_CurrentFlashTimer >= m_FlashTime)
            {
                if (m_PressStart.UIElement.Foreground == Brushes.White)
                {
                    m_PressStart.UIElement.Foreground = Brushes.Black;
                }
                else
                {
                    m_PressStart.UIElement.Foreground = Brushes.White;
                }
                m_CurrentFlashTimer = 0.0f;
            }
        }

        public void OnRender()
        {
            // TODO: Render some sort of background?
            // TODO: Flash press start
        }

        public void OnUIRender()
        {
        }

        public void OnEvent(Event e)
        {
            if (e is MouseButtonPressedEvent mbe)
            {
                Logger.Trace($"Position {{ .x = {m_GameName.Position.X}, .y = {m_GameName.Position.Y} }}");
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private bool m_Loaded = false; // TODO: Find a better way
        private Renderer m_Renderer;

        // Animation
        private const float m_UIVelocity = 100.0f;
        
        private UI.Text m_GameName; // End: 157
        private UI.Text m_PressStart;

        private const float m_FlashTime = 0.4f;
        private float m_CurrentFlashTimer = 0.0f;

    }

}