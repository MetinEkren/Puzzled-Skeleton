using System;
using System.Diagnostics;
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
using static Puzzled.Renderer;

namespace Puzzled
{

    ////////////////////////////////////////////////////////////////////////////////////
    // UV
    ////////////////////////////////////////////////////////////////////////////////////
    public struct UV
    {
        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        // Note: All values are in pixels
        public uint X, Y;
        public uint Width, Height;

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor
        ////////////////////////////////////////////////////////////////////////////////////
        public UV(uint x, uint y, uint width, uint height) { X = x; Y = y; Width = width; Height = height; }
        public UV(Texture texture) { X = 0; Y = 0; Width = texture.Width; Height = texture.Height; }
    }

    ////////////////////////////////////////////////////////////////////////////////////
    // ITexture
    ////////////////////////////////////////////////////////////////////////////////////
    public interface ITexture 
    {
        ImageSource GetImageSource();
        BitmapSource GetBitmapSource();
    }

    ////////////////////////////////////////////////////////////////////////////////////
    // Texture
    ////////////////////////////////////////////////////////////////////////////////////
    public class Texture : ITexture
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public Texture() // Creates a white 1x1 texture
            : this(new byte[]{ 255, 255, 255, 255 })
        {
        }
        public Texture(byte[] colorData) // 4 Bytes
        {
            Debug.Assert(colorData.Length == 4, "Must specify RGBA, in BGRA format.");

            WriteableBitmap bmp = new WriteableBitmap(1, 1, 96, 96, PixelFormats.Bgra32, null);
            Int32Rect rect = new Int32Rect(0, 0, 1, 1);
            bmp.WritePixels(rect, colorData, 4, 0);

            m_Image = bmp;
        }
        public Texture(string path)
        {
            string diskFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(), path);
            //string embeddedFile = "pack://application:,,,/" + path;

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(diskFile, UriKind.Absolute);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();

            m_Image = image;
        }
        ~Texture()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Getters
        ////////////////////////////////////////////////////////////////////////////////////
        public ImageSource GetImageSource() { return m_Image; }
        public BitmapSource GetBitmapSource() { return m_Image; }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private BitmapSource m_Image;

        public uint Width { get { return (uint)m_Image.PixelWidth; } }
        public uint Height { get { return (uint)m_Image.PixelHeight; } }

    }

    ////////////////////////////////////////////////////////////////////////////////////
    // CroppedTexture
    ////////////////////////////////////////////////////////////////////////////////////
    public class CroppedTexture : ITexture
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public CroppedTexture(Texture texture, UV textureCoords)
        {
            m_Image = new CroppedBitmap(
                texture.GetBitmapSource(),
                new Int32Rect((int)textureCoords.X, (int)textureCoords.Y, (int)textureCoords.Width, (int)textureCoords.Height) // UV rectangle in pixels
            );
            m_Image.Freeze(); // TODO: Optimize cropping
        }
        ~CroppedTexture()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Getters
        ////////////////////////////////////////////////////////////////////////////////////
        public ImageSource GetImageSource() { return m_Image; }
        public BitmapSource GetBitmapSource() { return m_Image; }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private CroppedBitmap m_Image;

        public uint Width { get { return (uint)m_Image.PixelWidth; } }
        public uint Height { get { return (uint)m_Image.PixelHeight; } }

    }

}
