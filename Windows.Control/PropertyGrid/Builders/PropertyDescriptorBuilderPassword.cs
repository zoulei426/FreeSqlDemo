using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace Windows.Control
{
    public class PropertyDescriptorBuilderPassword : PropertyDescriptorBuilder
    {
        #region Methods

        public override PropertyDescriptor Build(PropertyDescriptor defaultValue)
        {
            defaultValue.Designer.Dispatcher.Invoke(new Action(() =>
            {
                var b = new Binding("Value");
                b.Source = defaultValue;

                var designer = new PasswordBox();
                designer.BorderThickness = new System.Windows.Thickness(1);
                designer.Padding = new System.Windows.Thickness(4, 6, 4, 4);
                designer.Password = defaultValue.Value as string;
                designer.PasswordChanged += (s, e) => defaultValue.Value = designer.Password;
                defaultValue.Designer = designer;
            }));

            return defaultValue;
        }


        #endregion
    }
}
