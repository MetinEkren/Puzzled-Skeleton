using System;
using System.IO;
using System.Media;
using System.Text;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Puzzled
{

    ////////////////////////////////////////////////////////////////////////////////////
    // AudioFile
    ////////////////////////////////////////////////////////////////////////////////////
    public class AudioFile
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public AudioFile(string path, uint volume = 50)
        {
            Volume = volume;

            string diskFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(), path);
            //string embeddedFile = "pack://application:,,,/" + path;

            m_URI = new Uri(diskFile, UriKind.Absolute);
        }
        ~AudioFile() 
        { 
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        public void Play()
        {
            // Track player
            MediaPlayer mediaPlayer = new MediaPlayer();
            m_ActiveInstances.Add(mediaPlayer);

            // Open
            mediaPlayer.Open(m_URI);
            mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Volume = m_Volume;

            // Handle events
            mediaPlayer.MediaEnded += (s, e) =>
            {
                mediaPlayer.Close();
                m_ActiveInstances.Remove(mediaPlayer);
            };

            mediaPlayer.MediaFailed += (s, e) =>
            {
                mediaPlayer.Close();
                m_ActiveInstances.Remove(mediaPlayer);
            };

            // Finally play
            mediaPlayer.Play();
        }

        public void CloseAll()
        {
            foreach (var instance in m_ActiveInstances)
            {
                instance.Stop();
                instance.Close();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Getter
        ////////////////////////////////////////////////////////////////////////////////////
        public bool IsPlaying() { return (m_ActiveInstances.Count != 0); }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Uri m_URI;
        private double m_Volume = 0.5; // 50%

        private List<MediaPlayer> m_ActiveInstances = new List<MediaPlayer>();

        public uint Volume // Note: Percentage from 0% to 100%
        { 
            get { return (uint)(m_Volume * 100); }
            set { m_Volume = Math.Min(value / 100.0, 1.0); } 
        }

    }

}
