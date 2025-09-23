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
                    Logger.Trace("Left");
                    m_TESTTile.Position = new Maths.Vector2(m_TESTTile.Position.X + collision.Overlap, m_TESTTile.Position.Y);
                    break;
                
                case CollisionSide.Right:
                    Logger.Trace("Right");
                    m_TESTTile.Position = new Maths.Vector2(m_TESTTile.Position.X - collision.Overlap, m_TESTTile.Position.Y);
                    break;
                
                case CollisionSide.Top:
                    Logger.Trace("Top");
                    m_TESTTile.Position = new Maths.Vector2(m_TESTTile.Position.X, m_TESTTile.Position.Y - collision.Overlap);
                    break;
                
                case CollisionSide.Bottom:
                    Logger.Trace("Bottom");
                    m_TESTTile.Position = new Maths.Vector2(m_TESTTile.Position.X, m_TESTTile.Position.Y + collision.Overlap);
                    break;
                
                default:
                    break;
                }

                Logger.Trace($"Player edges: L={m_Player.HitboxPosition.X}, R={m_Player.HitboxPosition.X + m_Player.HitboxSize.X}, " +
             $"T={m_Player.HitboxPosition.Y}, B={m_Player.HitboxPosition.Y + m_Player.HitboxSize.Y}");

                Logger.Trace($"Tile edges:   L={m_TESTTile.Position.X}, R={m_TESTTile.Position.X + m_TESTTile.Size.X}, " +
                             $"T={m_TESTTile.Position.Y}, B={m_TESTTile.Position.Y + m_TESTTile.Size.Y}");

                Logger.Trace($"Overlap={collision.Overlap}, Side={collision.Side}");


                Logger.Trace($"{collision.Overlap}");
            }
        }

        public void OnRender()
        {
            m_Renderer.Begin();
            m_TESTTile.RenderTo(m_Renderer);
            m_Player.RenderTo(m_Renderer);
            m_Renderer.End();
        }

        public void OnEvent(Event e)
        {

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

        private Player m_Player = new Player();
        private StaticTile m_TESTTile = new StaticTile(new Maths.Vector2(100.0f, 100.0f), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.WhiteTexture);

    }

}