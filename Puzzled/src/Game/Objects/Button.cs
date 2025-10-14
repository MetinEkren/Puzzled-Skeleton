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
    // Button
    //////////////////////////////////////////////////////////////////////////////////
    public class Button : DynamicObject
    {
        //////////////////////////////////////////////////////////////////////////////////
        // Constructor
        //////////////////////////////////////////////////////////////////////////////////
        public Button(Maths.Vector2 position)
        {
            Position = position;
        }

        //////////////////////////////////////////////////////////////////////////////////
        // Methods
        //////////////////////////////////////////////////////////////////////////////////
        public override void RenderTo(Renderer renderer, bool debug = false)
        {
            renderer.AddQuad(Position, s_Size, s_Texture1);

            if (debug) // Outline tile hitbox
            {
                renderer.AddQuad(HitboxPosition, new Maths.Vector2(HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(HitboxPosition, new Maths.Vector2(1 * Settings.Scale, HitboxSize.Y), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(HitboxPosition.X, HitboxPosition.Y + HitboxSize.Y - (1 * Settings.Scale)), new Maths.Vector2(HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(HitboxPosition.X + HitboxSize.X - (1 * Settings.Scale), HitboxPosition.Y), new Maths.Vector2(1 * Settings.Scale, HitboxSize.Y), Assets.WhiteTexture);
            }
        }

        public void ChangeTexture(Renderer renderer, bool pressed)
        {
            if (pressed)
                renderer.AddQuad(Position, s_Size, s_Texture2);
            else
                renderer.AddQuad(Position, s_Size, s_Texture1);
        }

        //////////////////////////////////////////////////////////////////////////////////
        // Variables
        //////////////////////////////////////////////////////////////////////////////////
        public Maths.Vector2 Position;
        public Maths.Vector2 Velocity;

        private static readonly Maths.Vector2 s_Size = new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize);
        private static readonly CroppedTexture s_Texture1 = new CroppedTexture(Assets.ObjectsSheet, new UV(0, 16, Settings.SpriteSize / Settings.Scale, Settings.SpriteSize / Settings.Scale));
        private static readonly CroppedTexture s_Texture2 = new CroppedTexture(Assets.ObjectsSheet, new UV(16, 16, Settings.SpriteSize / Settings.Scale, Settings.SpriteSize / Settings.Scale));


        public Maths.Vector2 HitboxPosition { get { return Position; } }
        public Maths.Vector2 HitboxSize { get { return s_Size; } }

    }
}