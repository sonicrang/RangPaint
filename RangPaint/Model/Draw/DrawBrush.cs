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
        protected bool isDrawing;
        protected Point point;
        protected BrushStroke drawBrush;
        protected StylusPointCollection pts;

        public override void OnMouseDown(InkCanvas inkCanvas, System.Windows.Input.MouseButtonEventArgs e)
        {
            isDrawing = true;
            drawBrush = null;
            pts = new StylusPointCollection();
        }

        public override void OnMouseMove(InkCanvas inkCanvas, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && isDrawing)
            {
                point = e.GetPosition(inkCanvas);

                GetBrush(pts, (s) =>
                {
                    if (drawBrush != null)
                        inkCanvas.Strokes.Remove(drawBrush);

                    DrawingAttributes drawingAttributes = new DrawingAttributes
                    {
                        Color = inkCanvas.DefaultDrawingAttributes.Color,
                        Width = inkCanvas.DefaultDrawingAttributes.Width,
                        StylusTip = StylusTip.Ellipse,
                        IgnorePressure = true,
                        FitToCurve = true
                    };

                    drawBrush = new BrushStroke(s, drawingAttributes);
                    inkCanvas.Strokes.Add(drawBrush);
                }
                );
            }
        }

        public override void OnMouseUp(InkCanvas inkCanvas, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (drawBrush != null)
            {
                inkCanvas.Strokes.Remove(drawBrush);
                if (inkCanvas.EditingMode != InkCanvasEditingMode.EraseByPoint && inkCanvas.EditingMode != InkCanvasEditingMode.EraseByStroke)
                {
                    var stroke = drawBrush.Clone();
                    inkCanvas.Strokes.Add(stroke);
                }
            }
            isDrawing = false;
        }

        void GetBrush(StylusPointCollection pts, Action<StylusPointCollection> exec)
        {
            pts.Add(new StylusPoint(point.X, point.Y));
            exec(pts);
        }
    }
}