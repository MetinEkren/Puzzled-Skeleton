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
            // TODO: Render some sort of background?
        }

        public void OnUIRender()
        {
            DrawText("Hello World!", 64, 32);
        }

        public void OnEvent(Event e)
        {
        }

        void DrawText(string text, double x, double y)
        {
            TextBlock tb = new TextBlock
            {
                Text = text,
                Foreground = Brushes.White,
                FontSize = 64, // adjust for scaling
                FontFamily = new FontFamily("Consolas")
            };

            Canvas.SetLeft(tb, x * 4); // scale by 4
            Canvas.SetTop(tb, y * 4);  // scale by 4

            UICanvas.Children.Add(tb);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Renderer m_Renderer;

        private TextBlock m_GameName;
        

    }

}