using RangPaint.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Resources;

namespace RangPaint.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        #region Member

        public InkCanvas inkCanvas { get; set; }     //InkCanvas
        public DrawStrokeBase curDraw { get; set; }  //current draw stroke

        private StrokeCollection lstStrokeClipBoard; //clipboard of strokes

        private Point? selectionStartPoint = null;   //rectangle_select start point

        private DoCommandStack doCmdStack;           //redo undo command stack

        private int editingOperationCount;           //redo undo command count

        private bool isDraw;                         // is drawing stroke

        private bool isColorPickerMode;              // is ColorPicker 

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

        private string fieldSizeText;                //show stroke field size

        public string FieldSizeText
        {
            get { return fieldSizeText; }
            set
            {
                fieldSizeText = value;
                OnPropertyChanged("FieldSizeText");
            }
        }

        private string canvasSizeText;                //show canvas size

        public string CanvasSizeText
        {
            get { return canvasSizeText; }
            set
            {
                canvasSizeText = value;
                OnPropertyChanged("CanvasSizeText");
            }
        }

        private string fileSizeText;                  //show file size

        public string FileSizeText
        {
            get { return fileSizeText; }
            set
            {
                fileSizeText = value;
                OnPropertyChanged("FileSizeText");
            }
        }

        private Visibility isSavedVisible = Visibility.Collapsed;   //is saved visibillity

        public Visibility IsSavedVisible
        {
            get { return isSavedVisible; }
            set
            {
                isSavedVisible = value;
                OnPropertyChanged("IsSavedVisible");
            }
        }
        #endregion

        #region Function

        public MainViewModel(InkCanvas _inkCanvas)
        {
            inkCanvas = _inkCanvas;
            inkCanvas.PreviewMouseLeftButtonDown += CanvasMouseDown;
            inkCanvas.MouseMove += CanvasMouseMove;
            inkCanvas.MouseLeave += CanvasMouseLeave;
            inkCanvas.MouseUp += CanvasMouseUp;
            inkCanvas.SelectionMoving += Canvas_SelectionMovingOrResizing;
            inkCanvas.SelectionResizing += Canvas_SelectionMovingOrResizing;
            inkCanvas.SizeChanged += InkCanvas_SizeChanged;

            doCmdStack = new DoCommandStack(_inkCanvas.Strokes);
            lstStrokeClipBoard = new StrokeCollection();
        }

        #region Command
        public void Delete()
        {
            var lstStokes = inkCanvas.GetSelectedStrokes();
            if (lstStokes.Count > 0)
            {
                editingOperationCount++;
                Strokes_Removed(lstStokes);
                inkCanvas.Strokes.Remove(lstStokes);
            }
        }
        public void Cut()
        {
            var lstStokes = inkCanvas.GetSelectedStrokes();
            if (lstStokes.Count > 0)
            {
                editingOperationCount++;
                lstStrokeClipBoard.Clear();
                lstStrokeClipBoard.Add(lstStokes);
                Strokes_Removed(lstStokes);
                inkCanvas.Strokes.Remove(lstStokes);
            }
        }

        public void Copy()
        {
            var lstStokes = inkCanvas.GetSelectedStrokes();
            if (lstStokes.Count > 0)
            {
                editingOperationCount++;
                lstStrokeClipBoard.Clear();
                lstStrokeClipBoard.Add(lstStokes);
            }
        }


        public void Paste()
        {
            if (lstStrokeClipBoard.Count > 0)
            {
                editingOperationCount++;
                var newLstStrokes = lstStrokeClipBoard.Clone();
                Strokes_Added(newLstStrokes);
                inkCanvas.Strokes.Add(newLstStrokes);
                inkCanvas.Select(newLstStrokes);
            }
        }

        public void Undo()
        {
            doCmdStack.Undo();
        }

        /// <summary>
        /// Redo the last edit.
        /// </summary> 
        public void Redo()
        {
            doCmdStack.Redo();
        }

        #endregion

        #region Tool

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

        public void TextMode()
        {
            // curDraw = new DrawPen();
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

        public void ColorPickerMode()
        {
            ExitShapeMode();
            inkCanvas.EditingMode = InkCanvasEditingMode.None;
            isColorPickerMode = true;
        }

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

        private void ExitShapeMode()
        {
            curDraw = null;
        }


        #endregion

        #region Event
        public void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.C) && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Copy();
            }
            else if (e.KeyStates == Keyboard.GetKeyStates(Key.X) && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Cut();
            }
            else if (e.KeyStates == Keyboard.GetKeyStates(Key.V) && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Paste();
            }
            else if (e.KeyStates == Keyboard.GetKeyStates(Key.Y) && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Redo();
            }
            else if (e.KeyStates == Keyboard.GetKeyStates(Key.Z) && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Undo();
            }
            else if (e.KeyStates == Keyboard.GetKeyStates(Key.Delete))
            {
                Delete();
            }
            e.Handled = true;
        }
        private void Strokes_Added(StrokeCollection lstAdded)
        {
            CommandItem item = new StrokesAddedOrRemovedCI(doCmdStack, inkCanvas.EditingMode, lstAdded, new StrokeCollection(), editingOperationCount);
            doCmdStack.Enqueue(item);
        }

        private void Strokes_Removed(StrokeCollection lstRemoved)
        {
            CommandItem item = new StrokesAddedOrRemovedCI(doCmdStack, inkCanvas.EditingMode, new StrokeCollection(), lstRemoved, editingOperationCount);
            doCmdStack.Enqueue(item);
        }

        private void InkCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CanvasSizeText = inkCanvas.ActualWidth + "," + inkCanvas.ActualHeight + " 像素";
        }

        private void Canvas_SelectionMovingOrResizing(object sender, InkCanvasSelectionEditingEventArgs e)
        {
            Rect newRect = e.NewRectangle; Rect oldRect = e.OldRectangle;

            if (newRect.Top < 0d || newRect.Left < 0d)
            {
                Rect newRect2 =
                    new Rect(newRect.Left < 0d ? 0d : newRect.Left,
                                newRect.Top < 0d ? 0d : newRect.Top,
                                newRect.Width,
                                newRect.Height);

                e.NewRectangle = newRect2;
            }
            CommandItem item = new SelectionMovedOrResizedCI(doCmdStack, inkCanvas.GetSelectedStrokes(), newRect, oldRect, editingOperationCount);
            doCmdStack.Enqueue(item);
        }

        private void CanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (curDraw != null)
            {
                curDraw.OnMouseDown(inkCanvas, e);
            }

            if(isColorPickerMode)
            {
                isColorPickerMode = false;
                Mouse.OverrideCursor = Cursors.Arrow;

                POINT p;
                GetCursorPos(out p);
                IntPtr hdc = GetDC(IntPtr.Zero);
                int c = GetPixel(hdc, p.X, p.Y); 
                byte r = (byte)(c & 0xFF);
                byte g = (byte)((c & 0xFF00) >> 8);
                byte b = (byte)((c & 0xFF0000) >> 16);

             
                // set color
            }
        }
        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(inkCanvas);
            if (point.X >= 0 && point.Y >= 0)
            {
                MouseLocationText = (int)point.X + "," + (int)point.Y + " 像素";
            }
            else
            {
                MouseLocationText = "";
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (curDraw != null && inkCanvas.GetSelectedStrokes().Count == 0)
                {
                    isDraw = true;
                    inkCanvas.EditingMode = InkCanvasEditingMode.None;
                    curDraw.OnMouseMove(inkCanvas, e);
                }
                else
                {
                    isDraw = false;
                }
            }

            if(isColorPickerMode)
            {
                StreamResourceInfo sri = Application.GetResourceStream(new Uri("/RangPaint;component/Images/color_cursor.cur", UriKind.Relative));
                Cursor customCursor = new Cursor(sri.Stream);
                Mouse.OverrideCursor = customCursor;
            }
        }
        private void CanvasMouseLeave(object sender, MouseEventArgs e)
        {
            MouseLocationText = "";

            if (isColorPickerMode)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
        private void CanvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (curDraw != null && isDraw)
            {
                editingOperationCount++;
                curDraw.OnMouseUp(inkCanvas, e);
                var lstStrokes = new StrokeCollection() { curDraw.StrokeResult };
                inkCanvas.Select(lstStrokes);
                Strokes_Added(lstStrokes);
                isDraw = false;
            }
            else if (inkCanvas.EditingMode == InkCanvasEditingMode.Ink)
            {
                editingOperationCount++;
                Strokes_Added(new StrokeCollection() { inkCanvas.Strokes[inkCanvas.Strokes.Count - 1] });
            }
        }

        #endregion

        #endregion

        #region win API

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);

        [DllImport("user32.dll")]//取设备场景
        private static extern IntPtr GetDC(IntPtr hwnd);//返回设备场景句柄
        [DllImport("gdi32.dll")]//取指定点颜色
        private static extern int GetPixel(IntPtr hdc, int nXPos, int nYPos);


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
