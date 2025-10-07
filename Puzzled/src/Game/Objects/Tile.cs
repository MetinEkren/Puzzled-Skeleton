using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Puzzled
{

    //////////////////////////////////////////////////////////////////////////////////
    // Tile
    //////////////////////////////////////////////////////////////////////////////////
    public class Tile
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public Tile(Maths.Vector2 position, Maths.Vector2 size, ITexture texture)
        {
            m_Position = position;
            m_Size = size;
            m_Texture = texture;

            m_HitboxPosition = m_Position;
            m_HitboxSize = m_Size;
        }
        public Tile(Maths.Vector2 position, Maths.Vector2 size, ITexture texture, Maths.Vector2 hitboxPosition, Maths.Vector2 hitboxSize)
        {
            m_Position = position;
            m_Size = size;
            m_Texture = texture;

            m_HitboxPosition = hitboxPosition;
            m_HitboxSize = hitboxSize;
        }
        ~Tile()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        public void RenderTo(Renderer renderer, bool debug = false)
        {
            renderer.AddQuad(m_Position, m_Size, m_Texture);

            if (debug) // Outline tile hitbox
            {
                renderer.AddQuad(m_HitboxPosition, new Maths.Vector2(m_HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(m_HitboxPosition, new Maths.Vector2(1 * Settings.Scale, m_HitboxSize.Y), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(m_HitboxPosition.X, m_HitboxPosition.Y + m_HitboxSize.Y - (1 * Settings.Scale)), new Maths.Vector2(m_HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(m_HitboxPosition.X + m_HitboxSize.X - (1 * Settings.Scale), m_HitboxPosition.Y), new Maths.Vector2(1 * Settings.Scale, m_HitboxSize.Y), Assets.WhiteTexture);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Maths.Vector2 m_Position;
        private Maths.Vector2 m_Size;
        private ITexture m_Texture;

        private Maths.Vector2 m_HitboxPosition;
        private Maths.Vector2 m_HitboxSize;

        public Maths.Vector2 Position { get { return m_Position; } set { m_Position = value; } }
        public Maths.Vector2 Size { get { return m_Size; } set { m_Size = value; } }
        public ITexture Texture { get { return m_Texture; } }

        public Maths.Vector2 HitboxPosition { get { return m_HitboxPosition; } set { m_HitboxPosition = value; } }
        public Maths.Vector2 HitboxSize { get { return m_HitboxSize; } set { m_HitboxSize = value; } }

    }

}