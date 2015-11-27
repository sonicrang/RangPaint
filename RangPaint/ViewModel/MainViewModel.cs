using RangPaint.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace RangPaint.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        #region Member

        public InkCanvas inkCanvas { get; set; }

        public DrawStrokeBase curDraw { get; set; }

        private string mouseLocationText;

        public string MouseLocationText
        {
            get { return mouseLocationText; }
            set
            {
                mouseLocationText = value;
                OnPropertyChanged("MouseLocationText");
            }
        }


        #endregion

        #region Function

        public MainViewModel(InkCanvas _inkCanvas)
        {
            inkCanvas = _inkCanvas;
        }

        #region Clipboard

        public void Cut()
        {
          
        }

        public void Copy()
        {
            var lstStokes = inkCanvas.GetSelectedStrokes();
            Clipboard.Clear();
            foreach (var s in lstStokes)
            {
                string xmlString = XamlWriter.Save(s);
                DataObject rectangleData = new DataObject(DataFormats.Xaml, xmlString);
                Clipboard.SetDataObject(rectangleData);
            }
        }

        public void Paste(object sender, MouseButtonEventArgs e)
        {
            if (inkCanvas.CanPaste())
            {
                inkCanvas.Paste(e.GetPosition(inkCanvas));
            }
        }

        #endregion

        #region Image

        public void SelectMode()
        {
            ExitShapeMode();
            inkCanvas.EditingMode = InkCanvasEditingMode.Select;
        }

        public void RecSelectMode()
        {
            ExitShapeMode();
            inkCanvas.EditingMode = InkCanvasEditingMode.Select;
        }

        #endregion

        #region Tool
        public void TextMode()
        {
            ExitShapeMode();

        }

        public void PenMode()
        {
            ExitShapeMode();
            inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        public void BrushMode()
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.None;
            curDraw = new DrawBrush();
        }

        public void EraseByPointMode()
        {
            ExitShapeMode();
            inkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }

        public void EraseByStrokeMode()
        {
            ExitShapeMode();
            inkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
        }

        #endregion

        #region Shape
        public void DrawLine()
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.None;
            curDraw = new DrawLine();
        }

        public void DrawEllipse()
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.None;
            curDraw = new DrawEllipse(false, null);
        }
        public void DrawRectangle()
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.None;
            curDraw = new DrawRectangle(false, null);
        }

        public void DrawTriangle()
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.None;
            curDraw = new DrawRectangle(false, null);
        }

        #endregion

        private void ExitShapeMode()
        {
            curDraw = null;
        }


        public void CanvasPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (curDraw != null)
            {
                curDraw.OnMouseDown(inkCanvas, e);
            }
        }


        public void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(inkCanvas);
            if (point.X >= 0 && point.Y >= 0)
            {
                MouseLocationText = point.X + "," + point.Y + " 像素";
            }
            else
            {
                MouseLocationText = "";
            }

            if (e.LeftButton == MouseButtonState.Pressed && curDraw != null)
            {
                inkCanvas.EditingMode = InkCanvasEditingMode.None;
                curDraw.OnMouseMove(inkCanvas, e);
            }
        }

        public void CanvasMouseLeave(object sender, MouseEventArgs e)
        {
            MouseLocationText = "";
        }

        public void CanvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (curDraw != null)
            {
                curDraw.OnMouseUp(inkCanvas, e);
            }
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}
