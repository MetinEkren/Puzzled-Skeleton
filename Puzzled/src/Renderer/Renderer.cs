using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static Puzzled.Renderer;

namespace Puzzled
{

    //////////////////////////////////////////////////////////////////////////////////
    // Renderer
    //////////////////////////////////////////////////////////////////////////////////
    public class Renderer
    {
    
        ////////////////////////////////////////////////////////////////////////////////////
        // Internal structs
        ////////////////////////////////////////////////////////////////////////////////////
        internal struct Quad
        {
            public Maths.Vector2 Size;
            public Maths.Vector2 Position;
    
            public ITexture TextureReference;
        }

        internal class VisualHost : FrameworkElement
        {
            private readonly VisualCollection m_Children;

            public VisualHost() { m_Children = new VisualCollection(this); }

            public void AddVisual(Visual visual) { m_Children.Add(visual); }

            protected override int VisualChildrenCount => m_Children.Count;
            protected override Visual GetVisualChild(int index) => m_Children[index];
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public Renderer(Canvas canvas)
        {
            m_Canvas = canvas;
            m_Quads = new List<Quad>();

            m_Visual = new DrawingVisual();

            m_VisualHost = new VisualHost();
            m_VisualHost.AddVisual(m_Visual);

            m_Canvas.Children.Add(m_VisualHost);
        }
        ~Renderer()
        {
        }
    
        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        public void Begin()
        {
            m_Quads.Clear();
        }

        public void End()
        {
            using (DrawingContext dc = m_Visual.RenderOpen())
            {
                foreach (Quad quad in m_Quads)
                {
                    dc.DrawImage(quad.TextureReference.GetImageSource(), new Rect(quad.Position.X, quad.Position.Y, quad.Size.X, quad.Size.Y));
                }
            }

            m_VisualHost.InvalidateVisual();
        }

        public void AddQuad(Maths.Vector2 position, Maths.Vector2 size, ITexture texture) 
        {
            m_Quads.Add(new Quad
            { 
                Position = new Maths.Vector2(position.X, (float)m_Canvas.ActualHeight - size.Y - position.Y), 
                Size = size, 
                TextureReference = texture, 
            }); 
        }
        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Canvas m_Canvas;
        private List<Quad> m_Quads;

        private DrawingVisual m_Visual;
        private VisualHost m_VisualHost;

    }

}
