using System;
using System.Diagnostics;
using System.Collections.Generic;
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
    
            public Texture TextureReference;
            public UV TextureCoords;

            public Maths.Vector4 Colour;
        }
    
        internal class VisualHost : FrameworkElement
        {
            private readonly Visual m_Visual;
            public VisualHost(Visual visual) => m_Visual = visual;
    
            protected override int VisualChildrenCount => 1;
            protected override Visual GetVisualChild(int index) => m_Visual;
        }
    
        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public Renderer(Canvas canvas)
        {
            m_Quads = new List<Quad>();
            m_Canvas = canvas;

            m_Visual = new DrawingVisual();
            m_VisualHost = new VisualHost(m_Visual);
            
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
                    if (quad.TextureReference != null)
                    {
                        CroppedBitmap cropped = new CroppedBitmap(
                            quad.TextureReference.GetInternalImage(),
                            new Int32Rect((int)quad.TextureCoords.X, (int)quad.TextureCoords.Y, (int)quad.TextureCoords.Width, (int)quad.TextureCoords.Height) // UV rectangle in pixels
                        );
                    
                        dc.DrawImage(cropped, new Rect(quad.Position.X, quad.Position.Y, quad.Size.X, quad.Size.Y));
                    }
                    else // TODO: Optimize brush? // TODO: Support transparency?
                    {
                        Brush brush = new SolidColorBrush(Color.FromRgb((byte)((uint)(quad.Colour.X * 255.0f)), (byte)((uint)(quad.Colour.Y * 255.0f)), (byte)((uint)(quad.Colour.Z * 255.0f))));
                        dc.DrawRectangle(brush, null, new Rect(quad.Position.X, quad.Position.Y, quad.Size.X, quad.Size.Y));
                    }
                }
            }
        }
    
        public void AddQuad(Maths.Vector2 position, Maths.Vector2 size, Maths.Vector4 colour) { m_Quads.Add(new Quad{ Position = position, Size = size, Colour = colour }); }
        public void AddQuad(Maths.Vector2 position, Maths.Vector2 size, Texture texture) { m_Quads.Add(new Quad{ Position = position, Size = size, TextureReference = texture, TextureCoords = new UV(0, 0, texture.Width, texture.Height) }); }
        public void AddQuad(Maths.Vector2 position, Maths.Vector2 size, Texture texture, UV textureCoords) { m_Quads.Add(new Quad{ Position = position, Size = size, TextureReference = texture, TextureCoords = textureCoords }); }
        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private List<Quad> m_Quads;
        private Canvas m_Canvas;

        private DrawingVisual m_Visual;
        private VisualHost m_VisualHost;
    
    }

}
