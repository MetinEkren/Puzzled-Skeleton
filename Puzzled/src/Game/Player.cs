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
            if (Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.Up))
            {
                m_Velocity.Y = c_RunningVelocity;
            }
            if (Input.IsKeyPressed(Key.S) || Input.IsKeyPressed(Key.Down))
            {
                m_Velocity.Y = -c_RunningVelocity;
            }
            if (Input.IsKeyPressed(Key.A) || Input.IsKeyPressed(Key.Left))
            {
                m_Velocity.X = -c_RunningVelocity;
                m_Flipped = true;
            }
            if (Input.IsKeyPressed(Key.D) || Input.IsKeyPressed(Key.Right))
            {
                m_Velocity.X = c_RunningVelocity;
                m_Flipped = false;
            }

            m_Position.X += m_Velocity.X * deltaTime;
            m_Position.Y += m_Velocity.Y * deltaTime;

            if (IsMovingVertically() && m_State != State.Running)
                SetNewState(State.Running);
            if (!IsMovingVertically() && m_State != State.Idle) // TODO: Something with !IsMovingHorizontally()
                SetNewState(State.Idle);

            // Apply friction on X
            if (m_Velocity.X != 0.0f)
            {
                m_Velocity.X -= Settings.GroundFriction * Math.Sign(m_Velocity.X);

                // Snap to zero if it overshoots
                if (Math.Abs(m_Velocity.X) < Settings.GroundFriction)
                    m_Velocity.X = 0.0f;
            }

            // Apply friction on Y
            if (m_Velocity.Y != 0.0f)
            {
                m_Velocity.Y -= Settings.GroundFriction * Math.Sign(m_Velocity.Y);

                // Snap to zero if it overshoots
                if (Math.Abs(m_Velocity.Y) < Settings.GroundFriction)
                    m_Velocity.Y = 0.0f;
            }

            GetCurrentAnimation().Update(deltaTime);
        }

        public void RenderTo(Renderer renderer, bool debug = false)
        {
            if (debug)
                renderer.AddQuad(HitboxPosition, HitboxSize, Assets.WhiteTexture);

            renderer.AddQuad(Position, Size, GetCurrentAnimation().GetCurrentTexture(), m_Flipped);
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
            // TODO: Re-enable animations after testing movement
            //switch (m_State)
            //{
            //case State.Idle:            return m_IdleAnimation;
            //case State.Running:         return m_RunningAnimation;
            //
            //default:
            //    break;
            //}

            return m_IdleAnimation;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Private getters // TODO: Check if this is the proper way to do this (since yk gravity)
        ////////////////////////////////////////////////////////////////////////////////////
        public bool IsMovingUp() { return m_Velocity.Y > 0.0f; }
        public bool IsMovingDown() { return m_Velocity.Y < 0.0f; }
        public bool IsMovingHorizontally() { return IsMovingUp() || IsMovingDown(); }
        public bool IsTryingToMoveLeft() { return m_Velocity.X < 0.0f; }
        public bool IsTryingToMoveRight() { return m_Velocity.X > 0.0f; }
        public bool IsMovingVertically() { return IsTryingToMoveLeft() || IsTryingToMoveRight(); }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private State m_State = State.Idle;
        private Maths.Vector2 m_Position = new Maths.Vector2(0.0f, 0.0f);
        private Maths.Vector2 m_HitboxSize = new Maths.Vector2(Settings.SpriteSize - (2 * Settings.Scale) - (2 * Settings.Scale), Settings.SpriteSize - (3 * Settings.Scale));
        private Maths.Vector2 m_Velocity = new Maths.Vector2(0.0f, 0.0f);
        private bool m_Flipped = false;

        private Animation m_IdleAnimation = new Animation(Assets.IdleSheet, 16, 0.4f);
        private Animation m_RunningAnimation = new Animation(Assets.RunSheet, 16, 0.15f);
        // TODO: More animations

        public Maths.Vector2 Position { get { return m_Position; } set { m_Position = value; } }
        public Maths.Vector2 Size { get { return new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize); } }

        public Maths.Vector2 HitboxPosition { get { return new Maths.Vector2(m_Position.X + (2 * Settings.Scale), m_Position.Y); } }
        public Maths.Vector2 HitboxSize { get { return m_HitboxSize; } }

        ////////////////////////////////////////////////////////////////////////////////////
        // Static variables
        ////////////////////////////////////////////////////////////////////////////////////
        private const float c_RunningVelocity = 125.0f;

    }

}