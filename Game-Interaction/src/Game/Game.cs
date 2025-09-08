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
            ActiveScene.OnUpdate(deltaTime);
        }
    
        private void OnRender()
        {
            ActiveScene.OnRender();
        }

        private void OnEvent(Event e)
        {
            ActiveScene.OnEvent(e);
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
        public Scene ActiveScene { get { return (Scene)Window.ActiveScene; } set { Window.ActiveScene = value; } }

        public static Game Instance { get; private set; }
    
    }

}