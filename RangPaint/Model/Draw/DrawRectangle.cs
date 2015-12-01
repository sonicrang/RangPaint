using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;

namespace RangPaint.Model
{
    class DrawRectangle : DrawStrokeBase
    {
        protected Point topLeft;
        protected Point bottomRight;
        protected RectangleStroke drawRectangle;
        protected Brush backGround;

        public DrawRectangle(bool isFill, Brush backGround)
        {
            if (isFill)
            {
                this.backGround = backGround;
            }
        }


        public override void OnMouseDown(System.Windows.Controls.InkCanvas inkCanvas, System.Windows.Input.MouseButtonEventArgs e)
        {
            drawRectangle = null;
            topLeft = e.GetPosition(inkCanvas);
        }

        public override void OnMouseMove(System.Windows.Controls.InkCanvas inkCanvas, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                bottomRight = e.GetPosition(inkCanvas);

                StylusPointCollection pts = new StylusPointCollection();
                GetRectangle(pts, (s) =>
                {
                    if (drawRectangle != null)
                        inkCanvas.Strokes.Remove(drawRectangle);

                    DrawingAttributes drawingAttributes = new DrawingAttributes
                    {
                        Color = inkCanvas.DefaultDrawingAttributes.Color,
                        Width = inkCanvas.DefaultDrawingAttributes.Width,
                        StylusTip = StylusTip.Ellipse,
                        IgnorePressure = true,
                        FitToCurve = true
                    };

                    drawRectangle = new RectangleStroke(s, drawingAttributes);
                    drawRectangle.BackGround = backGround;
                    inkCanvas.Strokes.Add(drawRectangle);
                }
                );
            }
        }

        public override void OnMouseUp(System.Windows.Controls.InkCanvas inkCanvas, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (drawRectangle != null)
            {
                inkCanvas.Strokes.Remove(drawRectangle);
                if (inkCanvas.EditingMode != InkCanvasEditingMode.EraseByPoint && inkCanvas.EditingMode != InkCanvasEditingMode.EraseByStroke)
                {
                    var stroke = drawRectangle.Clone();
                    inkCanvas.Strokes.Add(stroke);
                    inkCanvas.Select(new StrokeCollection() { stroke });
                }
            }
        }

        void GetRectangle(StylusPointCollection pts, Action<StylusPointCollection> exec)
        {
            pts.Add(new StylusPoint(topLeft.X, topLeft.Y));
            pts.Add(new StylusPoint(bottomRight.X, bottomRight.Y));

            exec(pts);
        }
    }
}
