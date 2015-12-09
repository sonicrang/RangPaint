using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace RangPaint.Controls
{
    class SelectAdorner : Adorner
    {
        private Point? startPoint;
        private Point? endPoint;
        private Pen pen;

        private InkCanvas inkCanvas;

        public SelectAdorner(InkCanvas inkCanvas, Point? dragStartPoint)
            : base(inkCanvas)
        {
            this.inkCanvas = inkCanvas;
            this.startPoint = dragStartPoint;
            pen = new Pen(Brushes.LightSlateGray, 1);
            pen.DashStyle = new DashStyle(new double[] { 2 }, 1);
        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)

        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!this.IsMouseCaptured)
                    this.CaptureMouse();

                endPoint = e.GetPosition(inkCanvas);
                UpdateSelection();
                this.InvalidateVisual();
            }
            else
            {
                if (this.IsMouseCaptured) this.ReleaseMouseCapture();
            }

            //e.Handled = true;
        }

        protected override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            // release mouse capture
            if (this.IsMouseCaptured) this.ReleaseMouseCapture();

            // remove this adorner from adorner layer
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.inkCanvas);
            if (adornerLayer != null)
                adornerLayer.Remove(this);

            //e.Handled = true;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            // without a background the OnMouseMove event would not be fired!
            // Alternative: implement a Canvas as a child of this adorner, like
            // the ConnectionAdorner does.
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

            if (this.startPoint.HasValue && this.endPoint.HasValue)
                dc.DrawRectangle(Brushes.Transparent, pen, new Rect(this.startPoint.Value, this.endPoint.Value));
        }

        private void UpdateSelection()
        {
            Rect rubberBand = new Rect(startPoint.Value, endPoint.Value);
            var lstSelectedStrokes = new List<Stroke>();
            foreach (var item in inkCanvas.Strokes)
            {
                Rect itemRect = item.GetBounds();
                if (rubberBand.Contains(itemRect))
                {
                    lstSelectedStrokes.Add(item);
                }
            }

            inkCanvas.Select(new StrokeCollection(lstSelectedStrokes));
        }
    }
}
