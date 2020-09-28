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
    public class PropertyDescriptorBuilder
    {
        #region Ctor

        public PropertyDescriptorBuilder()
        {
        }

        #endregion Ctor

        #region Methods

        #region Methods - Public

        public void PostPropertyValueChanged(PropertyDescriptor pd, string propertyName)
        {
            OnPropertyValueChanged(pd, propertyName);
        }

        #endregion Methods - Public

        #region Methods - Protected

        public virtual PropertyDescriptor Build(PropertyDescriptor defaultValue)
        {
            return defaultValue;
        }

        public virtual void OnPropertyValueChanged(PropertyDescriptor pd, string propertyName)
        {
        }

        #endregion Methods - Protected

        #endregion Methods
    }
}