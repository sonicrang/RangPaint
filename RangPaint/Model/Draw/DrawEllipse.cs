using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;

namespace RangPaint.Model
{
    class DrawEllipse : DrawStrokeBase
    {
        protected Point topLeft;
        protected Point bottomRight;
        protected EllipseStroke drawEllipse;
        protected Brush backGround;

        public DrawEllipse(bool isFill, Brush backGround)
        {
            if (isFill)
            {
                this.backGround = backGround;
            }
        }

        public override void OnMouseDown(System.Windows.Controls.InkCanvas inkCanvas, System.Windows.Input.MouseButtonEventArgs e)
        {
            drawEllipse = null;
            topLeft = e.GetPosition(inkCanvas);
        }

        public override void OnMouseMove(System.Windows.Controls.InkCanvas inkCanvas, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                bottomRight = e.GetPosition(inkCanvas);

                StylusPointCollection pts = new StylusPointCollection();
                GetEllipse(pts, (s) =>
                {
                    if (drawEllipse != null)
                        inkCanvas.Strokes.Remove(drawEllipse);

                    DrawingAttributes drawingAttributes = new DrawingAttributes
                    {
                        Color = inkCanvas.DefaultDrawingAttributes.Color,
                        Width = inkCanvas.DefaultDrawingAttributes.Width,
                        StylusTip = StylusTip.Ellipse,
                        IgnorePressure = true,
                        FitToCurve = true
                    };

                    drawEllipse = new EllipseStroke(s, drawingAttributes);
                    drawEllipse.BackGround = backGround;
                    inkCanvas.Strokes.Add(drawEllipse);
                }
                );
            }
        }

        public override void OnMouseUp(System.Windows.Controls.InkCanvas inkCanvas, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (drawEllipse != null)
            {
                inkCanvas.Strokes.Remove(drawEllipse);
                if (inkCanvas.EditingMode != InkCanvasEditingMode.EraseByPoint && inkCanvas.EditingMode != InkCanvasEditingMode.EraseByStroke)
                {
                    var stroke = drawEllipse.Clone();
                    inkCanvas.Strokes.Add(stroke);
                    StrokeResult = stroke;
                }
            }
        }

        void GetEllipse(StylusPointCollection pts, Action<StylusPointCollection> exec)
        {
            pts.Add(new StylusPoint(topLeft.X, topLeft.Y));
            pts.Add(new StylusPoint(bottomRight.X, bottomRight.Y));

            exec(pts);
        }
    }
}
