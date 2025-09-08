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
    public partial class MainMenu
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public MainMenu()
        {
            InitializeComponent();
            
            m_BaseScene = new Scene(this);
            m_BaseScene.OnUpdateMethod = OnUpdate;
            m_BaseScene.OnRenderMethod = OnRender;
            m_BaseScene.OnEventMethod = OnEvent;
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

    }

}