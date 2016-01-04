using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RangPaint.Model
{
    public enum ModeEnum
    {
        None,
        Select,   
        Pen,
        Eraser,
        Draw
    }

    public enum ColorPickerModeEnum
    {
        True,
        False
    }

    public enum ColorModeEnum
    {
        Foreground,
        Background
    }
}
