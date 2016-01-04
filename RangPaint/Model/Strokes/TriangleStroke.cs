using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace RangPaint.Model
{
    class TriangleStroke : StrokeBase
    {
        public TriangleStroke(StylusPointCollection pts, DrawingAttributes da)
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

            BrushConverter bc = new BrushConverter();
            Brush BackGround = (Brush)bc.ConvertFromString(drawingAttributes.GetPropertyData(DrawAttributesGuid.BackgroundColor).ToString());
            GeometryConverter gc = new GeometryConverter();
            Geometry geometry = (Geometry)gc.ConvertFromString(string.Format("M {0},{1} {2},{3} {4},{5} Z", StylusPoints[0].X, StylusPoints[1].Y, (Math.Abs(StylusPoints[1].X - StylusPoints[0].X))/2 + StylusPoints[0].X, StylusPoints[0].Y, StylusPoints[1].X, StylusPoints[1].Y));
            GeometryDrawing gd = new GeometryDrawing(BackGround, pen, geometry);
            drawingContext.DrawDrawing(gd);
        }
    }
}
