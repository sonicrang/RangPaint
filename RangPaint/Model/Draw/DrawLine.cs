using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;


namespace RangPaint.Model
{
    public class DrawLine : DrawStrokeBase
    {
        protected Point startPoint;
        protected Point endPoint;

        public override void OnMouseDown(InkCanvas inkCanvas, System.Windows.Input.MouseButtonEventArgs e)
        {
            StrokeResult = null;
            startPoint = e.GetPosition(inkCanvas);
        }

        public override void OnMouseMove(InkCanvas inkCanvas, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                endPoint = e.GetPosition(inkCanvas);
                if(startPoint != endPoint)
                {
                    StylusPointCollection pts = new StylusPointCollection();
                    GetLine(pts, (s) =>
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

                        StrokeResult = new LineStroke(s, drawingAttributes);
                        inkCanvas.Strokes.Add(StrokeResult);
                    }
                    );
                }
            }
        }

        void GetLine(StylusPointCollection pts, Action<StylusPointCollection> exec)
        {
            pts.Add(new StylusPoint(startPoint.X, startPoint.Y));
            pts.Add(new StylusPoint(endPoint.X, endPoint.Y));
            exec(pts);
        }
    }
}
