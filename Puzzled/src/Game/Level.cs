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

            var side = Collision.AABB(m_Player.Position, m_Player.Size, m_TESTTile.Position, m_TESTTile.Size);
            if (side != CollisionSide.None)
            {
                switch (side)
                { 
                case CollisionSide.Left:
                    Logger.Trace("Left");
                    break;

                case CollisionSide.Right:
                    Logger.Trace("Right");
                    break;

                case CollisionSide.Top:
                    Logger.Trace("Top");
                    break;

                case CollisionSide.Bottom:
                    Logger.Trace("Bottom");
                    break;

                default:
                    break;
                }
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