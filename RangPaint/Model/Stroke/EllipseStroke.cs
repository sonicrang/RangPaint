using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace RangPaint.Model
{
    class EllipseStroke : StrokeBase
    {

        public EllipseStroke(StylusPointCollection pts, DrawingAttributes da)
            : base(pts, da)
        {
            this.StylusPoints = pts;
            this.DrawingAttributes = da;
        }

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            if (drawingContext == null)
            {
                throw new ArgumentNullException("drawingContext");
            }
            if (null == drawingAttributes)
            {
                throw new ArgumentNullException("drawingAttributes");
            }
            Pen pen = new Pen
            {
                StartLineCap = PenLineCap.Round,
                EndLineCap = PenLineCap.Round,
                Brush = new SolidColorBrush(drawingAttributes.Color),
                Thickness = drawingAttributes.Width
            };
            Rect r = new Rect(
                new Point(StylusPoints[0].X, StylusPoints[0].Y),
                new Point(StylusPoints[1].X, StylusPoints[1].Y));

            Point center = new Point(
                 (r.Left + r.Right) / 2.0,
                 (r.Top + r.Bottom) / 2.0);

            double radiusX = (r.Right - r.Left) / 2.0;
            double radiusY = (r.Bottom - r.Top) / 2.0;


            drawingContext.DrawEllipse(
                BackGround,
                pen,
                center,
                radiusX,
                radiusY);
        }
    }
}
