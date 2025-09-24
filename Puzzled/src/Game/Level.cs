using Puzzled.Physics;
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

            // Player collision
            {
                uint chunkBottomLeftX = (uint)Math.Floor(m_Player.Position.X / (Settings.SpriteSize * Settings.ChunkSize));
                uint chunkBottomLeftY = (uint)Math.Floor(m_Player.Position.Y / (Settings.SpriteSize * Settings.ChunkSize));

                // Note: Subtract a tiny epsilon (0.01f) to avoid rounding issues when the edge is exactly on a chunk boundary
                uint chunkTopRightX = (uint)Math.Floor((m_Player.Position.X + m_Player.Size.X - 0.01f) / (Settings.SpriteSize * Settings.ChunkSize));
                uint chunkTopRightY = (uint)Math.Floor((m_Player.Position.Y + m_Player.Size.Y - 0.01f) / (Settings.SpriteSize * Settings.ChunkSize));

                // Check collisions with chunks
                for (uint x = chunkBottomLeftX; x <= chunkTopRightX; x++)
                {
                    for (uint y = chunkBottomLeftY; y <= chunkTopRightY; y++)
                    {
                        if (!m_Chunks.ContainsKey((x, y)))
                            continue;

                        Chunk chunk = m_Chunks[(x, y)];

                        foreach (Tile tile in chunk.Tiles)
                        {
                            if (tile == null)
                                continue;

                            CollisionResult result = Collision.AABB(m_Player.HitboxPosition, m_Player.HitboxSize, tile.Position, tile.Size);

                            if (result.Side != CollisionSide.None)
                            {
                                switch (result.Side) // TODO: All sides
                                { 
                                //case CollisionSide.Left:
                                //    m_TESTTile.Position = new Maths.Vector2(m_TESTTile.Position.X + collision.Overlap, m_TESTTile.Position.Y);
                                //    break;
                                //
                                //case CollisionSide.Right:
                                //    m_TESTTile.Position = new Maths.Vector2(m_TESTTile.Position.X - collision.Overlap, m_TESTTile.Position.Y);
                                //    break;
                                //
                                //case CollisionSide.Top:
                                //    m_TESTTile.Position = new Maths.Vector2(m_TESTTile.Position.X, m_TESTTile.Position.Y - collision.Overlap);
                                //    break;
                                
                                case CollisionSide.Bottom:
                                    m_Player.Position = new Maths.Vector2(m_Player.Position.X, m_Player.Position.Y + result.Overlap);
                                    m_Player.Velocity = new Maths.Vector2(m_Player.Velocity.X, 0.0f);
                                    m_Player.CanJump = true;
                                    break;
                                
                                default:
                                    break;
                                }
                            }
                        }
                    }
                }
            }
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

            // TODO: Remove
            List<Tile> tiles = new List<Tile>();
            tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 0, 0.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.WhiteTexture));
            tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 1, 0.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.WhiteTexture));
            tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 2, 0.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.WhiteTexture));
            tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 3, 0.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.WhiteTexture));
            
            //tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 4, 0.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.WhiteTexture));
            //tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 5, 0.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.WhiteTexture));
            tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 6, 0.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.WhiteTexture));
            //tiles.Add(new Tile(new Maths.Vector2(Settings.SpriteSize * 7, 0.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.WhiteTexture));

            m_Chunks.Add((0, 0), new Chunk(0, 0, tiles));
            m_Chunks.Add((1, 0), new Chunk(1, 0, tiles));
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Renderer m_Renderer;
        private bool m_Debug = false;

        private Player m_Player;
        private Dictionary<(uint x, uint y), Chunk> m_Chunks = new Dictionary<(uint x, uint y), Chunk>();

        ////////////////////////////////////////////////////////////////////////////////////
        // Static variables
        ////////////////////////////////////////////////////////////////////////////////////
        private static Level s_CurrentLevel;

        public static Level Active { get { return s_CurrentLevel; } }

    }

}