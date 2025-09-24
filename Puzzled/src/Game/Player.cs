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
            //Logger.Trace($"Velocity {{ .x = {m_Velocity.X}, .y = {m_Velocity.Y} }}");

            // Movement
            {
                if ((Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.Up) || Input.IsKeyPressed(Key.Space)) && m_CanJump)
                {
                    m_Velocity.Y = Settings.PlayerJumpingVelocity;
                    m_CanJump = false;
                }
                if (Input.IsKeyPressed(Key.A) || Input.IsKeyPressed(Key.Left))
                {
                    m_Velocity.X = -Settings.PlayerRunningVelocity;
                    m_Flipped = true;
                }
                if (Input.IsKeyPressed(Key.D) || Input.IsKeyPressed(Key.Right))
                {
                    m_Velocity.X = Settings.PlayerRunningVelocity;
                    m_Flipped = false;
                }
            }
            
            // Velocity
            {
                m_Position.X += m_Velocity.X * deltaTime;
                m_Position.Y += m_Velocity.Y * deltaTime;
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
                if (m_Velocity.X != 0.0f)
                {
                    m_Velocity.X -= Settings.GroundFriction * Math.Sign(m_Velocity.X) * deltaTime;

                    // Snap to zero if it overshoots
                    if (Math.Abs(m_Velocity.X) < Settings.GroundFriction)
                        m_Velocity.X = 0.0f;
                }

                m_Velocity.Y -= Settings.Gravity * deltaTime;
                m_Velocity.Y = Math.Min(m_Velocity.Y, Settings.PlayerTerminalVelocity);

                // When falling (or jumping) you are no longer able to jump again
                if (m_Velocity.Y != 0.0f)
                    m_CanJump = false;
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
            Logger.Trace($"Starting chunk checks, chunkBottomLeftX = {chunkBottomLeftX}, chunkBottomLeftY = {chunkBottomLeftY}, chunkTopRightX = {chunkTopRightX}, chunkTopRightY = {chunkTopRightY}");
            for (uint x = chunkBottomLeftX; x <= chunkTopRightX; x++)
            {
                for (uint y = chunkBottomLeftY; y <= chunkTopRightY; y++)
                {
                    if (!chunks.ContainsKey((x, y)))
                    {
                        Logger.Warn($"Player is outside of any chunks. Position = {{ .x = { Position.X }, .y = {Position.Y} }}");
                        continue;
                    }

                    Chunk chunk = chunks[(x, y)];

                    foreach (Tile tile in chunk.Tiles)
                    {
                        if (tile == null)
                            continue;

                        CollisionResult result = Collision.AABB(HitboxPosition, HitboxSize, tile.Position, tile.Size);

                        switch (result.Side)
                        {
                        case CollisionSide.Left:
                            m_Position = new Maths.Vector2(m_Position.X + result.Overlap, m_Position.Y);
                            break;
                        case CollisionSide.Right:
                            m_Position = new Maths.Vector2(m_Position.X - result.Overlap, m_Position.Y);
                            break;
                        case CollisionSide.Top:
                            m_Position = new Maths.Vector2(m_Position.X, m_Position.Y - result.Overlap);
                            break;
                        case CollisionSide.Bottom:
                            m_Position = new Maths.Vector2(m_Position.X, m_Position.Y + result.Overlap);
                            m_Velocity = new Maths.Vector2(m_Velocity.X, 0.0f);
                            m_CanJump = true;
                            break;

                        default:
                            break;
                        }
                    }
                }
            }
            Logger.Trace("Ending chunk checks");
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
        // Private getters // TODO: Check if this is the proper way to do this (since yk gravity)
        ////////////////////////////////////////////////////////////////////////////////////
        public bool IsMovingUp() { return m_Velocity.Y > 0.0f; }
        public bool IsMovingDown() { return m_Velocity.Y < 0.0f; }
        public bool IsMovingVertically() { return IsMovingUp() || IsMovingDown(); }
        public bool IsTryingToMoveLeft() { return m_Velocity.X < 0.0f; }
        public bool IsTryingToMoveRight() { return m_Velocity.X > 0.0f; }
        public bool IsMovingHorizontally() { return IsTryingToMoveLeft() || IsTryingToMoveRight(); }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private State m_State = State.Idle;
        private Maths.Vector2 m_Position = new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize);
        private Maths.Vector2 m_HitboxSize = new Maths.Vector2(Settings.SpriteSize - (2 * Settings.Scale) - (2 * Settings.Scale), Settings.SpriteSize - (3 * Settings.Scale));
        private Maths.Vector2 m_Velocity = new Maths.Vector2(0.0f, 0.0f);
        private bool m_Flipped = false;
        private bool m_CanJump = true;

        private Animation m_IdleAnimation = new Animation(Assets.IdleSheet, 16, 0.4f);
        private Animation m_RunningAnimation = new Animation(Assets.RunSheet, 16, 0.15f);
        // TODO: More animations

        public Maths.Vector2 Position { get { return m_Position; } set { m_Position = value; } }
        public Maths.Vector2 Size { get { return new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize); } }
        public Maths.Vector2 Velocity { get { return m_Velocity; } set { m_Velocity = value; } }

        public Maths.Vector2 HitboxPosition { get { return new Maths.Vector2(m_Position.X + (2 * Settings.Scale), m_Position.Y); } }
        public Maths.Vector2 HitboxSize { get { return m_HitboxSize; } }

    }

}