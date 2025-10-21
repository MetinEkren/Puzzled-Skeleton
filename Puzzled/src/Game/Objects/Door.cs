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
        public Door(Maths.Vector2 position, DoorType type)
        {
            Position = position;
            Type = type;
        }

        //////////////////////////////////////////////////////////////////////////////////
        // Methods
        //////////////////////////////////////////////////////////////////////////////////
        public override void RenderTo(Renderer renderer, bool debug = false)
        {
            if (Type == DoorType.KeyDoor && !m_Opened)
            {
                renderer.AddQuad(Position, s_Size, s_TextureKey);
            }
            else if (Type == DoorType.ButtonDoor && !m_Opened)
            {
                renderer.AddQuad(Position, s_Size, s_TextureButton);
            }

            if (debug) // Outline tile hitbox
            {
                renderer.AddQuad(HitboxPosition, new Maths.Vector2(HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(HitboxPosition, new Maths.Vector2(1 * Settings.Scale, HitboxSize.Y), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(HitboxPosition.X, HitboxPosition.Y + HitboxSize.Y - (1 * Settings.Scale)), new Maths.Vector2(HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
                renderer.AddQuad(new Maths.Vector2(HitboxPosition.X + HitboxSize.X - (1 * Settings.Scale), HitboxPosition.Y), new Maths.Vector2(1 * Settings.Scale, HitboxSize.Y), Assets.WhiteTexture);
            }
        }

        public override void Update(float deltaTime)
        {
            if (!m_OpenedForever)
                m_Opened = false;
            else if (m_Opened)
            {
                HitboxSize = new Maths.Vector2(0, 0); // Remove hitbox
            }
        }

        public void Open()
        {
            m_Opened = true;
            Logger.Trace(m_Opened.ToString());
        }

        public void OpenForever()
        {
            m_Opened = true;
            m_OpenedForever = true;
        }

        //////////////////////////////////////////////////////////////////////////////////
        // Variables
        //////////////////////////////////////////////////////////////////////////////////
        public Maths.Vector2 Position;
        public Maths.Vector2 Velocity;
        public Maths.Vector2 HitboxSize = new Maths.Vector2(s_Size.X * 0.75f, s_Size.Y);
        private bool m_Opened;
        private bool m_OpenedForever;

        private static readonly Maths.Vector2 s_Size = new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize);
        private static readonly CroppedTexture s_TextureKey = new CroppedTexture(Assets.ObjectsSheet, new UV(32, 16, Settings.SpriteSize / Settings.Scale, Settings.SpriteSize / Settings.Scale));
        private static readonly CroppedTexture s_TextureButton = new CroppedTexture(Assets.ObjectsSheet, new UV(48, 16, Settings.SpriteSize / Settings.Scale, Settings.SpriteSize / Settings.Scale));

        public Maths.Vector2 HitboxPosition { get { return new Maths.Vector2(Position.X + (2 * Settings.Scale), Position.Y); } }
        public DoorType Type = DoorType.KeyDoor;

    }
}