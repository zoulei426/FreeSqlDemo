using System;
using System.Collections.Generic;
using System.Text;

namespace Windows.Core
{
    /// <summary>
    /// Defines the InputDirection of a grid (the direction the current cell is moved when Enter is pressed)
    /// </summary>
    public enum InputDirection
    {
        /// <summary>
        /// Move horizontally.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Move vertically.
        /// </summary>
        Vertical
    }
}