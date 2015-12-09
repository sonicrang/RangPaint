using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace RangPaint.Controls
{
    public class ResizeThumb : Thumb
    {
        public ResizeThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.ResizeThumb_DragDelta);
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as ResizeThumb;
            if (thumb != null)
            {
                Grid item = thumb.Parent as Grid;

                item.Height = item.ActualHeight + (int)e.VerticalChange;
                item.Width = item.ActualWidth + (int)e.HorizontalChange;
            }

            e.Handled = true;
        }
    }
}
