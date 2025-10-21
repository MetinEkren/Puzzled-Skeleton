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
    public enum DoorType
    {
        KeyDoor = 0,
        ButtonDoor = 1
    }

    //////////////////////////////////////////////////////////////////////////////////
    // Door
    //////////////////////////////////////////////////////////////////////////////////
    public class Door : DynamicObject
    {
        //////////////////////////////////////////////////////////////////////////////////
        // Constructor
        //////////////////////////////////////////////////////////////////////////////////
        public Door(Maths.Vector2 position)
        {
            Position = position;
        }

        //////////////////////////////////////////////////////////////////////////////////
        // Methods
        //////////////////////////////////////////////////////////////////////////////////
        public override void RenderTo(Renderer renderer, bool debug = false)
        {
            renderer.AddQuad(Position, s_Size, s_TextureButton);
            if (debug) // Outline tile hitbox
            {
                renderer.AddQuad(HitboxPosition, new Maths.Vector2(HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(HitboxPosition, new Maths.Vector2(1 * Settings.Scale, HitboxSize.Y), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(HitboxPosition.X, HitboxPosition.Y + HitboxSize.Y - (1 * Settings.Scale)), new Maths.Vector2(HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(HitboxPosition.X + HitboxSize.X - (1 * Settings.Scale), HitboxPosition.Y), new Maths.Vector2(1 * Settings.Scale, HitboxSize.Y), Assets.WhiteTexture);
            }
        }

        public void Open()
        {
            m_Opened = true;
        }

        //////////////////////////////////////////////////////////////////////////////////
        // Variables
        //////////////////////////////////////////////////////////////////////////////////
        public Maths.Vector2 Position;
        public Maths.Vector2 Velocity;
        private bool m_Opened;

        private static readonly Maths.Vector2 s_Size = new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize);
        private static readonly CroppedTexture s_TextureButton = new CroppedTexture(Assets.ObjectsSheet, new UV(32, 16, Settings.SpriteSize / Settings.Scale, Settings.SpriteSize / Settings.Scale));
        private static readonly CroppedTexture s_TextureKey = new CroppedTexture(Assets.ObjectsSheet, new UV(48, 16, Settings.SpriteSize / Settings.Scale, Settings.SpriteSize / Settings.Scale));

        public Maths.Vector2 HitboxPosition { get { return new Maths.Vector2(Position.X + (2 * Settings.Scale), Position.Y); } }
        public Maths.Vector2 HitboxSize { get { return new Maths.Vector2(s_Size.X * 0.75f, s_Size.Y); } }
        public DoorType Type = DoorType.KeyDoor;

    }
}