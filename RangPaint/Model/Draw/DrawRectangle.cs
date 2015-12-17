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
            StrokeResult = null;
            topLeft = e.GetPosition(inkCanvas);
        }

        public override void OnMouseMove(System.Windows.Controls.InkCanvas inkCanvas, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                bottomRight = e.GetPosition(inkCanvas);
                if(topLeft != bottomRight)
                {
                    StylusPointCollection pts = new StylusPointCollection();
                    GetRectangle(pts, (s) =>
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

                        StrokeResult = new RectangleStroke(s, drawingAttributes);
                        StrokeResult.BackGround = backGround;
                        inkCanvas.Strokes.Add(StrokeResult);
                    }
                    );
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
