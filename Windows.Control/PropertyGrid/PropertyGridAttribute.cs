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
    public class PropertyGridAttribute : Attribute
    {
        #region Properties

        public int Row { get; set; }
        public int Column { get; set; }

        #endregion Properties

        #region Ctor

        public PropertyGridAttribute()
        {
            Column = 2;
        }

        #endregion Ctor
    }
}