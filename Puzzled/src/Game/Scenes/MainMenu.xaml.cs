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

            m_Startup = new AudioFile("../../../Puzzled/Resources/Music/Title_Screen_Intro.wav");
            m_Loop = new AudioFile("../../../Puzzled/Resources/Music/Title_Screen_Loop.wav");

            m_Startup.Play();
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

                if (!s_AnimationPlayed)
                {
                    m_GameName.Position = new Maths.Vector2(center.X, Game.Instance.Window.Height);
                }
                else
                {
                    m_GameName.Position = new Maths.Vector2(center.X, m_GameNameHeight);
                }

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
                // Note: We don't add it yet since we want it to render later
            }
        }

        public void OnUpdate(float deltaTime)
        {
            if (!m_Loaded) return;

            // Title movement
            if (m_GameName.Position.Y > m_GameNameHeight) // Note: The title starts lower (which is higher in this coordinate space)
            {
                m_GameName.Position = new Maths.Vector2(m_GameName.Position.X, m_GameName.Position.Y - (m_UIVelocity * deltaTime));
            }
            else // Note: Only start updating the press start after title reaches height
            {
                if (!s_AnimationPlayed)
                {
                    m_Loop.Play();
                }

                s_AnimationPlayed = true;

                // Flashing press start
                m_CurrentFlashTimer += deltaTime;
                if (m_CurrentFlashTimer >= m_FlashTime)
                {
                    if (m_PressStartRendered)
                    {
                        m_PressStart.RemoveFromCanvas(UICanvas);
                        m_PressStartRendered = false;
                    }
                    else
                    {
                        m_PressStart.AddToCanvas(UICanvas);
                        m_PressStartRendered = true;
                    }

                    m_CurrentFlashTimer = 0.0f;
                }
            }
        }

        public void OnRender()
        {
            if (!m_Loaded) return;

            // TODO: Render some sort of background?
        }

        public void OnUIRender()
        {
            if (!m_Loaded) return;

        }

        public void OnEvent(Event e)
        {
            if (!m_Loaded) return;

            Action press = () =>
            {
                if (s_AnimationPlayed)
                {
                    Game.Instance.ActiveScene = new SavesMenu();
                }
                else
                {
                    m_GameName.Position = new Maths.Vector2(m_GameName.Position.X, m_GameNameHeight);
                }
            };

            if (e is MouseButtonPressedEvent mbe)
            {
                press();
            }
            if (e is KeyPressedEvent kpe)
            {
                press();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private bool m_Loaded = false; // TODO: Find a better way
        private Renderer m_Renderer;

        // Animation
        private const float m_UIVelocity = 86.3f; // Matches Intro.wav
        
        private UI.Text m_GameName;
        private const float m_GameNameHeight = 180.0f;

        private UI.Text m_PressStart;

        private const float m_FlashTime = 0.4f;
        private float m_CurrentFlashTimer = 0.0f;
        private bool m_PressStartRendered = false;

        // Sounds
        private AudioFile m_Startup;
        private AudioFile m_Loop;

        ////////////////////////////////////////////////////////////////////////////////////
        // Static variables
        ////////////////////////////////////////////////////////////////////////////////////
        private static bool s_AnimationPlayed = false;

    }

}