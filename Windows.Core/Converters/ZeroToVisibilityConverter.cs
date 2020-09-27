﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Windows.Core
{
    /// <summary>
    /// Converts <see cref="int" /> instances to <see cref="Visibility" /> instances.
    /// </summary>
    [ValueConversion(typeof(int), typeof(Visibility))]
    public class ZeroToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZeroToVisibilityConverter" /> class.
        /// Initializes a new instance of the <see cref="NullToVisibilityConverter" /> class.
        /// </summary>
        public ZeroToVisibilityConverter()
        {
            this.ZeroVisibility = Visibility.Collapsed;
            this.NotZeroVisibility = Visibility.Visible;
        }

        /// <summary>
        /// Gets or sets the not <c>null</c> visibility.
        /// </summary>
        /// <value>The not <c>null</c> visibility.</value>
        public Visibility NotZeroVisibility { get; set; }

        /// <summary>
        /// Gets or sets the <c>null</c> visibility.
        /// </summary>
        /// <value>The <c>null</c> visibility.</value>
        public Visibility ZeroVisibility { get; set; }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                var i = (int)value;
                if (i == 0)
                {
                    return this.ZeroVisibility;
                }
            }

            return this.NotZeroVisibility;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}