using Puzzled;
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
    public enum BridgeSide
    {
        Left = 1,
        Middle = 2,
        Right = 3
    }

    //////////////////////////////////////////////////////////////////////////////////
    // Bridge
    //////////////////////////////////////////////////////////////////////////////////
    public class Bridge : DynamicObject
    {
        //////////////////////////////////////////////////////////////////////////////////
        // Constructor
        //////////////////////////////////////////////////////////////////////////////////
        public Bridge(Maths.Vector2 position, BridgeSide side)
        {
            Position = position;
            m_Side = side;
            
        }

        //////////////////////////////////////////////////////////////////////////////////
        // Methods
        //////////////////////////////////////////////////////////////////////////////////
        public override void RenderTo(Renderer renderer, bool debug = false)
        {
            switch (m_Side)
            {
                case BridgeSide.Left:
                    renderer.AddQuad(Position, s_Size, s_TextureLeft);
                    break;
                case BridgeSide.Middle:
                    renderer.AddQuad(Position, s_Size, s_TextureMiddle);
                    break;
                case BridgeSide.Right:
                    renderer.AddQuad(Position, s_Size, s_TextureRight);
                    break;
            } 

            if (debug)
            {
                // Debug hitbox randen
                renderer.AddQuad(HitboxPosition, new Maths.Vector2(HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(HitboxPosition, new Maths.Vector2(1 * Settings.Scale, HitboxSize.Y), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(HitboxPosition.X, HitboxPosition.Y + HitboxSize.Y - (1 * Settings.Scale)), new Maths.Vector2(HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(HitboxPosition.X + HitboxSize.X - (1 * Settings.Scale), HitboxPosition.Y), new Maths.Vector2(1 * Settings.Scale, HitboxSize.Y), Assets.WhiteTexture);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////
        // Variables
        //////////////////////////////////////////////////////////////////////////////////
        public Maths.Vector2 Position;
        private BridgeSide m_Side;

        private static readonly Maths.Vector2 s_Size = new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize);
        private static readonly CroppedTexture s_TextureLeft = new CroppedTexture(Assets.ObjectsSheet, new UV(0, 48, Settings.SpriteSize / Settings.Scale, Settings.SpriteSize / Settings.Scale));
        private static readonly CroppedTexture s_TextureMiddle = new CroppedTexture(Assets.ObjectsSheet, new UV(16, 48, Settings.SpriteSize / Settings.Scale, Settings.SpriteSize / Settings.Scale));
        private static readonly CroppedTexture s_TextureRight = new CroppedTexture(Assets.ObjectsSheet, new UV(32, 48, Settings.SpriteSize / Settings.Scale, Settings.SpriteSize / Settings.Scale));

        public Maths.Vector2 HitboxPosition { get { return (new Maths.Vector2(Position.X, Position.Y + Settings.SpriteSize / 2)); } }
        public Maths.Vector2 HitboxSize { get { return new Maths.Vector2(s_Size.X, (Settings.SpriteSize / 2)); } }
    }

}