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
            m_Tiles = new List<Tile>();

            // Loading // TODO: Remove and replace with loading from file
            {
                m_Tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 0, 0.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.LeftBlock));
                m_Tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 1, 0.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.MiddleBlock));
                m_Tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 2, 0.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.MiddleBlock));
                m_Tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 3, 0.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.MiddleBlock));

                m_Tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 4, 0.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.MiddleBlock));
                m_Tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 5, 0.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.MiddleBlock));
                m_Tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 6, 0.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.MiddleBlock));
                m_Tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 7, 0.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.MiddleBlock));

                m_Tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 8, 0.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.RightBlock));
                m_Tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 9, Settings.SpriteSize * 1), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.SingleBlock));
                m_Tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 10, Settings.SpriteSize * 2), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.SingleBlock));
            }

            // Putting all tiles into chunks // TODO: Proper chunk management
            {
                m_Chunks.Add((0, 0), new Chunk(0, 0, m_Tiles));
                m_Chunks.Add((1, 0), new Chunk(1, 0, m_Tiles));
                m_Chunks.Add((2, 0), new Chunk(2, 0, m_Tiles));
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