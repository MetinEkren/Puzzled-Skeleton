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
            m_Overlay.PauseOverlay.Content = null; // This removes the overlay // pause menu goes away
            m_Overlay.Paused = false;
            m_CustomStopWatch.Start();//time starts again 
        }

        private void Button_Click_OptionsMenu(object sender, RoutedEventArgs e)
        {
            Game.Instance.ActiveScene = new OptionsMenu(m_Overlay);//it goes to option menu
        }

        private void Button_Click_Restart(object sender, RoutedEventArgs e)
        {
            m_Overlay.PauseOverlay.Content = null; // This removes the overlay // pause menu goes away
            m_Overlay.Paused = false;

            m_CustomStopWatch.Pauze();// stops time
            m_CustomStopWatch.Reset();// resets time

            m_Overlay.LoadLevel(m_Overlay.ActiveSave.Level);// resets the same level
            m_Overlay.Camera.Player = m_Overlay.Level.Player;// camera changes to new player
            m_Overlay.Camera.Reset();

            m_CustomStopWatch.Start();// start time
        }

        private void Button_Click_Quit(object sender, RoutedEventArgs e)
        {
            m_Overlay.PauseOverlay.Content = null; // This removes the overlay // pause menu goes away
            m_Overlay.Paused = false;

            m_CustomStopWatch.Pauze();// stops time
            m_CustomStopWatch.Reset();// resets time
            Assets.LevelMusic.Stop();// stops music

            Game.Instance.ActiveScene = new MainMenu();// goes to main menu
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private LevelOverlay m_Overlay;
        private Puzzled.CustomStopWatch m_CustomStopWatch;
    }

}