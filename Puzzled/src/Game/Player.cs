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
            // TODO: Fix diagonal movement being 1.4x higher than horizontal and vertical
            // TODO: Remove this movement code and replace with velocity and friction
            bool isMoving = false;
            if (Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.Up))
            {
                m_Position.Y += c_RunningVelocity * deltaTime;
                isMoving = true;
            }
            if (Input.IsKeyPressed(Key.S) || Input.IsKeyPressed(Key.Down))
            {
                m_Position.Y -= c_RunningVelocity * deltaTime;
                isMoving = true;
            }
            if (Input.IsKeyPressed(Key.A) || Input.IsKeyPressed(Key.Left))
            {
                m_Position.X -= c_RunningVelocity * deltaTime;
                isMoving = true;
                m_Flipped = true;
            }
            if (Input.IsKeyPressed(Key.D) || Input.IsKeyPressed(Key.Right))
            {
                m_Position.X += c_RunningVelocity * deltaTime;
                isMoving = true;
                m_Flipped = false;
            }

            if (isMoving && m_State != State.Running)
                SetNewState(State.Running);
            if (!isMoving && m_State != State.Idle)
                SetNewState(State.Idle);

            //m_Position.Y -= Settings.Gravity * deltaTime;

            GetCurrentAnimation().Update(deltaTime);
        }

        public void RenderTo(Renderer renderer)
        {
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
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private State m_State = State.Idle;
        private Maths.Vector2 m_Position = new Maths.Vector2(0.0f, 0.0f);
        private bool m_Flipped = false;

        private Maths.Vector2 m_HitboxSize = new Maths.Vector2(Settings.SpriteSize - (3 * Settings.Scale) - (2 * Settings.Scale), Settings.SpriteSize - (4 * Settings.Scale));

        private Animation m_IdleAnimation = new Animation(Assets.IdleSheet, 16, 0.4f);
        private Animation m_RunningAnimation = new Animation(Assets.RunSheet, 16, 0.15f);
        // TODO: More animations

        public Maths.Vector2 Position { get { return m_Position; } set { m_Position = value; } }
        public Maths.Vector2 Size { get { return new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize); } }

        public Maths.Vector2 HitboxPosition { get { return new Maths.Vector2(m_Position.X + (3 * Settings.Scale), m_Position.Y); } }
        //public Maths.Vector2 HitboxPosition { get { return m_Position; } }
        public Maths.Vector2 HitboxSize { get { return m_HitboxSize; } }

        ////////////////////////////////////////////////////////////////////////////////////
        // Static variables
        ////////////////////////////////////////////////////////////////////////////////////
        private const float c_RunningVelocity = 125.0f;

    }

}