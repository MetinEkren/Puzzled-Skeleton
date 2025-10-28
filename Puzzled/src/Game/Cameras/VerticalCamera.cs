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
            if (Player.Position.Y < (7 * 48))// if player position is smaller then 336px then it does nothing. 48 is 48px and is one tile.
            {
                return;
            }
            else
            {
                YOffset = Player.Position.Y - (7 * 48);// the camera shows 7 tiles onder the player. 
            }
        }

    }
}
