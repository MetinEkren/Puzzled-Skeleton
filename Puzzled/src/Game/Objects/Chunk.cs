using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using System.Collections.Generic;

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
                if (tile.Position.X >= m_ChunkX * (Settings.ChunkSize * Settings.SpriteSize) && tile.Position.X < (m_ChunkX + 1) * (Settings.ChunkSize * Settings.SpriteSize) &&
                    tile.Position.Y >= m_ChunkY * (Settings.ChunkSize * Settings.SpriteSize) && tile.Position.Y < (m_ChunkY + 1) * (Settings.ChunkSize * Settings.SpriteSize))
                {
                    m_Tiles[(int)(tile.Position.X / Settings.SpriteSize / (x + 1)), (int)(tile.Position.Y / Settings.SpriteSize / (y + 1))] = tile;
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

                renderer.AddQuad(tile.Position, tile.Size, tile.Texture);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private uint m_ChunkX, m_ChunkY;
        private Tile[,] m_Tiles = new Tile[Settings.ChunkSize, Settings.ChunkSize]; // [x,y]

        public Tile[,] Tiles { get { return m_Tiles; } }
    
    }

}