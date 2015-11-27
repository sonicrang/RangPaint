using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows;
using System.Windows.Shapes;

namespace RangPaint.Model
{
    public class ArrowLineStroke : StrokeBase
    {
        public ArrowLineStroke(StylusPointCollection pts, DrawingAttributes da)
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

            drawingContext.DrawLine(pen,
                new Point(StylusPoints[0].X, StylusPoints[0].Y),
                new Point(StylusPoints[1].X, StylusPoints[1].Y));

            double HeadWidth = drawingAttributes.Width * 2;
            double HeadHeight = drawingAttributes.Width * 2;
            double theta = Math.Atan2(StylusPoints[0].Y - StylusPoints[1].Y, StylusPoints[0].X - StylusPoints[1].X);
            double sint = Math.Sin(theta);
            double cost = Math.Cos(theta);

            Point pt1 = new Point(
                StylusPoints[1].X + (HeadWidth * cost - HeadHeight * sint),
                StylusPoints[1].Y + (HeadWidth * sint + HeadHeight * cost));

            drawingContext.DrawLine(pen,
            new Point(StylusPoints[1].X, StylusPoints[1].Y),
            new Point(pt1.X, pt1.Y));

            Point pt2 = new Point(
                StylusPoints[1].X + (HeadWidth * cost + HeadHeight * sint),
                StylusPoints[1].Y - (HeadHeight * cost - HeadWidth * sint));

            drawingContext.DrawLine(pen,
            new Point(StylusPoints[1].X, StylusPoints[1].Y),
            new Point(pt2.X, pt2.Y));
        }
    }
}
