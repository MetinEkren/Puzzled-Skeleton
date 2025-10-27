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
            Position = Settings.PlayerSpawnPosition;
        }
        public Player(Maths.Vector2 position)
        {
            Position = position;
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
                // Climbing
                if (IsClimbing)
                {
                    Velocity.Y = 0;
                    if (Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.Up) || Input.IsKeyPressed(Key.Space))
                        Velocity.Y = Settings.PlayerJumpingVelocity;
                    else if (Input.IsKeyPressed(Key.S) || Input.IsKeyPressed(Key.Down))
                        Velocity.Y = 0 - Settings.PlayerJumpingVelocity;
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

                // Walking
                else
                {
                    if ((Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.Up) || Input.IsKeyPressed(Key.Space)) && CanJump)
                    {
                        // TODO: Jump cooldown, to prevent super fast jumping up blocks
                        Velocity.Y = Settings.PlayerJumpingVelocity;
                        Assets.JumpSound.Play();
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

                if(!IsClimbing)
                {
                    Velocity.Y -= Settings.Gravity * deltaTime;
                    Velocity.Y = Math.Max(Velocity.Y, Settings.PlayerTerminalVelocity);
                }

                if (m_IsPushing)
                {
                    Velocity.X /= 2;
                    if (!Assets.BoxPushSound.IsPlaying())
                    {
                        Assets.BoxPushSound.Play();
                        Logger.Trace("holy mpoly");
                    }
                }

                // When falling (or jumping) you are no longer able to jump again
                if (Velocity.Y != 0.0f)
                    CanJump = false;
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
            IsClimbing = false;
            m_IsPushing = false;
            
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

        // Note: Resets the player to the start position
        public void Kill()
        {
            Position = Settings.PlayerSpawnPosition;
            Velocity = new Maths.Vector2(0.0f, 0.0f);
            ((LevelOverlay)(Game.Instance.ActiveScene)).Camera.Reset();
        }

        public void Push()
        {
            m_IsPushing = true;
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
                case State.Idle: return m_IdleAnimation;
                case State.Running: return m_RunningAnimation;

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
        public Maths.Vector2 Position;
        public Maths.Vector2 Velocity = new Maths.Vector2(0.0f, 0.0f);
        public bool CanJump = true;
        public bool HasKey = false;
        public bool IsClimbing = false;

        private bool m_IsPushing = false;
        private Animation m_IdleAnimation = new Animation(Assets.IdleSheet, (Settings.SpriteSize / Settings.Scale), Settings.IdleAdvanceTime);
        private Animation m_RunningAnimation = new Animation(Assets.RunSheet, (Settings.SpriteSize / Settings.Scale), Settings.RunAdvanceTime);
        // TODO: More animations

        public Maths.Vector2 Size { get { return new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize); } }

        public Maths.Vector2 HitboxPosition { get { return new Maths.Vector2(Position.X + (2 * Settings.Scale), Position.Y); } }
        public Maths.Vector2 HitboxSize { get { return m_HitboxSize; } }

    }

    }