using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace RangPaint.Model
{
    class BrushStroke : StrokeBase
    {

        public BrushStroke(StylusPointCollection pts, DrawingAttributes da)
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
            Color color = drawingAttributes.Color;
            Pen pen = new Pen
            {
                Brush = new RadialGradientBrush(Color.FromArgb(0x88, color.R, color.B, color.G), Color.FromArgb(0x00, color.R, color.B, color.G)), 
                Thickness = drawingAttributes.Width * 20,     
                StartLineCap = PenLineCap.Round,
                EndLineCap = PenLineCap.Round
            };
            for (int i = 1; i < StylusPoints.Count; i++)
            {
                drawingContext.DrawEllipse(null, pen, (Point)StylusPoints[i], 1, 1);
            }
        }
    }
}
