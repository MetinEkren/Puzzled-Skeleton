using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Puzzled
{

    //////////////////////////////////////////////////////////////////////////////////
    // TileType
    //////////////////////////////////////////////////////////////////////////////////
    public enum TileType
    {
        // Note: We use a number + 1, because Tiled shows the number -1 in the editor.
        
        // Ground tiles
        Ground0 = 0 + 1, Ground1 = 1 + 1, Ground2 = 2 + 1,
        Ground3 = 4 + 1,
        Ground4 = 6 + 1, Ground5 = 7 + 1, Ground6 = 8 + 1,
        Ground7 = 9 + 1, Ground8 = 10 + 1, Ground9 = 11 + 1,
        Ground10 = 18 + 1, Ground11 = 19 + 1, Ground12 = 20 + 1,
        Ground13 = 24 + 1, Ground14 = 25 + 1, Ground15 = 26 + 1,

        // Bridge tiles
        BridgeLeft = 27 + 1, BridgeMiddle = 28 + 1, BridgeRight = 29 + 1,

        // Spike tiles
        SpikeGround = 36 + 1, SpikeCeiling = 37 + 1, SpikeLeft = 38 + 1, SpikeRight = 39 + 1,

        // Chain tiles
        ChainBottom = 45 + 1, ChainTop = 46 + 1,

        // Misc tiles
        HollowTile = 30 + 1, FilledTile = 31 + 1,
        Coin = 48 + 1, // TODO: Move coin to objects
        GroundFilled = 54 + 1, GroundSpots = 55 + 1, Lava = 57 + 1
    }

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
        public Tile(Maths.Vector2 position, Maths.Vector2 size, ITexture texture, TileType type)
        {
            m_Position = position;
            m_Size = size;
            m_Texture = texture;

            m_HitboxPosition = GetHitboxPosition(position, type);
            m_HitboxSize = GetHitboxSize(size, type);
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
        // Private helper methods
        ////////////////////////////////////////////////////////////////////////////////////
        private Maths.Vector2 GetHitboxPosition(Maths.Vector2 position, TileType type)
        {
            switch (type)
            {
            case TileType.BridgeMiddle:
            case TileType.SpikeCeiling:
                return new Maths.Vector2(position.X, position.Y + (Settings.SpriteSize / 2));

            case TileType.SpikeRight:
                return new Maths.Vector2(position.X + (Settings.SpriteSize / 2), position.Y);

            case TileType.ChainTop:
            case TileType.ChainBottom:
                return new Maths.Vector2(position.X + (Settings.SpriteSize / 4), position.Y);

            default:
                break;
            }

            return position;
        }

        private Maths.Vector2 GetHitboxSize(Maths.Vector2 size, TileType type)
        {
            switch (type)
            {
            case TileType.BridgeMiddle:
            case TileType.SpikeGround:
            case TileType.SpikeCeiling:
                return new Maths.Vector2(size.X, (Settings.SpriteSize / 2));

            case TileType.SpikeLeft: // Left-side spikes
            case TileType.SpikeRight: // Right-side spikes
            case TileType.ChainTop: // Top chain
            case TileType.ChainBottom: // Bottom chain
                return new Maths.Vector2((Settings.SpriteSize / 2), size.Y);

            default:
                break;
            }

            return size;
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