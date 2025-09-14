using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Puzzled
{

    ////////////////////////////////////////////////////////////////////////////////////
    // Input
    ////////////////////////////////////////////////////////////////////////////////////
    public class Input
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Static methods
        ////////////////////////////////////////////////////////////////////////////////////
        public static bool IsKeyPressed(Key key)
        {
            return Keyboard.IsKeyDown(key);
        }

        public static bool IsMousePressed(MouseButton button)
        {
            switch (button)
            {
            case MouseButton.Left:          return (Mouse.LeftButton == MouseButtonState.Pressed);
            case MouseButton.Right:         return (Mouse.RightButton == MouseButtonState.Pressed);
            case MouseButton.Middle:        return (Mouse.MiddleButton == MouseButtonState.Pressed);
            case MouseButton.XButton1:      return (Mouse.XButton1 == MouseButtonState.Pressed);
            case MouseButton.XButton2:      return (Mouse.XButton2 == MouseButtonState.Pressed);

            default:
                break;
            }

            return false;
        }

        public static Maths.Vector2 GetMousePosition()
        {
            return new Maths.Vector2(Mouse.GetPosition(Game.Instance.Window));
        }

    }

}