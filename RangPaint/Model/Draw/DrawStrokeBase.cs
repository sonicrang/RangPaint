using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Ink;

namespace RangPaint.Model
{
    public abstract class DrawStrokeBase
    {
        public abstract void OnMouseDown(InkCanvas inkCanvas, MouseButtonEventArgs e);

        public abstract void OnMouseMove(InkCanvas inkCanvas, MouseEventArgs e);

        public StrokeBase StrokeResult { get; set; }

    }
}
