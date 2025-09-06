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
    // Texture
    ////////////////////////////////////////////////////////////////////////////////////
    public class Texture
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public Texture(string path)
        {
            string diskFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(), path);
            //string embeddedFile = "pack://application:,,,/" + path;

            m_Image = new BitmapImage();
            m_Image.BeginInit();
            m_Image.UriSource = new Uri(diskFile, UriKind.Absolute);
            m_Image.CacheOption = BitmapCacheOption.OnLoad;
            m_Image.EndInit();
        }
        ~Texture()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Getters
        ////////////////////////////////////////////////////////////////////////////////////
        public BitmapImage GetInternalImage() { return m_Image; }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private BitmapImage m_Image;

    }

}
