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
            m_Renderer = new Renderer(GameCanvas);

            m_TESTTexture = new Texture("../../../Puzzled/Resources/Textures/viking_room.png");
            
            m_TESTAudio = new AudioFile("../../../Puzzled/Resources/Sounds/pop.mp3");
            m_TESTAudio.Volume = 10; // %
        }
        ~MainMenu()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        public void OnUpdate(float deltaTime)
        {
        }

        public void OnRender()
        {
            m_Renderer.Begin();
            m_Renderer.AddQuad(new Maths.Vector2(0.0f, 0.0f), new Maths.Vector2(64.0f, 64.0f), m_TESTTexture, new UV(m_TESTTexture));
            m_Renderer.End();
        }

        public void OnEvent(Event e)
        {
            if (e is MouseButtonPressedEvent mbpe)
            {
                Console.WriteLine("AAAA");
                m_TESTAudio.Play();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Renderer m_Renderer;

        private Texture m_TESTTexture;
        private AudioFile m_TESTAudio;

    }

}