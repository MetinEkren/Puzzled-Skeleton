using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Puzzled
{

    ////////////////////////////////////////////////////////////////////////////////////
    // MainMenu
    ////////////////////////////////////////////////////////////////////////////////////
    public partial class MainWindow : Window
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public MainWindow()
        {
            InitializeComponent();

            // Setup loop & rendering settings
            CompositionTarget.Rendering += Tick;
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);

            // Setup callbacks
            this.SizeChanged += WindowResize;
            this.Closing += WindowClose;
            
            this.KeyDown += KeyPressed;
            this.KeyUp += KeyReleased;
            
            this.MouseDown += MousePressed;
            this.MouseUp += MouseReleased;
            this.MouseMove += MouseMoved;
            this.MouseWheel += MouseScrolled;

            m_Game = new Game(this);
        }
        ~MainWindow()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Private methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            double deltaTime = (now - m_LastTime).TotalSeconds;
            m_LastTime = now;

            m_Game.OnUpdate((float)deltaTime);
            m_Game.OnRender();
            m_Game.OnUIRender();
        }

        private void OnEvent(Event e)
        {
            m_Game.OnEvent(e);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Callbacks
        ////////////////////////////////////////////////////////////////////////////////////
        private void WindowResize(object sender, SizeChangedEventArgs e) { OnEvent(new WindowResizeEvent((uint)e.NewSize.Width, (uint)e.NewSize.Height)); }
        private void WindowClose(object sender, System.ComponentModel.CancelEventArgs e) { OnEvent(new WindowCloseEvent()); }

        private void KeyPressed(object sender, KeyEventArgs e) { OnEvent(new KeyPressedEvent(e.Key, e.IsRepeat)); }
        private void KeyReleased(object sender, KeyEventArgs e) { OnEvent(new KeyReleasedEvent(e.Key)); }

        private void MousePressed(object sender, MouseButtonEventArgs e) { OnEvent(new MouseButtonPressedEvent(e.ChangedButton)); }
        private void MouseReleased(object sender, MouseButtonEventArgs e) { OnEvent(new MouseButtonReleasedEvent(e.ChangedButton)); }
        private void MouseMoved(object sender, MouseEventArgs e) { OnEvent(new MouseMovedEvent((float)e.GetPosition(this).X, (float)e.GetPosition(this).Y)); }
        private void MouseScrolled(object sender, MouseWheelEventArgs e) { OnEvent(new MouseScrolledEvent(e.Delta / 120.0f)); } // Note: Positive = up, Negative = down, Each scroll is 120 units

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private DateTime m_LastTime = DateTime.Now;

        private Game m_Game;
        public object ActiveScene { get { return SceneContainer.Content; } set { SceneContainer.Content = value; } }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variable throughput
        ////////////////////////////////////////////////////////////////////////////////////
        public new uint Width { get { return (uint)(this.Content as FrameworkElement).ActualWidth; ; } }
        public new uint Height { get { return (uint)(this.Content as FrameworkElement).ActualHeight; ; } }

    }

}