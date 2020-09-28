using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Windows.Control
{
    public class GridDescriptorAttribute : Attribute
    {
        #region Properties

        public int Column { get; set; }

        #endregion Properties

        #region Ctor

        public GridDescriptorAttribute()
        {
            Column = 2;
        }

        #endregion Ctor
    }
}