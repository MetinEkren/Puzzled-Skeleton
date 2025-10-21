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
        //////////////////////////////////////////////////////////////////////////////////
        // Constructor
        //////////////////////////////////////////////////////////////////////////////////
        public DoorKey(Maths.Vector2 position)
        {
            Position = position;
        }

        public override void RenderTo(Renderer renderer, bool debug = false)
        {
            renderer.AddQuad(Position, s_Size, s_Texture);

            if (debug) // Outline tile hitbox
            {
                renderer.AddQuad(HitboxPosition, new Maths.Vector2(HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(HitboxPosition, new Maths.Vector2(1 * Settings.Scale, HitboxSize.Y), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(HitboxPosition.X, HitboxPosition.Y + HitboxSize.Y - (1 * Settings.Scale)), new Maths.Vector2(HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(HitboxPosition.X + HitboxSize.X - (1 * Settings.Scale), HitboxPosition.Y), new Maths.Vector2(1 * Settings.Scale, HitboxSize.Y), Assets.WhiteTexture);
            }
        }
        public Maths.Vector2 Position;
        public Maths.Vector2 Velocity;

        private static readonly Maths.Vector2 s_Size = new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize);
        private static readonly CroppedTexture s_Texture = new CroppedTexture(Assets.ObjectsSheet, new UV(16, 0, Settings.SpriteSize / Settings.Scale, Settings.SpriteSize / Settings.Scale));

        public Maths.Vector2 HitboxPosition { get { return Position; } }
        public Maths.Vector2 HitboxSize { get { return s_Size; } }

    }
}