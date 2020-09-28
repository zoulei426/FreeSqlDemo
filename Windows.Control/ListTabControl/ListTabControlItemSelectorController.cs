using Core;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Core;

namespace Windows.Control
{
    public class ListTabControlItemSelectorController : NotifyCDObject
    {
        #region Properties

        public double SelectorWidth
        {
            get { return _SelectorWidth; }
            private set { _SelectorWidth = value; NotifyPropertyChanged(() => SelectorWidth); }
        }

        private double _SelectorWidth;

        public double SelectorHeight
        {
            get { return _SelectorHeight; }
            set { _SelectorHeight = value; NotifyPropertyChanged(() => SelectorHeight); }
        }

        private double _SelectorHeight;

        public System.Windows.HorizontalAlignment HorizontalAlignment
        {
            get { return _HorizontalAlignment; }
            set { _HorizontalAlignment = value; NotifyPropertyChanged(() => HorizontalAlignment); }
        }

        private System.Windows.HorizontalAlignment _HorizontalAlignment;

        public System.Windows.VerticalAlignment VerticalAlignment
        {
            get { return _VerticalAlignment; }
            set { _VerticalAlignment = value; NotifyPropertyChanged(() => VerticalAlignment); }
        }

        private System.Windows.VerticalAlignment _VerticalAlignment;

        #endregion Properties

        #region Fields

        private ListTabControl tabControl;

        #endregion Fields

        #region Ctor

        public ListTabControlItemSelectorController(ListTabControl tabControl)
        {
            this.tabControl = tabControl;
        }

        #endregion Ctor

        #region Methods

        public void Refresh()
        {
            if (tabControl.SelectorVisibility != System.Windows.Visibility.Visible)
                return;

            if (tabControl.Direction == eDirection.Bottom ||
                tabControl.Direction == eDirection.Top)
            {
                SelectorHeight = tabControl.SelectorLength;
                SelectorWidth = double.NaN;
            }
            else
            {
                SelectorHeight = double.NaN;
                SelectorWidth = tabControl.SelectorLength;
            }

            if (tabControl.SelectorReverse && tabControl.Direction == eDirection.Top)
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            }
            else if (!tabControl.SelectorReverse && tabControl.Direction == eDirection.Top)
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top;
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            }
            else if (tabControl.SelectorReverse && tabControl.Direction == eDirection.Bottom)
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top;
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            }
            else if (!tabControl.SelectorReverse && tabControl.Direction == eDirection.Bottom)
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            }
            else if (tabControl.SelectorReverse && tabControl.Direction == eDirection.Left)
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            }
            else if (!tabControl.SelectorReverse && tabControl.Direction == eDirection.Left)
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            }
            else if (tabControl.SelectorReverse && tabControl.Direction == eDirection.Right)
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            }
            else if (!tabControl.SelectorReverse && tabControl.Direction == eDirection.Right)
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            }
        }

        #endregion Methods
    }
}