using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// <summary>
    /// RectSelectModeAdorner
    /// </summary>
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
            this.endPoint = Mouse.GetPosition(inkCanvas);

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

            e.Handled = true;
        }

        protected override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.IsMouseCaptured) this.ReleaseMouseCapture();

            // remove adorner layer
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.inkCanvas);
            if (adornerLayer != null)
            {
                adornerLayer.Remove(this);
            }
            e.Handled = true;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

            if (this.startPoint.HasValue && this.endPoint.HasValue)
                dc.DrawRectangle(Brushes.Transparent, pen, new Rect(this.startPoint.Value, this.endPoint.Value));
        }

        /// <summary>
        /// Update Selected Strokes
        /// </summary>
        private void UpdateSelection()
        {
            Rect rubberBand = new Rect(startPoint.Value, endPoint.Value);
            StrokeCollection lstSelectedStrokes = new StrokeCollection();
            foreach (var item in inkCanvas.Strokes)
            {
                Rect itemRect = item.GetBounds();
                if (rubberBand.Contains(itemRect))
                {
                    lstSelectedStrokes.Add(item);
                }
            }

            inkCanvas.Select(lstSelectedStrokes);

            if (lstSelectedStrokes.Count == 0)
                inkCanvas.EditingMode = InkCanvasEditingMode.None;
        }
    }
}
