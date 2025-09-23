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
    // CollisionResult
    //////////////////////////////////////////////////////////////////////////////////
    public struct CollisionResult
    { 
        public CollisionSide Side;
        public float Overlap;
    }

    //////////////////////////////////////////////////////////////////////////////////
    // Collision
    //////////////////////////////////////////////////////////////////////////////////
    public class Collision
    {

        //////////////////////////////////////////////////////////////////////////////////
        // Static methods
        //////////////////////////////////////////////////////////////////////////////////
        public static CollisionResult AABB(Maths.Vector2 positionA, Maths.Vector2 sizeA, Maths.Vector2 positionB, Maths.Vector2 sizeB)
        // TODO: Return multiple sides? with weight?
        {
            float overlapLeft = (positionA.X + sizeA.X) - positionB.X;   // A right - B left
            float overlapRight = (positionB.X + sizeB.X) - positionA.X; // B right - A left
            float overlapTop = (positionA.Y + sizeA.Y) - positionB.Y;   // A bottom - B top
            float overlapBottom = (positionB.Y + sizeB.Y) - positionA.Y; // B bottom - A top
            
            CollisionResult result = new CollisionResult { Side = CollisionSide.None, Overlap = 0.0f };
            
            if (overlapLeft > 0 && overlapRight > 0 && overlapTop > 0 && overlapBottom > 0)
            {
                float minX = Math.Min(overlapLeft, overlapRight);
                float minY = Math.Min(overlapTop, overlapBottom);
            
                if (minX < minY)
                {
                    result.Overlap = minX;
                    result.Side = (overlapLeft < overlapRight) ? CollisionSide.Right : CollisionSide.Left;
                }
                else
                {
                    result.Overlap = minY;
                    result.Side = (overlapTop < overlapBottom) ? CollisionSide.Top : CollisionSide.Bottom;
                }
            }

            return result;
        }

    }

}