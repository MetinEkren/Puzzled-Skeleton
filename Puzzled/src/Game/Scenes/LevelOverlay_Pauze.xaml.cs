using System;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Puzzled
{

    ////////////////////////////////////////////////////////////////////////////////////
    // LevelOverlay_Pauze
    ////////////////////////////////////////////////////////////////////////////////////
    public partial class LevelOverlay_Pauze : UserControl
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor
        ////////////////////////////////////////////////////////////////////////////////////
        public LevelOverlay_Pauze()
        {
            InitializeComponent();
        }

        public LevelOverlay_Pauze(LevelOverlay levelOverlay, Puzzled.CustomStopWatch stopWatch)
        {
            InitializeComponent();
            m_Overlay = levelOverlay;
            m_CustomStopWatch = stopWatch;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Callbacks
        ////////////////////////////////////////////////////////////////////////////////////
        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            m_Overlay.PauseOverlay.Content = null; // This removes the overlay
            m_Overlay.Paused = false;
            m_CustomStopWatch.Start();
        }

        private void Button_Click_OptionsMenu(object sender, RoutedEventArgs e)
        {
            Game.Instance.ActiveScene = new OptionsMenu(m_Overlay);
        }

        private void Button_Click_Restart(object sender, RoutedEventArgs e)
        {
            m_Overlay.PauseOverlay.Content = null; // This removes the overlay
            m_Overlay.Paused = false;

            m_CustomStopWatch.Pauze();
            m_CustomStopWatch.Reset();

            m_Overlay.LoadLevel(m_Overlay.ActiveSave.Level);

            m_Overlay.Camera.Player = m_Overlay.Level.Player;
            m_Overlay.Camera.Reset();

            m_CustomStopWatch.Start();
        }

        private void Button_Click_Quit(object sender, RoutedEventArgs e)
        {
            m_Overlay.PauseOverlay.Content = null; // This removes the overlay
            m_Overlay.Paused = false;

            m_CustomStopWatch.Pauze();
            m_CustomStopWatch.Reset();
            Assets.LevelMusic.Stop();

            Game.Instance.ActiveScene = new MainMenu();
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private LevelOverlay m_Overlay;
        private Puzzled.CustomStopWatch m_CustomStopWatch;
    }

}