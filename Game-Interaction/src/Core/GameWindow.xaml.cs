using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameInteraction
{

    ////////////////////////////////////////////////////////////////////////////////////
    // Window
    ////////////////////////////////////////////////////////////////////////////////////
    public partial class GameWindow : Window
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public GameWindow()
        {
            Instance = this;

            InitializeComponent();
            CompositionTarget.Rendering += Tick;

            m_Game = new Game();
        }
        ~GameWindow()
        {
            Instance = null;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            double deltaTime = (now - m_LastTime).TotalSeconds;
            m_LastTime = now;

            TickMethod?.Invoke(deltaTime);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Callbacks
        ////////////////////////////////////////////////////////////////////////////////////
        private void MousePressed(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Mouse pressed");
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            Console.WriteLine("Key pressed");
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        public Action<double> TickMethod { get; set; }
        private DateTime m_LastTime;

        private Game m_Game;

        public static GameWindow Instance { get; private set; }

    }

}