using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Windows.Core;

namespace Windows.Control
{
    public class PropertyGridColumnProxy : NotifyDependencyCDObject
    {
        #region Properties

        public Visibility DefaultValueColumnVisibility
        {
            get { return (Visibility)GetValue(DefaultValueColumnVisibilityProperty); }
            set { SetValue(DefaultValueColumnVisibilityProperty, value); }
        }

        public static readonly DependencyProperty DefaultValueColumnVisibilityProperty =
            DependencyProperty.Register("DefaultValueColumnVisibility", typeof(Visibility), typeof(PropertyGridColumnProxy), new PropertyMetadata(Visibility.Collapsed));

        public Visibility StateColumnVisibility
        {
            get { return (Visibility)GetValue(StateColumnVisibilityProperty); }
            set { SetValue(StateColumnVisibilityProperty, value); }
        }

        public static readonly DependencyProperty StateColumnVisibilityProperty =
            DependencyProperty.Register("StateColumnVisibility", typeof(Visibility), typeof(PropertyGridColumnProxy), new PropertyMetadata(Visibility.Collapsed));

        #endregion Properties

        #region Ctor

        public PropertyGridColumnProxy()
        {
        }

        #endregion Ctor
    }
}