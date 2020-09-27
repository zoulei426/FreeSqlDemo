using System;
using System.Collections.Generic;
using System.Text;

namespace Windows.Core
{
    /// <summary>
    /// Specifies the layout for checkboxes.
    /// </summary>
    public enum CheckBoxLayout
    {
        /// <summary>
        /// Show the header, then the check box without content
        /// </summary>
        Header,

        /// <summary>
        /// Hide the header, show the check box with the display name as content
        /// </summary>
        HideHeader,

        /// <summary>
        /// Collapse the header, show the check box with the display name as content
        /// </summary>
        CollapseHeader
    }
}