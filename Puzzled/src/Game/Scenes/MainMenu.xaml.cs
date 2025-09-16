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
                s_StartupAudio.Play();
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
                    m_GameName.Position = new Maths.Vector2(center.X, m_GameNameHeight);
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
            if (m_GameName.Position.Y > m_GameNameHeight) // Note: The title starts lower (which is higher in this coordinate space)
            {
                m_GameName.Position = new Maths.Vector2(m_GameName.Position.X, m_GameName.Position.Y - (m_UIVelocity * deltaTime));
            }
            else // Note: Only start updating the press start after title reaches height
            {
                // TODO: Stop somewhere
                if (!s_LoopAudio.IsPlaying())
                    s_LoopAudio.Start();

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
                    s_StartupAudio.CloseAll();
                    m_GameName.Position = new Maths.Vector2(m_GameName.Position.X, m_GameNameHeight);
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
        private const float m_UIVelocity = 61.75f; // Matches Intro.wav

        private UI.Text m_GameName;
        private const float m_GameNameHeight = 180.0f;

        private UI.Text m_PressStart;

        private const float m_FlashTime = 0.4f;
        private float m_CurrentFlashTimer = 0.0f;
        private bool m_PressStartRendered = false;

        // Sounds
        private static FireableAudio s_StartupAudio = new FireableAudio(Assets.StartupMusicPath, 5);
        private static LoopAudio s_LoopAudio = new LoopAudio(Assets.MainMenuMusicPath, 5);

        ////////////////////////////////////////////////////////////////////////////////////
        // Static variables
        ////////////////////////////////////////////////////////////////////////////////////
        private static bool s_BootUp = true;
        private static bool s_AnimationPlayed = false;

    }

}