using System;
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
    // Fonts
    ////////////////////////////////////////////////////////////////////////////////////
    public class Utils // Note: A static font cache
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Static methods
        ////////////////////////////////////////////////////////////////////////////////////
        public static Maths.Vector2 GetCenter(Canvas canvas, FrameworkElement uiElement)
        {
            // First measure the text with "infinite" space so WPF computes its DesiredSize
            uiElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            
            double x = (canvas.ActualWidth - uiElement.DesiredSize.Width) / 2;
            double y = (canvas.ActualHeight - uiElement.DesiredSize.Height) / 2;

            return new Maths.Vector2((float)x, (float)y);
        }

    }

}