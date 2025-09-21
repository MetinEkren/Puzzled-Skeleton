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
            m_IdleAnimation.Update(deltaTime);

            Logger.Trace($"Current sprite: {m_IdleAnimation.GetCurrentSpriteID()}");
        }

        public void OnRender()
        {
            m_Renderer.Begin();
            m_StaticTile.RenderTo(m_Renderer);

            m_Renderer.AddQuad(new Maths.Vector2(48.0f, 48.0f), new Maths.Vector2(48.0f, 48.0f), m_IdleAnimation.GetCurrentTexture());

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

        private Animation m_IdleAnimation = new Animation(Assets.IdleSheet, 16, 0.4f);
        private StaticTile m_StaticTile = new StaticTile(new Maths.Vector2(0.0f, 0.0f), new Maths.Vector2(16.0f * 3, 16.0f * 3), Assets.PainSheet);

    }

}