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
        public Level(Canvas canvas, string levelPath)
        {
            s_CurrentLevel = this;
            m_Renderer = new Renderer(canvas);

            Load(levelPath);
        }
        ~Level()
        {
            s_CurrentLevel = null;
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
            m_Renderer.Begin();

            foreach (KeyValuePair<(uint x, uint y), Chunk> chunk in m_Chunks)
                chunk.Value.RenderTo(m_Renderer, m_Debug);
            
            m_Player.RenderTo(m_Renderer, m_Debug);
            
            m_Renderer.End();
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
            m_Tiles = LevelLoader.Load(path, out tilesX, out tilesY);

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
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Renderer m_Renderer;
        private bool m_Debug = false;

        private Player m_Player;
        private List<Tile> m_Tiles; // Note: Contiguous list of all tiles, not used at the moment, but for level loading is useful
        private Dictionary<(uint x, uint y), Chunk> m_Chunks = new Dictionary<(uint x, uint y), Chunk>();

        ////////////////////////////////////////////////////////////////////////////////////
        // Static variables
        ////////////////////////////////////////////////////////////////////////////////////
        private static Level s_CurrentLevel;

        public static Level Active { get { return s_CurrentLevel; } }

    }

}