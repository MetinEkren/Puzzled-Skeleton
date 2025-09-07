using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace GameInteraction
{

    ////////////////////////////////////////////////////////////////////////////////////
    // Game (everything originates from here)
    ////////////////////////////////////////////////////////////////////////////////////
    public class Game
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public Game()
        {
            Instance = this;
            GameWindow.Instance.TickMethod = Tick;

            m_Renderer = new Renderer();
            m_TESTTexture = new Texture("Resources/Textures/viking_room.png");
        }
        ~Game()
        {
            Instance = null;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        public void Tick(double deltaTime)
        {
            OnUpdate((float)deltaTime);
            OnRender();
        }

        private void OnUpdate(float deltaTime)
        {
        }

        private void OnRender()
        {
            m_Renderer.Begin();

            //m_Renderer.AddQuad(new Maths.Vector2(0.0f, 0.0f), new Maths.Vector2(64.0f, 64.0f), m_TESTTexture, new UV(0, 0, 1024, 1024));
            m_Renderer.AddQuad(new Maths.Vector2(0.0f, 0.0f), new Maths.Vector2(64.0f, 64.0f), m_TESTTexture, new UV(0, 0, m_TESTTexture.Width, m_TESTTexture.Height));

            m_Renderer.End();
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Renderer m_Renderer;
        private Texture m_TESTTexture;
        
        public static Game Instance { get; private set; }

    }

}