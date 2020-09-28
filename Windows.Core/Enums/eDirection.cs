using System;
using System.Collections.Generic;
using System.Text;

namespace Windows.Core
{
    public enum eDirection
    {
        Left,
        Top,
        Right,
        Bottom,
    }

    public enum eCloseReason
    {
        ESC,
        Cancel,
        Enter,
        Confirm,
    }
}