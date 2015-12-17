using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;


namespace RangPaint.Model
{
    class DrawBrush : DrawStrokeBase
    {
        protected Point point;
        protected StylusPointCollection pts;

        public override void OnMouseDown(InkCanvas inkCanvas, System.Windows.Input.MouseButtonEventArgs e)
        {
            StrokeResult = null;
            point = e.GetPosition(inkCanvas);
            pts = new StylusPointCollection();
        }

        public override void OnMouseMove(InkCanvas inkCanvas, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var p = e.GetPosition(inkCanvas);
                if (p != point)
                {
                    point = p;
                    GetBrush(pts, (s) =>
                    {
                        if (StrokeResult != null)
                            inkCanvas.Strokes.Remove(StrokeResult);

                        DrawingAttributes drawingAttributes = new DrawingAttributes
                        {
                            Color = inkCanvas.DefaultDrawingAttributes.Color,
                            Width = inkCanvas.DefaultDrawingAttributes.Width,
                            StylusTip = StylusTip.Ellipse,
                            IgnorePressure = true,
                            FitToCurve = true
                        };

                        StrokeResult = new BrushStroke(s, drawingAttributes);
                        inkCanvas.Strokes.Add(StrokeResult);
                    }
                   );
                }
            }
        }

        void GetBrush(StylusPointCollection pts, Action<StylusPointCollection> exec)
        {
            pts.Add(new StylusPoint(point.X, point.Y));
            exec(pts);
        }
    }
}