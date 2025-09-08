using System;
using System.Text;
using System.Threading;
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
    public partial class WindowBase
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public WindowBase(Window window)
        {
            m_Window = window;

            // Setup loop
            CompositionTarget.Rendering += Tick;

            // Setup callbacks
            m_Window.SizeChanged += WindowResize;
            m_Window.Closing += WindowClose;

            m_Window.KeyDown += KeyPressed;
            m_Window.KeyUp += KeyReleased;

            m_Window.MouseDown += MousePressed;
            m_Window.MouseUp += MouseReleased;
            m_Window.MouseMove += MouseMoved;
            m_Window.MouseWheel += MouseScrolled;
        }
        ~WindowBase()
        {
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

        private void OnEvent(Event e)
        {
            EventMethod?.Invoke(e);
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
        private void MouseMoved(object sender, MouseEventArgs e) { OnEvent(new MouseMovedEvent((float)e.GetPosition(m_Window).X, (float)e.GetPosition(m_Window).Y)); }
        private void MouseScrolled(object sender, MouseWheelEventArgs e) { OnEvent(new MouseScrolledEvent(e.Delta / 120.0f)); } // Note: Positive = up, Negative = down, Each scroll is 120 units // TODO: Fact check

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        public Action<double> TickMethod { get; set; }
        public Action<Event> EventMethod { get; set; }

        public Canvas WindowCanvas { get { return (Canvas)m_Window.FindName("WindowCanvas"); } }

        private DateTime m_LastTime = DateTime.Now;

        private Window m_Window;

        ////////////////////////////////////////////////////////////////////////////////////
        // Variable throughput
        ////////////////////////////////////////////////////////////////////////////////////
        public uint Width { get { return (uint)m_Window.ActualWidth; } }
        public uint Height { get { return (uint)m_Window.ActualHeight; } }

    }

}