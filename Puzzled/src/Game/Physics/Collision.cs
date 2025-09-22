using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Puzzled.Physics
{

    //////////////////////////////////////////////////////////////////////////////////
    // CollisionSide
    //////////////////////////////////////////////////////////////////////////////////
    public enum CollisionSide
    {
        None = 0,
        Left = 1 << 0,
        Right = 1 << 1,
        Top = 1 << 2,
        Bottom = 1 << 3,
    }

    //////////////////////////////////////////////////////////////////////////////////
    // Collision
    //////////////////////////////////////////////////////////////////////////////////
    public class Collision
    {

        //////////////////////////////////////////////////////////////////////////////////
        // Static methods
        //////////////////////////////////////////////////////////////////////////////////
        public static CollisionSide AABB(Maths.Vector2 positionA, Maths.Vector2 sizeA, Maths.Vector2 positionB, Maths.Vector2 sizeB)
        // Note: The CollisionSide return is calculated from boxA // TODO: Return multiple sides? with weight?
        {
            CollisionSide side = CollisionSide.None;

            // Get half extents
            float halfWidthA = sizeA.X / 2.0f;
            float halfHeightA = sizeA.Y / 2.0f;
            float halfWidthB = sizeB.X / 2.0f;
            float halfHeightB = sizeB.Y / 2.0f;

            // Get centers
            float centerAx = positionA.X + halfWidthA;
            float centerAy = positionA.Y + halfHeightA;
            float centerBx = positionB.X + halfWidthB;
            float centerBy = positionB.Y + halfHeightB;

            // Calculate difference
            float dx = centerBx - centerAx;
            float dy = centerBy - centerAy;

            // Calculate combined half extents
            float combinedHalfWidths = halfWidthA + halfWidthB;
            float combinedHalfHeights = halfHeightA + halfHeightB;

            // Check for collision
            if (Math.Abs(dx) < combinedHalfWidths && Math.Abs(dy) < combinedHalfHeights)
            {
                // Collision occurred, now determine the side
                float overlapX = combinedHalfWidths - Math.Abs(dx);
                float overlapY = combinedHalfHeights - Math.Abs(dy);

                if (overlapX < overlapY)
                {
                    if (dx > 0)
                        side = CollisionSide.Right; // boxB is on the right of A
                    else
                        side = CollisionSide.Left;  // boxB is on the left of A
                }
                else
                {
                    if (dy > 0)
                        side = CollisionSide.Top; // boxB is above A
                    else
                        side = CollisionSide.Bottom; // boxB is below A
                }
            }

            return side;
        }

    }

}