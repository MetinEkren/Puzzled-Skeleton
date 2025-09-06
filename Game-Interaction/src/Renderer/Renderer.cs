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

namespace GameInteraction
{

    ////////////////////////////////////////////////////////////////////////////////////
    // Renderer
    ////////////////////////////////////////////////////////////////////////////////////
    public class Renderer
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Internal struct
        ////////////////////////////////////////////////////////////////////////////////////
        internal struct Quad
        {
            public Maths.Vector2 Size;
            public Maths.Vector2 Position;

            public Texture TextureReference;
            public Maths.Vector4 UV;
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
        public Renderer()
        {
            m_Quads = new List<Quad>();
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
            DrawingVisual visual = new DrawingVisual();

            using (DrawingContext dc = visual.RenderOpen())
            {
                foreach (Quad quad in m_Quads)
                {
                    Debug.Assert(quad.TextureReference != null, "Texture must not be null.");
                    
                    // TODO: UVs
                    /*
                     * CroppedBitmap cropped = new CroppedBitmap(
                        quad.Texture.Bitmap,
                        new Int32Rect(uStart, vStart, width, height) // UV rectangle in pixels
                    );
                    */

                    dc.DrawImage(quad.TextureReference.GetInternalImage(), new Rect(quad.Position.X, quad.Position.Y, quad.Size.X, quad.Size.Y));
                }
            }

            // Add the visual to the canvas (or layer)
            VisualHost host = new VisualHost(visual);
            GameWindow.Instance.WindowCanvas.Children.Add(host);
        }

        public void AddQuad(Maths.Vector2 position, Maths.Vector2 size, Texture texture, Maths.Vector4 uv)
        {
            m_Quads.Add(new Quad
            {
                Position = position,
                Size = size,
                TextureReference = texture,
                UV = uv
            });
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private List<Quad> m_Quads;

    }

}
