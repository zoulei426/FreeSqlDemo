using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Windows.Control
{
    public class InitializePropertyDescriptorEventArgs : EventArgs
    {
        #region Properties

        public PropertyDescriptor PropertyDescriptor { get; set; }
        public bool Cancel { get; set; }

        #endregion

        #region Ctor

        public InitializePropertyDescriptorEventArgs(PropertyDescriptor property)
        {
            PropertyDescriptor = property;
            Cancel = true;
        }

        #endregion
    }
}
