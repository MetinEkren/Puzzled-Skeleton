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
            if (!(Game.Instance.ActiveScene is LevelOverlay)) // Fixes a bug, the root issue is yet to be 
                return;

            uint bottomBuffer = (7 * Settings.SpriteSize);
            uint topBuffer = (8 * Settings.SpriteSize);

            if (Player.Position.Y < bottomBuffer) // if player position is smaller then 336px then it does nothing. 48 is 48px and is one tile.
                return;
            else if (Player.Position.Y > (((LevelOverlay)(Game.Instance.ActiveScene)).Level.Height - topBuffer))
                return;

            YOffset = Player.Position.Y - bottomBuffer;// the camera shows 7 tiles onder the player. 
        }

    }
}
