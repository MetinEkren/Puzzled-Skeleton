﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;

namespace Puzzled
{

    ////////////////////////////////////////////////////////////////////////////////////
    // WinMenu // Note: Currently being used to test functionality
    ////////////////////////////////////////////////////////////////////////////////////
    public partial class WinMenu : UserControl, IScene
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public WinMenu()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }
        public WinMenu(LevelOverlay instance)
        {
            m_Level = instance;

            InitializeComponent();
            Loaded += OnLoad;
        }
        ~WinMenu()
        {
            // Note: For future, don't put anything in destructor, since objects are not destroyed at set moment. (GC moment)
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void OnLoad(object sender, RoutedEventArgs args) // Note: We need to do this after layout pass to make sure sizes are calculated
        {
            m_Renderer = new Renderer(UICanvas);

            Save save1 = Assets.LoadSave(1);
            Save save2 = Assets.LoadSave(2);
            Save save3 = Assets.LoadSave(3);

            PlayerName1.Text = save1.Name;
            PlayerName2.Text = save2.Name;
            PlayerName3.Text = save3.Name;

            List<Save> saves = new List<Save> { save1, save2, save3 };
            saves.Sort(new SortSaveScoreHelper(m_Level.ActiveSave.Level));

            // TODO: inplement scores sorting + Score display

            PlayerScore1.Text = saves[0].Scores[(int)m_Level.ActiveSave.Level - 1].ToString();
            PlayerScore2.Text = saves[1].Scores[(int)m_Level.ActiveSave.Level - 1].ToString();
            PlayerScore3.Text = saves[2].Scores[(int)m_Level.ActiveSave.Level - 1].ToString();

            Loaded -= OnLoad;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!IsLoaded) return;
            m_IdleAnimation.Update(deltaTime);
        }

        public void OnRender()
        {
            if (!IsLoaded) return;
        }

        public void OnUIRender()
        {
            if (!IsLoaded) return;
            m_Renderer.Begin();
            m_Renderer.AddQuad(new Maths.Vector2(570f, 0.0f), new Maths.Vector2(200f, 200f), m_IdleAnimation.GetCurrentTexture(), true);
            m_Renderer.End();
        }

        public void OnEvent(Event e)
        {
            if (!IsLoaded) return;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Callbacks
        ////////////////////////////////////////////////////////////////////////////////////
        void NextLevelPressed(object sender, RoutedEventArgs args)
        {
            Logger.Info($"Going to next level, {m_Level.ActiveSave.Level}.");
            m_Level.LoadLevel(m_Level.ActiveSave.Level);
            Game.Instance.ActiveScene = m_Level;
        }

        void BackToMainMenu(object sender, RoutedEventArgs args)
        {
            Logger.Info("Going back to main menu.");
            Game.Instance.ActiveScene = new MainMenu();
        }

        void RestartGame(object sender, RoutedEventArgs args)
        {
            Logger.Info($"Going to next level, {m_Level.ActiveSave.Level}.");

            m_Level.ActiveSave = new Save
            {
                Name = m_Level.ActiveSave.Name,
                Level = m_Level.ActiveSave.Level - 1,
                Scores = m_Level.ActiveSave.Scores
            }; // Restart current level

            m_Level.LoadLevel(m_Level.ActiveSave.Level);
            Game.Instance.ActiveScene = m_Level;
        }

        void OptionsMenu(object sender, RoutedEventArgs args)
        {
            Logger.Info("Going to option menu");
            Game.Instance.ActiveScene = new OptionsMenu();
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Renderer m_Renderer;

        private LevelOverlay m_Level;
        private Animation m_IdleAnimation = new Animation(Assets.IdleSheet, (Settings.SpriteSize / Settings.Scale), Settings.IdleAdvanceTime);

    }

}