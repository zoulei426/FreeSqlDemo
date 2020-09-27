using System;
using System.Collections.Generic;
using System.Text;

namespace Windows.Core
{
    /// <summary>
    /// Specifies the visibility of the tab strip.
    /// </summary>
    public enum TabVisibility
    {
        /// <summary>
        /// The tabs are visible.
        /// </summary>
        Visible,

        /// <summary>
        /// The tabs are visible if there is more than one tab.
        /// </summary>
        VisibleIfMoreThanOne,

        /// <summary>
        /// The tab strip is collapsed. The contents of the tab pages will be stacked vertically in the control.
        /// </summary>
        Collapsed
    }
}