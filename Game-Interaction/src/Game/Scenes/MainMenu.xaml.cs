using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace GameInteraction
{

    ////////////////////////////////////////////////////////////////////////////////////
    // MainMenu
    ////////////////////////////////////////////////////////////////////////////////////
    public partial class MainMenu : UserControl
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public MainMenu()
        {
            InitializeComponent();
            m_BaseScene = new Scene(OnUpdate, OnRender, OnEvent);
            m_Renderer = new Renderer(GameCanvas);

            m_TESTTexture = new Texture("../../../Game-Interaction/Resources/Textures/viking_room.png");
        }
        ~MainMenu()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        public void OnUpdate(float deltaTime)
        {
            Console.WriteLine($"OnUpdate - {deltaTime}");
        }

        public void OnRender()
        {
            Console.WriteLine("OnRender");

            m_Renderer.Begin();
            m_Renderer.AddQuad(new Maths.Vector2(0.0f, 0.0f), new Maths.Vector2(64.0f, 64.0f), m_TESTTexture, new UV(m_TESTTexture));
            m_Renderer.End();
        }

        public void OnEvent(Event e)
        {
            Console.WriteLine("OnEvent");

            if (e is WindowCloseEvent wce)
            {
                Console.WriteLine("Closing...");
            }
            if (e is MouseScrolledEvent mse)
            {
                Console.WriteLine($"Offset: {mse.YOffset}");
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Scene m_BaseScene;
        private Renderer m_Renderer;

        private Texture m_TESTTexture;

    }

}