using Puzzled.Physics;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Puzzled
{

    //////////////////////////////////////////////////////////////////////////////////
    // Player
    //////////////////////////////////////////////////////////////////////////////////
    public class Player
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // State
        ////////////////////////////////////////////////////////////////////////////////////
        public enum State
        {
            Idle = 0,
            Running,
            // TODO: More
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public Player()
        {
        }
        ~Player()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        public void Update(float deltaTime)
        {
            //Logger.Trace($"Position {{ .x = {Position.X}, .y = {Position.Y} }}");
            //Logger.Trace($"Velocity {{ .x = {Velocity.X}, .y = {Velocity.Y} }}");

            // Movement
            {
                if ((Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.Up) || Input.IsKeyPressed(Key.Space)) && CanJump)
                {
                    // TODO: Jump cooldown, to prevent super fast jumping up blocks
                    Velocity.Y = Settings.PlayerJumpingVelocity;
                    CanJump = false;
                }
                if (Input.IsKeyPressed(Key.A) || Input.IsKeyPressed(Key.Left))
                {
                    Velocity.X = -Settings.PlayerRunningVelocity;
                    m_Flipped = true;
                }
                if (Input.IsKeyPressed(Key.D) || Input.IsKeyPressed(Key.Right))
                {
                    Velocity.X = Settings.PlayerRunningVelocity;
                    m_Flipped = false;
                }
            }
            
            // Velocity
            {
                Position.X += Velocity.X * deltaTime;
                Position.Y += Velocity.Y * deltaTime;
            }

            // Animation
            {
                if (IsMovingHorizontally() && m_State != State.Running)
                    SetNewState(State.Running);
                if (!IsMovingHorizontally() && m_State != State.Idle) // TODO: Something with !IsMovingVertically()
                    SetNewState(State.Idle);
            }

            // Friction & Gravity
            {
                if (Velocity.X != 0.0f)
                {
                    Velocity.X -= Settings.GroundFriction * Math.Sign(Velocity.X) * deltaTime;

                    // Snap to zero if it overshoots
                    if (Math.Abs(Velocity.X) < Settings.GroundFriction)
                        Velocity.X = 0.0f;
                }

                Velocity.Y -= Settings.Gravity * deltaTime;
                Velocity.Y = Math.Min(Velocity.Y, Settings.PlayerTerminalVelocity);

                // When falling (or jumping) you are no longer able to jump again
                if (Velocity.Y != 0.0f)
                    CanJump = false;
            }

            GetCurrentAnimation().Update(deltaTime);
        }

        public void HandleStaticCollisions(Dictionary<(uint x, uint y), Chunk> chunks)
        {
            uint chunkBottomLeftX = (uint)Math.Floor(HitboxPosition.X / (Settings.SpriteSize * Settings.ChunkSize));
            uint chunkBottomLeftY = (uint)Math.Floor(HitboxPosition.Y / (Settings.SpriteSize * Settings.ChunkSize));

            // Note: Subtract a tiny epsilon (0.01f) to avoid rounding issues when the edge is exactly on a chunk boundary
            uint chunkTopRightX = (uint)Math.Floor((HitboxPosition.X + HitboxSize.X - 0.01f) / (Settings.SpriteSize * Settings.ChunkSize));
            uint chunkTopRightY = (uint)Math.Floor((HitboxPosition.Y + HitboxSize.Y - 0.01f) / (Settings.SpriteSize * Settings.ChunkSize));

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
                    if (!chunks.ContainsKey((x, y)))
                    {
                        Logger.Warn($"Trying to check for chunk ({x}, {y}), it doesn't exist. Position = {{ .x = { Position.X }, .y = {Position.Y} }}");
                        continue;
                    }

                    Chunk chunk = chunks[(x, y)];

                    foreach (Tile tile in chunk.Tiles)
                    {
                        if (tile == null)
                            continue;

                        CollisionResult result = Collision.AABB(HitboxPosition, HitboxSize, tile.HitboxPosition, tile.HitboxSize);

                        switch (result.Side) // TODO: Fix collision bug with side jumping
                        {
                        case CollisionSide.Left:
                            Position = new Maths.Vector2(Position.X + result.Overlap, Position.Y);
                            Velocity = new Maths.Vector2(0.0f, Velocity.Y);
                            break;
                        case CollisionSide.Right:
                            Position = new Maths.Vector2(Position.X - result.Overlap, Position.Y);
                            Velocity = new Maths.Vector2(0.0f, Velocity.Y);
                            break;
                        case CollisionSide.Top:
                            Position = new Maths.Vector2(Position.X, Position.Y - result.Overlap);

                            if (Velocity.Y > 0.0f)
                                Velocity = new Maths.Vector2(Velocity.X, 0.0f);
                            break;
                        case CollisionSide.Bottom:
                            Position = new Maths.Vector2(Position.X, Position.Y + result.Overlap);

                            if (Velocity.Y < 0.0f)
                            {
                                Velocity = new Maths.Vector2(Velocity.X, 0.0f);
                                CanJump = true;
                            }

                            // Note: We don't set CanJump while the player is still jumping
                            
                            break;

                        default:
                            break;
                        }
                    }
                }
            }
            //Logger.Trace("Ending chunk checks");
        }

        public void RenderTo(Renderer renderer, bool debug = false)
        {
            renderer.AddQuad(Position, Size, GetCurrentAnimation().GetCurrentTexture(), m_Flipped);
            
            if (debug) // Outline hitbox
            {
                renderer.AddQuad(HitboxPosition, new Maths.Vector2(HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(HitboxPosition, new Maths.Vector2(1 * Settings.Scale, HitboxSize.Y), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(HitboxPosition.X, HitboxPosition.Y + HitboxSize.Y - (1 * Settings.Scale)), new Maths.Vector2(HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(HitboxPosition.X + HitboxSize.X - (1 * Settings.Scale), HitboxPosition.Y), new Maths.Vector2(1 * Settings.Scale, HitboxSize.Y), Assets.WhiteTexture);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Private methods
        ////////////////////////////////////////////////////////////////////////////////////
        public void SetNewState(State state)
        {
            m_State = state;
            GetCurrentAnimation().Reset();
        }
        
        public Animation GetCurrentAnimation()
        {
            switch (m_State)
            {
            case State.Idle:            return m_IdleAnimation;
            case State.Running:         return m_RunningAnimation;
            
            default:
                break;
            }

            return m_IdleAnimation;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Private getters // FUTURE TODO: Check if this is the proper way to do this (since yk gravity)
        ////////////////////////////////////////////////////////////////////////////////////
        public bool IsMovingUp() { return Velocity.Y > 0.0f; }
        public bool IsMovingDown() { return Velocity.Y < 0.0f; }
        public bool IsMovingVertically() { return IsMovingUp() || IsMovingDown(); }
        public bool IsTryingToMoveLeft() { return Velocity.X < 0.0f; }
        public bool IsTryingToMoveRight() { return Velocity.X > 0.0f; }
        public bool IsMovingHorizontally() { return IsTryingToMoveLeft() || IsTryingToMoveRight(); }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private State m_State = State.Idle;
        private Maths.Vector2 m_HitboxSize = new Maths.Vector2(Settings.SpriteSize - (2 * Settings.Scale) - (2 * Settings.Scale), Settings.SpriteSize - (3 * Settings.Scale));       
        private bool m_Flipped = false;
        public Maths.Vector2 Position = new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize);
        public Maths.Vector2 Velocity = new Maths.Vector2(0.0f, 0.0f);
        public bool CanJump = true;

        private Animation m_IdleAnimation = new Animation(Assets.IdleSheet, (Settings.SpriteSize / Settings.Scale), Settings.IdleAdvanceTime);
        private Animation m_RunningAnimation = new Animation(Assets.RunSheet, (Settings.SpriteSize / Settings.Scale), Settings.RunAdvanceTime);
        // TODO: More animations

        public Maths.Vector2 Size { get { return new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize); } }

        public Maths.Vector2 HitboxPosition { get { return new Maths.Vector2(Position.X + (2 * Settings.Scale), Position.Y); } }
        public Maths.Vector2 HitboxSize { get { return m_HitboxSize; } }

    }

}