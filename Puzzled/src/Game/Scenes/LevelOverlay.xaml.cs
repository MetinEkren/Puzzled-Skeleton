using System.Windows;
using System.Windows.Controls;

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
        ~LevelOverlay()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void OnLoad(object sender, RoutedEventArgs args) // Note: We need to do this after layout pass to make sure sizes are calculated
        {
            m_Renderer = new Renderer(GameCanvas);

            m_TESTTexture = new Texture("../../../Puzzled/Resources/Textures/viking_room.png");
        }

        public void OnUpdate(float deltaTime)
        {
            if (!IsLoaded) return;

            Logger.Trace($"Width = {Game.Instance.Window.Width}, Height = {Game.Instance.Window.Height}");

            // TODO: Move to OnRender, currently used to update positions
            m_Renderer.Begin();
            m_Renderer.AddQuad(new Maths.Vector2(0.0f, 0.0f), new Maths.Vector2(64.0f, 64.0f), m_TESTTexture);
            m_Renderer.AddQuad(new Maths.Vector2(64.0f, 64.0f), new Maths.Vector2(64.0f, 64.0f), m_TESTTexture);
            m_Renderer.AddQuad(new Maths.Vector2(128.0f, 128.0f), new Maths.Vector2(64.0f, 64.0f), m_TESTTexture);
            m_Renderer.AddQuad(new Maths.Vector2(192.0f, 200.0f), new Maths.Vector2(64.0f, 64.0f), m_TESTTexture);
            m_Renderer.AddQuad(new Maths.Vector2(256.0f, Game.Instance.Window.Height - 64.0f), new Maths.Vector2(64.0f, 64.0f), m_TESTTexture);
            m_Renderer.End();
        }

        public void OnRender()
        {
            if (!IsLoaded) return;
        }

        public void OnUIRender()
        {
            if (!IsLoaded) return;
        }

        public void OnEvent(Event e)
        {
            if (!IsLoaded) return;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Renderer m_Renderer;

        private Texture m_TESTTexture;

    }

}