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
    public class Fonts // Note: A static font cache
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Static methods
        ////////////////////////////////////////////////////////////////////////////////////
        public static FontFamily GetFont(string name) // Note: Can return null, result must be checked
        {
            if (!s_Fonts.ContainsKey(name))
            {
                s_Fonts[name] = new FontFamily(name);
            }

            return s_Fonts[name];
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Static variables
        ////////////////////////////////////////////////////////////////////////////////////
        private static Dictionary<string, FontFamily> s_Fonts = new Dictionary<string, FontFamily>();

    }

}