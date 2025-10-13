using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Diagnostics;

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
            m_Position = position;
        }

        //////////////////////////////////////////////////////////////////////////////////
        // Methods
        //////////////////////////////////////////////////////////////////////////////////
        public void RenderTo(Renderer renderer, bool debug = false)
        {
            renderer.AddQuad(m_Position, s_Size, s_Texture);

            //if (debug) // Outline tile hitbox
            //{
            //    renderer.AddQuad(m_HitboxPosition, new Maths.Vector2(m_HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
            //    renderer.AddQuad(m_HitboxPosition, new Maths.Vector2(1 * Settings.Scale, m_HitboxSize.Y), Assets.WhiteTexture);
            //    renderer.AddQuad(new Maths.Vector2(m_HitboxPosition.X, m_HitboxPosition.Y + m_HitboxSize.Y - (1 * Settings.Scale)), new Maths.Vector2(m_HitboxSize.X, 1 * Settings.Scale), Assets.WhiteTexture);
            //    renderer.AddQuad(new Maths.Vector2(m_HitboxPosition.X + m_HitboxSize.X - (1 * Settings.Scale), m_HitboxPosition.Y), new Maths.Vector2(1 * Settings.Scale, m_HitboxSize.Y), Assets.WhiteTexture);
            //}
        }


        //////////////////////////////////////////////////////////////////////////////////
        // Variables
        //////////////////////////////////////////////////////////////////////////////////
        private Maths.Vector2 m_Position;

        public Maths.Vector2 Position { get { return (m_Position); } set { m_Position = value; } }

        private static readonly Maths.Vector2 s_Size = new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize);
        private static readonly CroppedTexture s_Texture = new CroppedTexture(Assets.ObjectsSheet, new UV(32, 0, Settings.SpriteSize / Settings.Scale, Settings.SpriteSize / Settings.Scale));
    }
}