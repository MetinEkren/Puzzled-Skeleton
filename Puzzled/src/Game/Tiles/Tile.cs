using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Puzzled
{

    //////////////////////////////////////////////////////////////////////////////////
    // Tile
    //////////////////////////////////////////////////////////////////////////////////
    public interface ITile // Note: Tile size is 8x8
    {
        void RenderTo(Renderer renderer);
    }

}