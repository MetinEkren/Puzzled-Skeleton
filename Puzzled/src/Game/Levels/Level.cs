using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;

namespace Puzzled
{

    //////////////////////////////////////////////////////////////////////////////////
    // Level
    //////////////////////////////////////////////////////////////////////////////////
    public class Level
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public Level(Canvas canvas, Renderer renderer, string levelPath)
        {
            m_Renderer = renderer;

            Load(levelPath);
        }
        ~Level()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        public void OnUpdate(float deltaTime)
        {
            m_Player.Update(deltaTime);
            m_Player.HandleStaticCollisions(m_Chunks);
        }

        public void OnRender()
        {
            foreach (KeyValuePair<(uint x, uint y), Chunk> chunk in m_Chunks)
                chunk.Value.RenderTo(m_Renderer, m_Debug);

            foreach (DynamicObject obj in m_DynamicObjects)
                obj.RenderTo(m_Renderer, m_Debug);
            
            m_Player.RenderTo(m_Renderer, m_Debug);
        }

        public void OnEvent(Event e)
        {
            if (e is KeyPressedEvent kpe)
            {
                // Note: For testing a debug
                if (kpe.KeyCode == Key.H)
                    m_Debug = !m_Debug;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Loading
        ////////////////////////////////////////////////////////////////////////////////////
        public void Load(string path)
        {
            Logger.Info($"Loading level from: {path}");

            m_Player = new Player();

            uint tilesX, tilesY;
            m_Tiles = new List<Tile>();
            
            LevelLoader.Load(path, ref m_Tiles, out tilesX, out tilesY);

            // Putting all tiles into chunks 
            {
                uint chunksX = (uint)Math.Ceiling((double)(tilesX / (float)Settings.ChunkSize));
                uint chunksY = (uint)Math.Ceiling((double)(tilesY / (float)Settings.ChunkSize));

                for (uint x = 0; x < chunksX; x++)
                {
                    for (uint y = 0; y < chunksY; y++)
                    {
                        m_Chunks.Add((x, y), new Chunk(x, y, m_Tiles));
                    }
                }
            }

            // Dynamic objects
            {
                Box box = new Box(new Maths.Vector2(48, 48));
                m_DynamicObjects.Add(box);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Renderer m_Renderer;
        private bool m_Debug = false;

        private Player m_Player;

        private List<Tile> m_Tiles; // Note: Contiguous list of all tiles, not used at the moment, but for level loading is useful
        private Dictionary<(uint x, uint y), Chunk> m_Chunks = new Dictionary<(uint x, uint y), Chunk>();
        private List<DynamicObject> m_DynamicObjects = new List<DynamicObject>();

    }

}