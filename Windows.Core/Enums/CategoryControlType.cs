using System;
using System.Collections.Generic;
using System.Text;

namespace Windows.Core
{
    /// <summary>
    /// Specifies the control type for categories.
    /// </summary>
    public enum CategoryControlType
    {
        /// <summary>
        /// Group boxes.
        /// </summary>
        GroupBox,

        /// <summary>
        /// Expander controls.
        /// </summary>
        Expander,

        /// <summary>
        /// Content control. Remember to set the CategoryControlTemplate.
        /// </summary>
        Template
    }
}