using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameInteraction
{

    ////////////////////////////////////////////////////////////////////////////////////
    // AudioFile
    ////////////////////////////////////////////////////////////////////////////////////
    public class AudioFile
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public AudioFile(string path)
        {
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
        public void Play() // TODO: Future optimization, don't open the file each time and somehow just play
        {
            MediaPlayer mediaPlayer = new MediaPlayer();

            mediaPlayer.Open(m_URI);
            mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Volume = m_Volume;

            mediaPlayer.MediaEnded += (s, e) =>
            {
                mediaPlayer.Close();
            };

            mediaPlayer.Play();
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Uri m_URI;
        private double m_Volume = 0.5; // 50%
        
        public uint Volume // Note: Percentage from 0% to 100%
        { 
            get { return (uint)(m_Volume * 100); }
            set 
            {
                m_Volume = Math.Min(value / 100.0, 1.0); 
            } 
        }
    }

}
