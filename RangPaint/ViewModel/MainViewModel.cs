using RangPaint.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;

namespace RangPaint.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        #region Member

        public InkCanvas inkCanvas { get; set; }     //InkCanvas

        public DrawStrokeBase curDraw { get; set; }  //current draw stroke

        private List<Stroke> lstStrokeClipBoard;     //clipboard of strokes

        private Point? selectionStartPoint = null;   //rectangle_select start point

        private string mouseLocationText;            //show mouse location
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
            lstStrokeClipBoard = new List<Stroke>();
        }

        #region Clipboard

        public void Cut()
        {
            var lstStokes = inkCanvas.GetSelectedStrokes();
            if (lstStokes.Count > 0)
            {
                lstStrokeClipBoard.Clear();
            }
            foreach (var s in lstStokes)
            {
                lstStrokeClipBoard.Add(s);
                inkCanvas.Strokes.Remove(s);
            }
        }

        public void Copy()
        {
            var lstStokes = inkCanvas.GetSelectedStrokes();
            if (lstStokes.Count > 0)
            {
                lstStrokeClipBoard.Clear();
            }
            foreach (var s in lstStokes)
            {
                lstStrokeClipBoard.Add(s);
            }
        }


        public void Paste()
        {
            foreach (var s in lstStrokeClipBoard)
            {
                inkCanvas.Strokes.Add(s.Clone());
            }
        }

        #endregion

        #region Image

        /// <summary>
        /// free select mode
        /// </summary>
        public void SelectMode()
        {
            ExitShapeMode();
            inkCanvas.EditingMode = InkCanvasEditingMode.Select;
        }

        /// <summary>
        /// rectangle seletc mode
        /// </summary>
        public void RecSelectMode()
        {
            ExitShapeMode();
            inkCanvas.EditingMode = InkCanvasEditingMode.None;
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
            curDraw = new DrawBrush();
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
            curDraw = new DrawLine();
        }

        public void DrawEllipse()
        {
            curDraw = new DrawEllipse(false, null);
        }
        public void DrawRectangle()
        {
            curDraw = new DrawRectangle(false, null);
        }

        public void DrawTriangle()
        {

            curDraw = new DrawRectangle(false, null);
        }

        #endregion

        private void ExitShapeMode()
        {
            curDraw = null;
        }


        public void CanvasMouseDown(object sender, MouseButtonEventArgs e)
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

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (curDraw != null && inkCanvas.GetSelectedStrokes().Count == 0)
                {
                    inkCanvas.EditingMode = InkCanvasEditingMode.None;
                    curDraw.OnMouseMove(inkCanvas, e);
                }
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
