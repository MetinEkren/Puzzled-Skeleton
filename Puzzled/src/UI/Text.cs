using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Puzzled.UI
{

    ////////////////////////////////////////////////////////////////////////////////////
    // Text
    ////////////////////////////////////////////////////////////////////////////////////
    public class Text // Note: A TextBlock with it's position built in.
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructors & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public Text() { }
        public Text(string text, float sizeInDPI, string fontName, Maths.Vector2 position)
            : this(text, sizeInDPI, fontName, position, Brushes.White)
        {
        }
        public Text(string text, float sizeInDPI, string fontName, Maths.Vector2 position, Brush brush)
        {
            UIElement = new TextBlock
            {
                Text = text,
                Foreground = brush,
                FontSize = (double)sizeInDPI,
                FontFamily = Fonts.GetFont(fontName)
            };
        }
        ~Text()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        public void AddToCanvas(Canvas canvas)
        {
            canvas.Children.Add(UIElement);
        }

        public void RemoveFromCanvas(Canvas canvas)
        {
            canvas.Children.Remove(UIElement);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private Maths.Vector2 m_Position;
        public Maths.Vector2 Position 
        { 
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
                Canvas.SetLeft(UIElement, m_Position.X);
                Canvas.SetTop(UIElement, m_Position.Y);
            }
        }
        public TextBlock UIElement;

    }

}