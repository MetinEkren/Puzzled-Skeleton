using Puzzled.Physics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

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

            // Player collision (static & dynamic)
            {
                HandleStaticCollisions(ref m_Player.Position, ref m_Player.Velocity, ref m_Player.CanJump, m_Player.HitboxPosition, m_Player.HitboxSize);
                
                foreach(DynamicObject obj in m_DynamicObjects)
                {
                    if(obj is Box box)
                    {
                        CollisionResult result = Collision.AABB(box.HitboxPosition, box.HitboxSize, m_Player.HitboxPosition, m_Player.HitboxSize);
                        switch(result.Side)
                        {
                            case CollisionSide.Left:
                                {
                                    box.Position.X += result.Overlap;
                                    break;
                                }
                            case CollisionSide.Right:
                                {
                                    box.Position.X -= result.Overlap;
                                    break;
                                }
                            case CollisionSide.Top:
                                {
                                    box.Position.Y -= result.Overlap;
                                    break;
                                }
                            case CollisionSide.Bottom:
                                {
                                    box.Position.Y += result.Overlap;
                                    break;
                                }
                        }
                    }
                }
            }

            // Dynamic objects (static collision)
            {
                foreach (DynamicObject obj in m_DynamicObjects)
                {
                    obj.Update(deltaTime);

                    if (obj is Box box)
                    {
                        bool canJump = false;
                        HandleStaticCollisions(ref box.Position, ref box.Velocity, ref canJump, box.HitboxPosition, box.HitboxSize);
                    }
                }
            }
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
                Box box = new Box(new Maths.Vector2(100, 300));
                m_DynamicObjects.Add(box);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Private methods
        ////////////////////////////////////////////////////////////////////////////////////
        public void HandleStaticCollisions(ref Maths.Vector2 position, ref Maths.Vector2 velocity, ref bool canJump, Maths.Vector2 hitboxPosition, Maths.Vector2 hitboxSize)
        {
            uint chunkBottomLeftX = (uint)Math.Floor(hitboxPosition.X / (Settings.SpriteSize * Settings.ChunkSize));
            uint chunkBottomLeftY = (uint)Math.Floor(hitboxPosition.Y / (Settings.SpriteSize * Settings.ChunkSize));

            // Note: Subtract a tiny epsilon (0.01f) to avoid rounding issues when the edge is exactly on a chunk boundary
            uint chunkTopRightX = (uint)Math.Floor((hitboxPosition.X + hitboxSize.X - 0.01f) / (Settings.SpriteSize * Settings.ChunkSize));
            uint chunkTopRightY = (uint)Math.Floor((hitboxPosition.Y + hitboxSize.Y - 0.01f) / (Settings.SpriteSize * Settings.ChunkSize));

            // Fix collisions no longer working when moving out of chunk coordinate space
            // + Fix chunks out of coordinate space being checked, instead set them to 0,0 (useless check, but easiest solution)
            {
                if (chunkBottomLeftX > Settings.MaxChunks) { chunkBottomLeftX = 0; chunkTopRightX = 0; }
                if (chunkBottomLeftY > Settings.MaxChunks) { chunkBottomLeftY = 0; chunkTopRightY = 0; }

                //if (chunkBottomLeftX > chunkTopRightX) chunkBottomLeftX = chunkTopRightX;
                //if (chunkBottomLeftY > chunkTopRightY) chunkBottomLeftY = chunkTopRightY;
            }

            // Check collisions with chunks
            //Logger.Trace($"Starting chunk checks, chunkBottomLeftX = {chunkBottomLeftX}, chunkBottomLeftY = {chunkBottomLeftY}, chunkTopRightX = {chunkTopRightX}, chunkTopRightY = {chunkTopRightY}");
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

                        CollisionResult result = Collision.AABB(hitboxPosition, hitboxSize, tile.HitboxPosition, tile.HitboxSize);

                        switch (result.Side) // TODO: Fix collision bug with side jumping
                        {
                        case CollisionSide.Left:
                            position = new Maths.Vector2(position.X + result.Overlap, position.Y);
                            velocity = new Maths.Vector2(0.0f, velocity.Y);
                            break;
                        case CollisionSide.Right:
                            position = new Maths.Vector2(position.X - result.Overlap, position.Y);
                                velocity = new Maths.Vector2(0.0f, velocity.Y);
                            break;
                        case CollisionSide.Top:
                            position = new Maths.Vector2(position.X, position.Y - result.Overlap);

                            if (velocity.Y > 0.0f)
                                velocity = new Maths.Vector2(velocity.X, 0.0f);
                            break;
                        case CollisionSide.Bottom:
                            position = new Maths.Vector2(position.X, position.Y + result.Overlap);

                            if (velocity.Y < 0.0f)
                            {
                                velocity = new Maths.Vector2(velocity.X, 0.0f);
                                canJump = true;
                            }

                            // Note: We don't set CanJump while the player is still jumping

                            break;

                        default:
                            break;
                        }
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
        private List<DynamicObject> m_DynamicObjects = new List<DynamicObject>();

    }

}