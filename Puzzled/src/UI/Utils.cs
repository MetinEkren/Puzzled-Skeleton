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

        ////////////////////////////////////////////////////////////////////////////////////
        // Static getters // Note: DIPs are Device Independent Pixels 
        // Note 2: These functions are ChatGPT generated so don't blame me if it doesn't work (Jorben)
        // TODO: Check functionality
        ////////////////////////////////////////////////////////////////////////////////////
        public static double ConvertDIPsToPixels(double dips, Visual visual)
        {
            // Get the DPI info from the visual's presentation source
            var source = PresentationSource.FromVisual(visual);
            if (source?.CompositionTarget != null)
            {
                var dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
                return dips * source.CompositionTarget.TransformToDevice.M11;
            }

            return dips;
        }

        public static double ConvertPixelsToDIPs(double pixels, Visual visual)
        {
            var source = PresentationSource.FromVisual(visual);
            if (source?.CompositionTarget != null)
            {
                return pixels / source.CompositionTarget.TransformToDevice.M11;
            }

            return pixels;
        }

    }

}