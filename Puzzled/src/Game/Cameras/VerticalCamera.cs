using Puzzled.Physics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Puzzled
{

    //////////////////////////////////////////////////////////////////////////////////
    // VerticalCamera
    //////////////////////////////////////////////////////////////////////////////////
    public class VerticalCamera : LevelCamera
    {
        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor
        ////////////////////////////////////////////////////////////////////////////////////
        public VerticalCamera(Player player)
            : base(player)
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        public override void Update()
        {
            if (Player.Position.Y < (7 * 48))
            {
                return;
            }
            else
            {
                YOffset = Player.Position.Y - (7 * 48);
            }
        }

    }
}
