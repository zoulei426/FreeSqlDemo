using System;
using System.Windows.Data;

namespace Windows.Core
{
    public class GuidToStringConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? null : value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            Guid id = Guid.Empty;

            if (Guid.TryParse(value as string, out id))
                return id;

            return null;
        }

        #endregion Methods
    }
}