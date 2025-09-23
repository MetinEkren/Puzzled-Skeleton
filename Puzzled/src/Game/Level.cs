using Puzzled.Physics;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using System.Windows.Input;

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
            s_CurrentLevel = this;
            m_Renderer = new Renderer(canvas);
            Load(levelPath);
        }
        ~Level()
        {
            s_CurrentLevel = null;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        public void OnUpdate(float deltaTime)
        {
            m_Player.Update(deltaTime);

            var collision = Collision.AABB(m_Player.HitboxPosition, m_Player.HitboxSize, m_Ground.Position, m_Ground.Size);
            if (collision.Side != CollisionSide.None)
            {
                switch (collision.Side)
                { 
                //case CollisionSide.Left:
                //    m_TESTTile.Position = new Maths.Vector2(m_TESTTile.Position.X + collision.Overlap, m_TESTTile.Position.Y);
                //    break;
                //
                //case CollisionSide.Right:
                //    m_TESTTile.Position = new Maths.Vector2(m_TESTTile.Position.X - collision.Overlap, m_TESTTile.Position.Y);
                //    break;
                //
                //case CollisionSide.Top:
                //    m_TESTTile.Position = new Maths.Vector2(m_TESTTile.Position.X, m_TESTTile.Position.Y - collision.Overlap);
                //    break;
                
                case CollisionSide.Bottom:
                    m_Player.Position = new Maths.Vector2(m_Player.Position.X, m_Player.Position.Y + collision.Overlap);
                    m_Player.Velocity = new Maths.Vector2(m_Player.Velocity.X, 0.0f);
                    break;
                
                default:
                    break;
                }
            }
        }

        public void OnRender()
        {
            m_Renderer.Begin();
            m_Ground.RenderTo(m_Renderer, m_Debug);
            m_Player.RenderTo(m_Renderer, m_Debug);
            m_Renderer.End();
        }

        public void OnEvent(Event e)
        {
            if (e is KeyPressedEvent kpe)
            {
                // Note: For testing a debug 
                if (kpe.KeyCode == Key.H)
                    m_Debug = !m_Debug;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Loading
        ////////////////////////////////////////////////////////////////////////////////////
        public void Load(string path)
        {
            Logger.Info($"Loading level from: {path}");

            m_Player = new Player();
            
            // TODO: Remove
            m_Ground = new StaticTile(new Maths.Vector2(0.0f, 0.0f), new Maths.Vector2(Game.Instance.Window.Width, Settings.SpriteSize), Assets.WhiteTexture);

            // TODO: ...
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Renderer m_Renderer;
        private bool m_Debug = false;

        private Player m_Player;
        private StaticTile m_Ground;

        ////////////////////////////////////////////////////////////////////////////////////
        // Static variables
        ////////////////////////////////////////////////////////////////////////////////////
        private static Level s_CurrentLevel;

        public static Level Active { get { return s_CurrentLevel; } }

    }

}