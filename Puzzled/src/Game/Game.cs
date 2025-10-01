using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Puzzled
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

            ActiveScene = new MainMenu();
        }
        ~Game()
        {
            Instance = null;
        }
    
        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        public void OnUpdate(float deltaTime)
        {
            ActiveScene.OnUpdate(deltaTime);
        }
    
        public void OnRender()
        {
            ActiveScene.OnRender();
        }

        public void OnUIRender()
        {
            ActiveScene.OnUIRender();
        }

        public void OnEvent(Event e)
        {
            ActiveScene.OnEvent(e);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        public MainWindow Window;
        public IScene ActiveScene { get { return (IScene)Window.ActiveScene; } set { Window.ActiveScene = value; } }

        ////////////////////////////////////////////////////////////////////////////////////
        // Static variables
        ////////////////////////////////////////////////////////////////////////////////////
        public static Game Instance { get; private set; }
    
    }

}