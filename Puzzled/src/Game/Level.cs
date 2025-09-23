using Puzzled.Physics;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Puzzled
{

    //////////////////////////////////////////////////////////////////////////////////
    // Level
    //////////////////////////////////////////////////////////////////////////////////
    public class Level
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public Level(Canvas canvas, string levelPath)
        {
            m_Renderer = new Renderer(canvas);
            Load(levelPath);
        }
        ~Level()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        public void OnUpdate(float deltaTime)
        {
            m_Player.Update(deltaTime);

            var collision = Collision.AABB(m_TESTTile.Position, m_TESTTile.Size, m_Player.HitboxPosition, m_Player.HitboxSize);
            if (collision.Side != CollisionSide.None)
            {
                switch (collision.Side)
                { 
                case CollisionSide.Left:
                    m_TESTTile.Position = new Maths.Vector2(m_TESTTile.Position.X + collision.Overlap, m_TESTTile.Position.Y);
                    break;
                
                case CollisionSide.Right:
                    m_TESTTile.Position = new Maths.Vector2(m_TESTTile.Position.X - collision.Overlap, m_TESTTile.Position.Y);
                    break;
                
                case CollisionSide.Top:
                    m_TESTTile.Position = new Maths.Vector2(m_TESTTile.Position.X, m_TESTTile.Position.Y - collision.Overlap);
                    break;
                
                case CollisionSide.Bottom:
                    m_TESTTile.Position = new Maths.Vector2(m_TESTTile.Position.X, m_TESTTile.Position.Y + collision.Overlap);
                    break;
                
                default:
                    break;
                }
            }
        }

        public void OnRender()
        {
            m_Renderer.Begin();
            m_TESTTile.RenderTo(m_Renderer, m_Debug);
            m_Player.RenderTo(m_Renderer, m_Debug);
            m_Renderer.End();
        }

        public void OnEvent(Event e)
        {
            if (e is KeyPressedEvent kpe)
            {
                // Note: For testing a debug 
                if (kpe.KeyCode == System.Windows.Input.Key.H)
                    m_Debug = !m_Debug;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Loading
        ////////////////////////////////////////////////////////////////////////////////////
        public void Load(string path)
        {
            Logger.Info($"Loading level from: {path}");

            // TODO: ...
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Renderer m_Renderer;
        private bool m_Debug = false;

        private Player m_Player = new Player();
        private StaticTile m_TESTTile = new StaticTile(new Maths.Vector2(100.0f, 100.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.WhiteTexture);

    }

}