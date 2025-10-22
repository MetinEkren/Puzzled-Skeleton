using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Puzzled
{

    ////////////////////////////////////////////////////////////////////////////////////
    // LevelOverlay // Note: Currently being used to test functionality
    ////////////////////////////////////////////////////////////////////////////////////
    public partial class OptionsMenu : UserControl, IScene
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////


        public OptionsMenu()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }
        public OptionsMenu(IScene previousScene)
        {
            m_PreviousScene = previousScene;

            InitializeComponent();
            Loaded += OnLoad;
        }

        ~OptionsMenu()
        {
            // Note: For future, don't put anything in destructor, since objects are not destroyed at set moment. (GC moment)
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        private void OnLoad(object sender, RoutedEventArgs args) // Note: We need to do this after layout pass to make sure sizes are calculated
        {
            Loaded -= OnLoad;

            MusicSlider.Value = Settings.MusicVolume;
            SFXSlider.Value = Settings.SFXVolume;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!IsLoaded) return;

            MusicValueText.Text = Settings.MusicVolume.ToString() + "%";
            SFXValueText.Text =  Settings.SFXVolume.ToString() + "%";
        }

        public void OnRender()
        {
            if (!IsLoaded) return;
        }

        public void OnUIRender()
        {
            if (!IsLoaded) return;
        }

        public void OnEvent(Event e)
        {
            if (!IsLoaded) return;

            if (e is KeyPressedEvent kpe)
            {
                if (kpe.KeyCode == Key.Escape)
                {
                    Game.Instance.ActiveScene = m_PreviousScene;
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// Callbacks
        ////////////////////////////////////////////////////////////////////////////////////

        void BackButtonPressed(object sender, RoutedEventArgs args)
        {
            Game.Instance.ActiveScene = m_PreviousScene;
        }

        private void MusicSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Settings.MusicVolume = (uint)MusicSlider.Value;
        }

        private void SFXSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Settings.SFXVolume = (uint)SFXSlider.Value;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private IScene m_PreviousScene;


    }        

}