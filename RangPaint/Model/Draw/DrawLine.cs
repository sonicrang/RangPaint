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

        protected LineStroke drawLine;

        public override void OnMouseDown(InkCanvas inkCanvas, System.Windows.Input.MouseButtonEventArgs e)
        {
            drawLine = null;
            startPoint = e.GetPosition(inkCanvas);
        }

        public override void OnMouseMove(InkCanvas inkCanvas, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                endPoint = e.GetPosition(inkCanvas);
                StylusPointCollection pts = new StylusPointCollection();
                GetLine(pts, (s) =>
                {
                    if (drawLine != null)
                        inkCanvas.Strokes.Remove(drawLine);

                    DrawingAttributes drawingAttributes = new DrawingAttributes
                    {
                        Color = inkCanvas.DefaultDrawingAttributes.Color,
                        Width = inkCanvas.DefaultDrawingAttributes.Width,
                        StylusTip = StylusTip.Ellipse,
                        IgnorePressure = true,
                        FitToCurve = true
                    };

                    drawLine = new LineStroke(s, drawingAttributes);
                    inkCanvas.Strokes.Add(drawLine);
                }
                );
            }
        }

        public override void OnMouseUp(InkCanvas inkCanvas, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (drawLine != null)
            {
                inkCanvas.Strokes.Remove(drawLine);
                if (inkCanvas.EditingMode != InkCanvasEditingMode.EraseByPoint && inkCanvas.EditingMode != InkCanvasEditingMode.EraseByStroke)
                {
                    var stroke = drawLine.Clone();
                    inkCanvas.Strokes.Add(stroke);
                    StrokeResult = stroke;
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
