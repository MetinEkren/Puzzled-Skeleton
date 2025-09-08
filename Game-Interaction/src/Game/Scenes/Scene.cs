using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace GameInteraction
{

    ////////////////////////////////////////////////////////////////////////////////////
    // Scene
    ////////////////////////////////////////////////////////////////////////////////////
    public class Scene
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public Scene()
        {
            Game.Instance.Window.TickMethod = OnTick;
            Game.Instance.Window.EventMethod = OnEvent;
        }
        public Scene(Action<float> onUpdate, Action onRender, Action<Event> onEvent)
        {
            Game.Instance.Window.TickMethod = OnTick;
            Game.Instance.Window.EventMethod = OnEvent;

            OnUpdateMethod = onUpdate;
            OnRenderMethod = onRender;
            OnEventMethod = onEvent;
        }
        ~Scene()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void OnUpdate(float deltaTime)
        {
            OnUpdateMethod?.Invoke(deltaTime);
        }

        private void OnRender()
        {
            OnRenderMethod?.Invoke();
        }

        private void OnEvent(Event e)
        {
            OnEventMethod?.Invoke(e);
        }
        
        private void OnTick(double deltaTime)
        {
            OnUpdate((float)deltaTime);
            OnRender();
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        public Action<float> OnUpdateMethod { get; set; }
        public Action OnRenderMethod { get; set; }
        public Action<Event> OnEventMethod { get; set; }

    }

}