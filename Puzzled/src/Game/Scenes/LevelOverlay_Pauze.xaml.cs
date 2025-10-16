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

        public LevelOverlay_Pauze(LevelOverlay levelOverlay)
        {
            InitializeComponent();
            m_Overlay = levelOverlay;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Callbacks
        ////////////////////////////////////////////////////////////////////////////////////
        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            m_Overlay.PauseOverlay.Content = null; // This removes the overlay
            m_Overlay.Pauze = false;
        }

        private void Button_Click_OptionsMenu(object sender, RoutedEventArgs e)
        {
            Game.Instance.ActiveScene = new OptionsMenu(m_Overlay);
        }

        private void Button_Click_Restart(object sender, RoutedEventArgs e)
        {
            m_Overlay.PauseOverlay.Content = null; // This removes the overlay
            m_Overlay.Pauze = false;

            m_Overlay.LoadLevel(m_Overlay.ActiveSave.Level);
        }

        private void Button_Click_Quit(object sender, RoutedEventArgs e)
        {
            m_Overlay.PauseOverlay.Content = null; // This removes the overlay
            m_Overlay.Pauze = false;

            Game.Instance.ActiveScene = new MainMenu();
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private LevelOverlay m_Overlay;

    }

}