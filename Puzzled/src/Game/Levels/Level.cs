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

using Puzzled.Physics;

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
            Logger.Trace($"Box count: {m_DynamicObjects.Count}");
            Logger.Trace($"FPS: { 1/ deltaTime }");

            // TODO: Move a lot of this code to Physics.cs
            
            // Player update
            m_Player.Update(deltaTime);

            // Dynamic object update
            foreach (DynamicObject obj in m_DynamicObjects)
                obj.Update(deltaTime);

            // Dynamic collisions between objects
            foreach (DynamicObject obj in m_DynamicObjects)
                HandleDynamicCollision(obj);

            // Dynamic objects collision with static tiles (static collision)
            // Note: Only includes tiles with gravity, since others are static (in terms of position)
            {
                foreach (DynamicObject obj in m_DynamicObjects)
                {
                    if (obj is Box box)
                    {
                        bool canJump = false;
                        HandleStaticCollisions(ref box.Position, ref box.Velocity, ref canJump, box.HitboxPosition, box.HitboxSize);
                    }
                    
                }
            }

            // Player collision (static & dynamic)
            {
                // Dynamic
                HandleDynamicCollisionPlayer();

                // Static
                HandleStaticCollisions(ref m_Player.Position, ref m_Player.Velocity, ref m_Player.CanJump, m_Player.HitboxPosition, m_Player.HitboxSize);
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
            if (e is MouseButtonPressedEvent mbpe)
            {
                //for (int i = 0; i < 10000; i++)
                    m_DynamicObjects.Add(new Box(new Maths.Vector2(Input.GetMousePosition().X - (Settings.SpriteSize / 2), (Game.Instance.Window.Height - Input.GetMousePosition().Y - (Settings.SpriteSize / 2)))));
            }

            if (e is KeyPressedEvent kpe)
            {
                // Note: For testing a debug
                if (kpe.KeyCode == Key.H)
                    m_Debug = !m_Debug;

                if (kpe.KeyCode == Key.R)
                {
                    if (m_DynamicObjects.Count > 0)
                    {
                        m_DynamicObjects.Clear();
                    }
                    else
                    {
                        m_DynamicObjects.Add(new Button(new Maths.Vector2(96, 48)));
                        m_DynamicObjects.Add(new Door(new Maths.Vector2(384, 624)));
                    }
                }
                if (kpe.KeyCode == Key.P)
                {
                    Logger.Trace("");

                    Logger.Trace($"Player Position {{ .x = {m_Player.Position.X}, .y = {m_Player.Position.Y} }}");
                    Logger.Trace($"Player Velocity {{ .x = {m_Player.Velocity.X}, .y = {m_Player.Velocity.Y} }}");
                    
                    Logger.Trace("");
                }
                if (kpe.KeyCode == Key.L)
                {
                    Settings.LimitDeltaTime = !Settings.LimitDeltaTime;
                }
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
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Private methods
        ////////////////////////////////////////////////////////////////////////////////////
        public bool HandleStaticCollisions(ref Maths.Vector2 position, ref Maths.Vector2 velocity, ref bool canJump, Maths.Vector2 hitboxPosition, Maths.Vector2 hitboxSize)
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
            bool hasCollided = false;
            while (true)
            {
                bool collision = false;
                for (uint x = chunkBottomLeftX; x <= chunkTopRightX; x++)
                {
                    for (uint y = chunkBottomLeftY; y <= chunkTopRightY; y++)
                    {
                        if (!m_Chunks.ContainsKey((x, y)))
                            continue;

                        Chunk chunk = m_Chunks[(x, y)];
                        collision = HandleChunkStaticCollision(chunk, ref position, ref velocity, ref canJump, ref hitboxPosition, hitboxSize);
                        hasCollided |= collision;
                    }
                }

                if (!collision)
                    break;
            }

            return hasCollided;
        }

        private bool HandleDynamicCollision(DynamicObject obj)
        {
            bool hasCollided = false;
            foreach (DynamicObject obj2 in m_DynamicObjects)
            {
                if (obj == obj2)
                    continue;

                // Collision between boxes
                if (obj is Box box)
                {
                    if (obj2 is Box box2)
                    {
                        CollisionResult result = Collision.AABB(box.HitboxPosition, box.HitboxSize, box2.HitboxPosition, box2.HitboxSize);

                        bool collision = HandleCollision(result,
                            // Left
                            () =>
                            {
                                box2.Position.X -= result.Overlap;
                            },
                            // Right
                            () =>
                            {
                                box2.Position.X += result.Overlap;
                            },
                            // Top
                            () =>
                            {
                                box2.Position.Y += result.Overlap;
                                box.Velocity.Y = 0.0f;
                                box2.Velocity.Y = 0.0f;
                            },
                            // Bottom
                            () =>
                            {
                                box2.Position.Y -= result.Overlap;
                                box2.Velocity.Y = 0.0f;
                                box.Velocity.Y = 0.0f;
                            }
                        );
                        hasCollided |= collision;

                        if (!collision)
                            continue;

                        // A collision has occurred and been resolved
                        // Make sure we are not in walls
                        bool canJump = false;
                        collision = HandleStaticCollisions(ref box.Position, ref box.Velocity, ref canJump, box2.HitboxPosition, box2.HitboxSize);
                        hasCollided |= collision;

                        // After colliding with player, resolving and then colliding with static blocks and resolving
                        // We need to check if we're now colliding with the player again, if so move the player
                        result = Collision.AABB(box.HitboxPosition, box.HitboxSize, box2.HitboxPosition, box2.HitboxSize);
                        collision = HandleCollision(result,
                            // Left
                            () =>
                            {
                                box.Position.X -= result.Overlap;
                            },
                            // Right
                            () =>
                            {
                                box.Position.X += result.Overlap;
                            },
                            // Top
                            () =>
                            {
                                box.Position.Y += result.Overlap;
                            },
                            // Bottom
                            () =>
                            {
                                box.Position.Y -= result.Overlap;
                            }
                        );
                        hasCollided |= collision;

                        // Note: We currently don't do anything if we collide again, which is fine I think
                    }

                    // Note: We don't need button logic here, since it's below
                }
                else if (obj is Button button)
                {
                    if (obj2 is Box box2)
                    {
                        CollisionResult result = Collision.AABB(button.HitboxPosition, button.HitboxSize, box2.HitboxPosition, box2.HitboxSize);
                        if (result.Side != CollisionSide.None)
                        {
                            hasCollided = true;
                            button.Press();
                        }
                    }
                }
            }

            return hasCollided;
        }

        private bool HandleDynamicCollisionPlayer()
        {
            bool hasCollided = false;

            // Dynamic
            foreach (DynamicObject obj in m_DynamicObjects)
            {
                if (obj is Box box)
                {
                    CollisionResult result = Collision.AABB(box.HitboxPosition, box.HitboxSize, m_Player.HitboxPosition, m_Player.HitboxSize);

                    bool collision = HandleCollision(result,
                        // Left
                        () =>
                        {
                            box.Position.X += result.Overlap;
                        },
                        // Right
                        () =>
                        {
                            box.Position.X -= result.Overlap;
                        },
                        // Top
                        () =>
                        {
                            if (m_Player.Velocity.Y <= 0.0f)
                            {
                                m_Player.Velocity.Y = 0.0f;
                                m_Player.CanJump = true;
                            }
                        },
                        // Bottom
                        () =>
                        {
                            box.Position.Y += result.Overlap;

                            if (m_Player.Velocity.Y > 0.0f)
                            {
                                box.Velocity.X = (m_Player.Velocity.X / Settings.PlayerRunningVelocity) * Settings.BoxHitVelocity;
                                box.Velocity.Y = (m_Player.Velocity.Y / Settings.PlayerJumpingVelocity) * Settings.BoxHitVelocity;
                                //box.Velocity.X = m_Player.Velocity.X;
                                //box.Velocity.Y = m_Player.Velocity.Y;
                            }
                            else
                            {
                                box.Velocity.Y = 0.0f;
                            }
                        }
                    );
                    hasCollided |= collision;

                    if (!collision)
                        continue;

                    // A collision has occurred and been resolved
                    // Make sure we are not in walls
                    bool canJump = false;
                    collision = HandleStaticCollisions(ref box.Position, ref box.Velocity, ref canJump, box.HitboxPosition, box.HitboxSize);
                    hasCollided |= collision;

                    // After colliding with player, resolving and then colliding with static blocks and resolving
                    // We need to check if we're now colliding with the player again, if so move the player
                    result = Collision.AABB(box.HitboxPosition, box.HitboxSize, m_Player.HitboxPosition, m_Player.HitboxSize);
                    collision = HandleCollision(result,
                        // Left
                        () =>
                        {
                            m_Player.Position.X -= result.Overlap;
                        },
                        // Right
                        () =>
                        {
                            m_Player.Position.X += result.Overlap;
                        },
                        // Top
                        () =>
                        {
                            m_Player.Position.Y += result.Overlap;
                        },
                        // Bottom
                        () =>
                        {
                            m_Player.Position.Y -= result.Overlap;
                        }
                    );
                    hasCollided |= collision;
                }
                else if (obj is Button button)
                {
                    CollisionResult result = Collision.AABB(button.HitboxPosition, button.HitboxSize, m_Player.HitboxPosition, m_Player.HitboxSize);
                    if (result.Side != CollisionSide.None)
                    {
                        button.Press();
                        hasCollided = true;
                    }
                }
                else if (obj is Door door)
                {
                    CollisionResult result = Collision.AABB(door.HitboxPosition, door.HitboxSize, m_Player.HitboxPosition, m_Player.HitboxSize);
                    bool collision = HandleCollision(result,
                        // Left
                        () =>
                        {
                            m_Player.Position.X -= result.Overlap;
                        },
                        // Right
                        () =>
                        {
                            m_Player.Position.X += result.Overlap;
                        },
                        // Top
                        () =>
                        {
                            m_Player.Position.Y += result.Overlap;
                        },
                        // Bottom
                        () =>
                        {
                            m_Player.Position.Y -= result.Overlap;
                        }
                    );
                    hasCollided |= collision;
                }
            }

            return hasCollided;
        }

        private bool HandleChunkStaticCollision(Chunk chunk, ref Maths.Vector2 position, ref Maths.Vector2 velocity, ref bool canJump, ref Maths.Vector2 hitboxPosition, Maths.Vector2 hitboxSize)
        {
            foreach (Tile tile in chunk.Tiles)
            {
                if (tile == null)
                    continue;

                if (HandleColliderStaticCollision(ref position, ref velocity, ref canJump, ref hitboxPosition, hitboxSize, tile.HitboxPosition, tile.HitboxSize))
                    return true;
            }

            return false;
        }

        private bool HandleColliderStaticCollision(ref Maths.Vector2 position, ref Maths.Vector2 velocity, ref bool canJump, ref Maths.Vector2 hitboxPosition, Maths.Vector2 hitboxSize, Maths.Vector2 colliderPosition, Maths.Vector2 colliderSize)
        {
            CollisionResult result = Collision.AABB(hitboxPosition, hitboxSize, colliderPosition, colliderSize);

            switch (result.Side) // TODO: Fix collision bug with side jumping
            {
                case CollisionSide.Left:
                    position.X += result.Overlap;
                    hitboxPosition.X += result.Overlap;
                    velocity = new Maths.Vector2(0.0f, velocity.Y);
                    return true;
                case CollisionSide.Right:
                    position.X -= result.Overlap;
                    hitboxPosition.X -= result.Overlap;
                    velocity = new Maths.Vector2(0.0f, velocity.Y);
                    return true;
                case CollisionSide.Top:
                    position.Y -= result.Overlap;
                    hitboxPosition.Y -= result.Overlap;

                    if (velocity.Y > 0.0f)
                        velocity = new Maths.Vector2(velocity.X, 0.0f);
                    return true;
                case CollisionSide.Bottom:
                    position.Y += result.Overlap;
                    hitboxPosition.Y += result.Overlap;

                    if (velocity.Y <= 0.0f) // TODO: Fix tripping bug    
                    {
                        velocity = new Maths.Vector2(velocity.X, 0.0f);
                        canJump = true;
                    }

                    // Note: We don't set CanJump while the player is still jumping

                    return true;

                default:
                    break;
            }

            return false;
        }

        private bool HandleCollision(CollisionResult result, Action left, Action right, Action top, Action bottom)
        {
            switch (result.Side) // TODO: Fix collision bug with side jumping
            {
            case CollisionSide.Left:
                left.Invoke();
                return true;
            case CollisionSide.Right:
                right.Invoke();
                return true;
            case CollisionSide.Top:
                top.Invoke();
                return true;
            case CollisionSide.Bottom:
                bottom.Invoke();
                return true;

            default:
                break;
            }

            return false;
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