using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Windows.Control
{
    public class PropertyGridConverterParameterPair
    {
        #region Properties

        public PropertyGrid PropertyGrid { get; set; }
        public object ConverterParameter { get; set; }

        #endregion

        #region Ctor

        public PropertyGridConverterParameterPair(PropertyGrid pg, object param)
        {
            PropertyGrid = pg;
            ConverterParameter = param;
        }

        #endregion
    }
}
