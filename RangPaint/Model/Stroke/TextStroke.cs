using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Ink;
using System.Windows;

namespace RangPaint.Model
{
    class TextStroke : StrokeBase
    {
        public delegate void DeleteDelegate();
        public event DeleteDelegate onDelete;

        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        private string textFontFamilyName;

        public string TextFontFamilyName
        {
            get { return textFontFamilyName; }
            set { textFontFamilyName = value; }
        }
        private FontStyle textFontStyle;

        public FontStyle TextFontStyle
        {
            get { return textFontStyle; }
            set { textFontStyle = value; }
        }
        private FontWeight textFontWeight;

        public FontWeight TextFontWeight
        {
            get { return textFontWeight; }
            set { textFontWeight = value; }
        }
        private FontStretch textFontStretch;

        public FontStretch TextFontStretch
        {
            get { return textFontStretch; }
            set { textFontStretch = value; }
        }
        private double textFontSize;

        public double TextFontSize
        {
            get { return textFontSize; }
            set { textFontSize = value; }
        }

        private Rect rect;

        public Rect Rect
        {
            get { return rect; }
            set { rect = value; }
        }
        private FormattedText formattedText;

        public TextStroke(StylusPointCollection pts, DrawingAttributes da)
            : base(pts, da)
        {
            this.StylusPoints = pts;
            this.DrawingAttributes = da;
        }

        public void delete()
        {
            if (onDelete != null)
                onDelete();
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
                Thickness = LineWidth
            };
            Rect = new Rect(new Point(StylusPoints[0].X, StylusPoints[0].Y), new Point(StylusPoints[1].X, StylusPoints[1].Y));

            CreateFormattedText();

            drawingContext.PushClip(new RectangleGeometry(rect));
            drawingContext.DrawText(formattedText, new Point(rect.Left, rect.Top));

            drawingContext.Pop();
            drawingContext.DrawRectangle(ForeGround, pen, Rect);
        }

        void CreateFormattedText()
        {
            if (String.IsNullOrEmpty(textFontFamilyName))
            {
                textFontFamilyName = "Tahoma";
            }

            if (text == null)
            {
                text = "";
            }

            if (textFontSize <= 0.0)
            {
                textFontSize = 12.0;
            }

            Typeface typeface = new Typeface(
                new FontFamily(textFontFamilyName),
                textFontStyle,
                textFontWeight,
                textFontStretch);

            formattedText = new FormattedText(
                text,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                typeface,
                textFontSize,
                ForeGround);
            
            formattedText.MaxTextWidth = rect.Width;
        }
    }
}
