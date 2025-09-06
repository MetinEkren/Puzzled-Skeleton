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
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            Rectangle rect = new Rectangle
            {
                Tag = "TESTRECT",
                Height = 64,
                Width = 64,
                Fill = brush
            };

            Canvas.SetLeft(rect, 0.0f);     // X
            Canvas.SetTop(rect, 0.0f);      // Y

            // finally add the circle to the canvas
            GameWindow.Instance.WindowCanvas.Children.Add(rect);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        public static Game Instance { get; private set; }

    }

}