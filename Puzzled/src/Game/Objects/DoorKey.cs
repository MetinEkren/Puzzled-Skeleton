using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Puzzled
{

    //////////////////////////////////////////////////////////////////////////////////
    // DoorKey
    //////////////////////////////////////////////////////////////////////////////////
    public class DoorKey : DynamicObject
    {
        private bool m_Collected = false;
        private Animation m_IdleAnimation;

        public Maths.Vector2 Position;
        public Maths.Vector2 Velocity;
        private static readonly Maths.Vector2 s_Size = new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize);

        //////////////////////////////////////////////////////////////////////////////////
        // Constructor
        //////////////////////////////////////////////////////////////////////////////////
        public DoorKey(Maths.Vector2 position)
        {
            Position = position;

            m_IdleAnimation = new Animation(
                Assets.DoorKeySheet,
                (Settings.SpriteSize / Settings.Scale),
                0.35f
            );

        }

        public override void Update(float deltaTime)
        {
            if (!m_Collected)
            {
                m_IdleAnimation.Update(deltaTime);
            }
        }
        public override void RenderTo(Renderer renderer, bool debug = false)
        {
            if (m_Collected)
                return;

            renderer.AddQuad(Position, s_Size, m_IdleAnimation.GetCurrentTexture());

            if (debug) // Outline tile hitbox
            {
                renderer.AddQuad(HitboxPosition, new Maths.Vector2(HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(HitboxPosition, new Maths.Vector2(1 * Settings.Scale, HitboxSize.Y), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(HitboxPosition.X, HitboxPosition.Y + HitboxSize.Y - (1 * Settings.Scale)), new Maths.Vector2(HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(HitboxPosition.X + HitboxSize.X - (1 * Settings.Scale), HitboxPosition.Y), new Maths.Vector2(1 * Settings.Scale, HitboxSize.Y), Assets.WhiteTexture);
            }
        }

        public void Collect()
        {
            m_Collected = true;
            HitboxSize = new Maths.Vector2(0, 0);
            Assets.KeyPickupSound.Play();
        }

        public Maths.Vector2 HitboxPosition { get { return Position; } }
        public Maths.Vector2 HitboxSize = s_Size;

    }
}