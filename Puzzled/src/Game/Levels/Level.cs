using Puzzled.Physics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Security.Policy;
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
            // TODO: Move a lot of this code to Physics.cs

            Player.Update(deltaTime);

            // Dynamic object update
            foreach (KeyValuePair<uint, DynamicObject> obj in DynamicObjects)
                obj.Value.Update(deltaTime);

            // Dynamic collisions between objects
            foreach (KeyValuePair<uint, DynamicObject> obj in DynamicObjects)
                HandleDynamicCollision(obj.Value);

            // Dynamic objects collision with static tiles (static collision)
            // Note: Only includes tiles with gravity, since others are static (in terms of position)
            {
                foreach (KeyValuePair<uint, DynamicObject> obj in DynamicObjects)
                {
                    if (obj.Value is Box box)
                    {
                        bool canJump = false;
                        HandleStaticCollisions(ref box.Position, ref box.Velocity, ref canJump, box.HitboxPosition, box.HitboxSize);
                    }
                    
                }
            }
    
            HandleDynamicCollisionPlayer();

            // Static
            HandleStaticCollisions(ref Player.Position, ref Player.Velocity, ref Player.CanJump, Player.HitboxPosition, Player.HitboxSize);
        }

        public void OnRender()
        {
            foreach (KeyValuePair<(uint x, uint y), Chunk> chunk in m_Chunks)
                chunk.Value.RenderTo(m_Renderer, m_Debug);

            foreach (KeyValuePair<uint, DynamicObject> obj in DynamicObjects)
                obj.Value.RenderTo(m_Renderer, m_Debug);

            Player.RenderTo(m_Renderer, m_Debug);
        }

        public void OnEvent(Event e)
        {
            if (e is KeyPressedEvent kpe)
            {
                // Note: For testing a debug
                if (kpe.KeyCode == Key.H)
                    m_Debug = !m_Debug;
                if (kpe.KeyCode == Key.R)
                    DynamicObjects.Clear();
            }

            if (e is MouseButtonPressedEvent)
            {
                DynamicObjects.Add((uint)new Random().Next(), (new Box(new Maths.Vector2(Input.GetMousePosition().X - (Settings.SpriteSize / 2), (Game.Instance.Window.Height - Input.GetMousePosition().Y - (Settings.SpriteSize / 2))))));
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Loading
        ////////////////////////////////////////////////////////////////////////////////////
        public void Load(string path)
        {
            Logger.Info($"Loading level from: {path}");

            Player = new Player();

            uint tilesX, tilesY;
            m_Tiles = new List<Tile>();

            LevelLoader.Load(path, ref m_Tiles, ref DynamicObjects, out tilesX, out tilesY);

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

            // Music
            if (Assets.WinMenuMusic.IsPlaying())
                Assets.WinMenuMusic.Stop();
            Assets.LevelMusic.Start();
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

        private bool HandleDynamicCollision(DynamicObject obj) // Collisions between dynamicobjects themself
        {
            bool hasCollided = false;
            foreach (KeyValuePair<uint, DynamicObject> obj2 in DynamicObjects)
            {
                if (obj == obj2.Value)
                    continue;

                if (obj is Box box)
                {
                    // Collision between boxes
                    if (obj2.Value is Box box2)
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
                    if (obj2.Value is Door door)
                    {
                        CollisionResult result = Collision.AABB(box.HitboxPosition, box.HitboxSize, door.HitboxPosition, door.HitboxSize);

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
                            // Top (Can never happen, always a tile above a door)
                            () => { },
                            // Bottom (Can never happen, always a tile below a door)
                            () => { }
                        );
                        hasCollided |= collision;

                        if (!collision)
                            continue;
                    }
                    // Note: We don't need button logic here, since it's below
                }
                else if (obj is Button button)
                {
                    // Collision between box and button
                    else if (obj2.Value is Button button)
                    {
                        CollisionResult result = Collision.AABB(button.HitboxPosition, button.HitboxSize, box.HitboxPosition, box.HitboxSize);
                        if (result.Side != CollisionSide.None)
                        {
                            button.Press();
                            hasCollided = true;
                        }
                    }
                }
                else if (obj is Bridge bridge)
                {
                    if (obj2.Value is Box box2)
                    {
                        CollisionResult result = Collision.AABB(box2.HitboxPosition, box2.HitboxSize, bridge.HitboxPosition, bridge.HitboxSize);
                        bool collision = HandleCollision(result,
                        // Left
                        () => { },
                        // Right
                        () => { },
                        // Top
                        () =>
                        {
                        },
                        // Bottom
                        () =>
                        {
                            if (box2.Velocity.Y < 0.0f)
                            {
                                box2.Position.Y += result.Overlap;
                                box2.Velocity.Y = 0.0f;
                            }
                        }
                        );
                    }
                }
            }

            return hasCollided;
        }

        private bool HandleDynamicCollisionPlayer() // Collisions between player and dynamic objects
        {
            bool hasCollided = false;

            // Dynamic
            foreach (KeyValuePair<uint, DynamicObject> obj in DynamicObjects)
            {
                if (obj.Value is Box box)
                {
                    CollisionResult result = Collision.AABB(box.HitboxPosition, box.HitboxSize, Player.HitboxPosition, Player.HitboxSize);

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
                            if (Player.Velocity.Y <= 0.0f)
                            {
                                Player.Velocity.Y = 0.0f;
                                Player.CanJump = true;
                            }
                        },
                        // Bottom
                        () =>
                        {
                            box.Position.Y += result.Overlap;

                            if (Player.Velocity.Y > 0.0f)
                            {
                                box.Velocity.X = (Player.Velocity.X / Settings.PlayerRunningVelocity) * Settings.BoxHitVelocity;
                                box.Velocity.Y = (Player.Velocity.Y / Settings.PlayerJumpingVelocity) * Settings.BoxHitVelocity;
                                //box.Velocity.X = Player.Velocity.X;
                                //box.Velocity.Y = Player.Velocity.Y;
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
                    // Make sure we are not in walls or the box in another dynamic object
                    collision = HandleDynamicCollision(box);
                    hasCollided |= collision;

                    bool canJump = false;
                    collision = HandleStaticCollisions(ref box.Position, ref box.Velocity, ref canJump, box.HitboxPosition, box.HitboxSize);
                    hasCollided |= collision;

                    // After colliding with player, resolving and then colliding with static blocks and resolving
                    // We need to check if we're now colliding with the player again, if so move the player
                    result = Collision.AABB(box.HitboxPosition, box.HitboxSize, Player.HitboxPosition, Player.HitboxSize);
                    collision = HandleCollision(result,
                        // Left
                        () =>
                        {
                            Player.Position.X -= result.Overlap;
                        },
                        // Right
                        () =>
                        {
                            Player.Position.X += result.Overlap;
                        },
                        // Top
                        () =>
                        {
                            Player.Position.Y += result.Overlap;
                        },
                        // Bottom
                        () =>
                        {
                            Player.Position.Y -= result.Overlap;
                        }
                    );
                    hasCollided |= collision;
                }
                else if (obj.Value is Button button)
                {
                    CollisionResult result = Collision.AABB(button.HitboxPosition, button.HitboxSize, Player.HitboxPosition, Player.HitboxSize);
                    if (result.Side != CollisionSide.None)
                    {
                        button.Press();
                        hasCollided = true;
                    }
                }
                else if (obj.Value is DoorKey doorkey)
                {
                    CollisionResult result = Collision.AABB(doorkey.HitboxPosition, doorkey.HitboxSize, Player.HitboxPosition, Player.HitboxSize);
                    if (result.Side != CollisionSide.None)
                    {
                        doorkey.Collect();
                        Player.HasKey = true;
                    }
                }
                else if (obj.Value is Door door)
                {

                    CollisionResult result = Collision.AABB(door.HitboxPosition, door.HitboxSize, Player.HitboxPosition, Player.HitboxSize);
                    bool collision = HandleCollision(result,
                        // Left
                        () =>
                        {
                            Player.Position.X -= result.Overlap;
                        },
                        // Right
                        () =>
                        {
                            Player.Position.X += result.Overlap;
                        },
                        // Top
                        () =>
                        {
                            Player.Position.Y += result.Overlap;
                        },
                        // Bottom
                        () =>
                        {
                            Player.Position.Y -= result.Overlap;
                        }
                    );
                    hasCollided |= collision;

                    if (collision && door.Type == DoorType.KeyDoor && Player.HasKey)
                    {
                        door.OpenForever();
                        Player.HasKey = false;
                    }
                }
                else if (obj.Value is Bridge bridge)
                {

                    CollisionResult result = Collision.AABB(Player.HitboxPosition, Player.HitboxSize, bridge.HitboxPosition, bridge.HitboxSize);
                    bool collision = HandleCollision(result,
                        // Left
                        () => { },
                        // Right
                        () => { },
                        // Top
                        () =>
                        {
                        },
                        // Bottom
                        () => 
                        {
                            if (Player.Velocity.Y < 0.0f)
                            {
                                Player.Position.Y += result.Overlap;
                                Player.Velocity.Y = 0.0f;
                                Player.CanJump = true;
                            }
                        }
                    );
                    hasCollided |= collision;
                else if (obj.Value is Spike spike)
                {
                    CollisionResult result = Collision.AABB(spike.HitboxPosition, spike.HitboxSize, Player.HitboxPosition, Player.HitboxSize);
                    if (result.Side != CollisionSide.None)
                    {
                        Player.Kill();
                    }
                }
                else if (obj.Value is Lava lava)
                {
                    CollisionResult result = Collision.AABB(lava.HitboxPosition, lava.HitboxSize, Player.HitboxPosition, Player.HitboxSize);
                    if (result.Side != CollisionSide.None)
                    {
                        Player.Kill();
                    }
                }
                else if (obj.Value is Ladder ladder)
                {
                    CollisionResult result = Collision.AABB(ladder.HitboxPosition, ladder.HitboxSize, Player.HitboxPosition, Player.HitboxSize);
                    if (result.Side != CollisionSide.None)
                    {
                        Player.IsClimbing = true;
                    }
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

        public Player Player;

        private List<Tile> m_Tiles; // Note: Contiguous list of all tiles, not used at the moment, but for level loading is useful
        private Dictionary<(uint x, uint y), Chunk> m_Chunks = new Dictionary<(uint x, uint y), Chunk>();
        public Dictionary<uint, DynamicObject> DynamicObjects = new Dictionary<uint, DynamicObject>();

    }

}