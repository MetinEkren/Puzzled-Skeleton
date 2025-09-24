using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using System.Windows.Input;

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
            Logger.Trace($"Position {{ .x = {Position.X}, .y = {Position.Y} }}");
            Logger.Trace($"Velocity {{ .x = {m_Velocity.X}, .y = {m_Velocity.Y} }}");

            // TODO: Fix diagonal movement being 1.4x higher than horizontal and vertical
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
            
            // Velocity
            {
                m_Position.X += m_Velocity.X * deltaTime;
                m_Position.Y += m_Velocity.Y * deltaTime;
            }

            if (IsMovingHorizontally() && m_State != State.Running)
                SetNewState(State.Running);
            if (!IsMovingHorizontally() && m_State != State.Idle) // TODO: Something with !IsMovingVertically()
                SetNewState(State.Idle);

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
            }

            GetCurrentAnimation().Update(deltaTime);
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

        public bool CanJump { get { return m_CanJump; } set { m_CanJump = value; } }

    }

}