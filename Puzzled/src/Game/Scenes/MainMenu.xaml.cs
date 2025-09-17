using System.Windows;
using System.Windows.Controls;

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

            if (s_BootUp)
            {
                Assets.IntroMusic.Play();
                s_BootUp = false;
            }
        }
        ~MainMenu()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void OnLoad(object sender, RoutedEventArgs args) // Note: We need to do this after layout pass to make sure sizes are calculated
        {
            m_Renderer = new Renderer(GameCanvas);

            // Game name
            {
                m_GameName = new UI.Text(
                    "Puzzled Skeleton",
                    48.0f,

                    "Courier New",
                    new Maths.Vector2(0.0f, 0.0f)
                );

                Maths.Vector2 center = UI.Utils.GetCenter(UICanvas, m_GameName.UIElement);

                if (!s_AnimationPlayed)
                {
                    // Start at bottom to create the animation
                    m_GameName.Position = new Maths.Vector2(center.X, Game.Instance.Window.Height);
                }
                else
                {
                    // Startup animation has already been played so just start at desired height
                    m_GameName.Position = new Maths.Vector2(center.X, c_GameNameHeight);
                }

                m_GameName.AddToCanvas(UICanvas);
            }

            // Press start
            {
                m_PressStart = new UI.Text(
                    "Press Any Key",
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
            if (!IsLoaded) return;

            // Title movement
            if (m_GameName.Position.Y > c_GameNameHeight) // Note: The title starts lower (which is higher in this coordinate space)
            {
                m_GameName.Position = new Maths.Vector2(m_GameName.Position.X, m_GameName.Position.Y - (c_UIVelocity * deltaTime));
            }
            else // Note: Only start updating the press start after title reaches height
            {
                // TODO: Stop somewhere
                if (!Assets.MainMenuMusic.IsPlaying())
                    Assets.MainMenuMusic.Start();

                s_AnimationPlayed = true;

                // Flashing press start
                m_CurrentFlashTimer += deltaTime;
                if (m_CurrentFlashTimer >= c_TimeBetweenFlashes)
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
            if (!IsLoaded) return;

            // TODO: Render some sort of background?
        }

        public void OnUIRender()
        {
            if (!IsLoaded) return;
        }

        public void OnEvent(Event e)
        {
            if (!IsLoaded) return;

            void PressCallback()
            {
                if (s_AnimationPlayed)
                {
                    Game.Instance.ActiveScene = new SavesMenu();
                }
                else
                {
                    // Note: If the startup animation is still playing skip that animation (move title to height)
                    Assets.IntroMusic.CloseAll();
                    m_GameName.Position = new Maths.Vector2(m_GameName.Position.X, c_GameNameHeight);
                }
            }

            // On any event do press()
            if (e is MouseButtonPressedEvent) { PressCallback(); }
            if (e is KeyPressedEvent) { PressCallback(); }

            // TODO: Remove after testing
            if (e is MouseScrolledEvent)
            {
                Game.Instance.ActiveScene = new LevelOverlay();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Renderer m_Renderer;

        // Animation
        private UI.Text m_GameName;
        private UI.Text m_PressStart;

        private float m_CurrentFlashTimer = 0.0f;
        private bool m_PressStartRendered = false;

        ////////////////////////////////////////////////////////////////////////////////////
        // Static variables
        ////////////////////////////////////////////////////////////////////////////////////
        private static bool s_BootUp = true;
        private static bool s_AnimationPlayed = false;

        private const float c_UIVelocity = 62.75f; // Matches Intro.wav
        private const float c_GameNameHeight = 180.0f;
        private const float c_TimeBetweenFlashes = 0.4f;


    }

}
