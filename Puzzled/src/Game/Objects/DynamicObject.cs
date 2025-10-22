using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Diagnostics;

namespace Puzzled
{

    //////////////////////////////////////////////////////////////////////////////////
    // DynamicObject
    //////////////////////////////////////////////////////////////////////////////////
    public class DynamicObject
    {
        public virtual void Update(float deltaTime) { }
        public virtual void RenderTo(Renderer renderer, bool debug = false) { }
    }


}