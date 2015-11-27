using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Ink;
using System.Windows.Input;
using System.Reflection;

namespace RangPaint.Model
{
    public class StrokeBase : Stroke
    {

        protected PathGeometry pathGeomery = null;

        public StrokeBase(StylusPointCollection pts, DrawingAttributes da)
            : base(pts, da)
        {
            this.StylusPoints = pts;
            this.DrawingAttributes = da;
        }

        public StrokeBase()
            : base(null, null)
        {

        }

        public bool IsSelected
        {
            get
            {
                PropertyInfo property = typeof(Stroke).GetProperty("IsSelected",
                   System.Reflection.BindingFlags.Instance |
                   System.Reflection.BindingFlags.GetProperty |
                   System.Reflection.BindingFlags.NonPublic);
                return (bool)property.GetValue(this, null);
            }
        }



        private double lineWidth = 1.0;

        public double LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }

        private Brush foreGround = Brushes.Black;

        public Brush ForeGround
        {
            get { return foreGround; }
            set { foreGround = value; }
        }

        private Brush backGround = null;

        public Brush BackGround
        {
            get { return backGround; }
            set { backGround = value; }
        }

    }
}
