using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace GameInteraction
{

    //////////////////////////////////////////////////////////////////////////////////
    // Game (everything originates from here)
    //////////////////////////////////////////////////////////////////////////////////
    public class Game
    {
    
        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public Game(MainWindow window)
        {
            Instance = this;
            Window = window;

            Window.TickMethod = Tick;
            Window.EventMethod = OnEvent;

            ActiveScene = new MainMenu();
        }
        ~Game()
        {
            Instance = null;
        }
    
        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void OnUpdate(float deltaTime)
        {
        }
    
        private void OnRender()
        {
            //m_Renderer.Begin();
            //
            ////m_Renderer.AddQuad(new Maths.Vector2(0.0f, 0.0f), new Maths.Vector2(64.0f, 64.0f), m_TESTTexture, new UV(0, 0, 1024, 1024));
            //m_Renderer.AddQuad(new Maths.Vector2(0.0f, 0.0f), new Maths.Vector2(64.0f, 64.0f), m_TESTTexture, new UV(0, 0, m_TESTTexture.Width, m_TESTTexture.Height));
            //
            //m_Renderer.End();
        }

        private void OnEvent(Event e)
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Hidden
        ////////////////////////////////////////////////////////////////////////////////////
        private void Tick(double deltaTime)
        {
            OnUpdate((float)deltaTime);
            OnRender();
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        public MainWindow Window;
        public object ActiveScene { get { return Window.ActiveScene; } set { Window.ActiveScene = value; } }

        public static Game Instance { get; private set; }
    
    }

}