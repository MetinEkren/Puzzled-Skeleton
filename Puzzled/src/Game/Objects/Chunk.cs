using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Diagnostics;

namespace Puzzled
{

    //////////////////////////////////////////////////////////////////////////////////
    // Chunk // Note: Tiles must be snapped to a grid
    //////////////////////////////////////////////////////////////////////////////////
    public class Chunk
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public Chunk(uint x, uint y, List<Tile> allTiles)
        {
            m_ChunkX = x;
            m_ChunkY = y;

            foreach (Tile tile in allTiles)
            {
                if (tile.Position.X >= m_ChunkX * (Settings.ChunkSize * Settings.SpriteSize) && tile.Position.X < ((m_ChunkX + 1) * (Settings.ChunkSize * Settings.SpriteSize)) &&
                    tile.Position.Y >= m_ChunkY * (Settings.ChunkSize * Settings.SpriteSize) && tile.Position.Y < ((m_ChunkY + 1) * (Settings.ChunkSize * Settings.SpriteSize)))
                {
                    int xIndex = (int)((tile.Position.X - (Settings.ChunkSize * Settings.SpriteSize * x)) / Settings.SpriteSize);
                    int yIndex = (int)((tile.Position.Y - (Settings.ChunkSize * Settings.SpriteSize * y)) / Settings.SpriteSize);
                    m_Tiles[xIndex, yIndex] = tile;
                }
            }
        }
        ~Chunk()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        public void RenderTo(Renderer renderer, bool debug = false)
        {
            //(void)debug;

            foreach (Tile tile in m_Tiles)
            {
                if (tile == null)
                    continue;
                
                tile.RenderTo(renderer, debug);
            }

            if (debug) // Outline chunk size
            {
                renderer.AddQuad(Position, new Maths.Vector2(Size.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(Position, new Maths.Vector2(1 * Settings.Scale, Size.Y), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(Position.X, Position.Y + Size.Y - (1 * Settings.Scale)), new Maths.Vector2(Size.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(Position.X + Size.X - (1 * Settings.Scale), Position.Y), new Maths.Vector2(1 * Settings.Scale, Size.Y), Assets.WhiteTexture);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private uint m_ChunkX, m_ChunkY;
        
        // TODO: Maybe make a dynamic list, better performance, since many tiles are null otherwise.
        private Tile[,] m_Tiles = new Tile[Settings.ChunkSize, Settings.ChunkSize]; // [x,y]

        public Tile[,] Tiles { get { return m_Tiles; } }

        public Maths.Vector2 Position { get { return new Maths.Vector2(m_ChunkX * (Settings.ChunkSize * Settings.SpriteSize), m_ChunkY * (Settings.ChunkSize * Settings.SpriteSize)); } }
        public Maths.Vector2 Size { get { return new Maths.Vector2((Settings.ChunkSize * Settings.SpriteSize), (Settings.ChunkSize * Settings.SpriteSize)); } }
    
    }

}