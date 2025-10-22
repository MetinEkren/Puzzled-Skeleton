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

            public bool FlipTexture;

            // Note: 2 Aliases
            public uint Opacity { get { return m_Opacity; } set { m_Opacity = (uint)Math.Min(value, 100.0); } }
            public uint Transparency { get { return Opacity; } set { Opacity = value; } }

            private uint m_Opacity;
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
                    if (quad.FlipTexture)
                    {
                        double centerX = quad.Position.X + quad.Size.X / 2;
                        double centerY = quad.Position.Y + quad.Size.Y / 2;

                        dc.PushTransform(new ScaleTransform(-1.0f, 1.0f, centerX, centerY));
                    }

                    dc.DrawImage(quad.TextureReference.GetImageSource(), new Rect(quad.Position.X + 50, quad.Position.Y + 50, quad.Size.X, quad.Size.Y));

                    dc.PushOpacity(quad.Opacity / 100.0);
                    //dc.DrawImage(quad.TextureReference.GetImageSource(), new Rect(quad.Position.X, quad.Position.Y, quad.Size.X, quad.Size.Y));
                    dc.Pop();

                    if (quad.FlipTexture)
                        dc.Pop();
                }
            }

            //m_VisualHost.InvalidateVisual();
        }

        public void AddQuad(Maths.Vector2 position, Maths.Vector2 size, ITexture texture) { AddQuad(position, size, texture, false, 100); } 
        public void AddQuad(Maths.Vector2 position, Maths.Vector2 size, ITexture texture, uint opacity) { AddQuad(position, size, texture, false, opacity); } 
        public void AddQuad(Maths.Vector2 position, Maths.Vector2 size, ITexture texture, bool flipTexture) { AddQuad(position, size, texture, flipTexture, 100); } 
        public void AddQuad(Maths.Vector2 position, Maths.Vector2 size, ITexture texture, bool flipTexture, uint opacity)
        {
            m_Quads.Add(new Quad
            { 
                Position = new Maths.Vector2(position.X, (float)m_Canvas.ActualHeight - size.Y - position.Y), 
                Size = size, 
                TextureReference = texture,

                FlipTexture = flipTexture,
                Opacity = opacity,
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
