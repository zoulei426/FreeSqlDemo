using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    [AttributeUsage(AttributeTargets.All)]
    public class EnabledAttribute : Attribute
    {
        #region Properties

        public bool Value { get; set; }

        #endregion Properties

        #region Ctor

        public EnabledAttribute(bool value)
        {
            Value = value;
        }

        #endregion Ctor
    }
}