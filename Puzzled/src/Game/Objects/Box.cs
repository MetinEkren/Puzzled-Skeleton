using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using static Puzzled.Player;

namespace Puzzled
{

    //////////////////////////////////////////////////////////////////////////////////
    // Box
    //////////////////////////////////////////////////////////////////////////////////
    public class Box : DynamicObject
    {

        //////////////////////////////////////////////////////////////////////////////////
        // Constructor
        //////////////////////////////////////////////////////////////////////////////////
        public Box(Maths.Vector2 position)
        {
            Position = position;
            HitboxSize = s_Size;
        }

        //////////////////////////////////////////////////////////////////////////////////
        // Methods
        //////////////////////////////////////////////////////////////////////////////////
        public override void Update(float deltaTime)
        {
            if (!m_Destroyed)
            {
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
                    Velocity.Y = Math.Max(Velocity.Y, Settings.BoxTerminalVelocity);
                }

                // Velocity
                {
                    Position.X += Velocity.X * deltaTime;
                    Position.Y += Velocity.Y * deltaTime;
                }
            }
        }

        public override void RenderTo(Renderer renderer, bool debug = false)
        {
            if (!m_Destroyed)
                renderer.AddQuad(Position, s_Size, s_Texture);

            if (debug) // Outline tile hitbox
            {
                renderer.AddQuad(HitboxPosition, new Maths.Vector2(HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(HitboxPosition, new Maths.Vector2(1 * Settings.Scale, HitboxSize.Y), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(HitboxPosition.X, HitboxPosition.Y + HitboxSize.Y - (1 * Settings.Scale)), new Maths.Vector2(HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(HitboxPosition.X + HitboxSize.X - (1 * Settings.Scale), HitboxPosition.Y), new Maths.Vector2(1 * Settings.Scale, HitboxSize.Y), Assets.WhiteTexture);
            }
        }

        public void Destroy()
        {
            m_Destroyed = true;
            HitboxSize = new Maths.Vector2(0, 0);
        }

        //////////////////////////////////////////////////////////////////////////////////
        // Variables
        //////////////////////////////////////////////////////////////////////////////////
        public Maths.Vector2 Position;
        public Maths.Vector2 Velocity;
        public Maths.Vector2 HitboxSize;

        private static readonly Maths.Vector2 s_Size = new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize);
        private static readonly CroppedTexture s_Texture = new CroppedTexture(Assets.ObjectsSheet, new UV(32, 0, Settings.SpriteSize / Settings.Scale, Settings.SpriteSize / Settings.Scale));
        private bool m_Destroyed = false;
    
        public Maths.Vector2 HitboxPosition { get { return Position; } }

    }
}