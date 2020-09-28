using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Windows.Control
{
    public static class PropertyGridLayoutAttacher
    {
        #region Properties - ColumnSpacing

        public static double GetColumnSpacing(DependencyObject obj)
        {
            return (double)obj.GetValue(ColumnSpacingProperty);
        }

        public static void SetColumnSpacing(DependencyObject obj, double value)
        {
            obj.SetValue(ColumnSpacingProperty, value);
        }

        public static readonly DependencyProperty ColumnSpacingProperty =
            DependencyProperty.RegisterAttached("ColumnSpacing", typeof(double), typeof(PropertyGridLayoutAttacher), new PropertyMetadata(50.0, (s, a) =>
            {
            }));

        #endregion Properties - ColumnSpacing

        #region Properties - DetailsRowCount

        public static int GetDetailsRowCount(DependencyObject obj)
        {
            return (int)obj.GetValue(DetailsRowCountProperty);
        }

        public static void SetDetailsRowCount(DependencyObject obj, int value)
        {
            obj.SetValue(DetailsRowCountProperty, value);
        }

        public static readonly DependencyProperty DetailsRowCountProperty =
            DependencyProperty.RegisterAttached("DetailsRowCount", typeof(int), typeof(PropertyGridLayoutAttacher), new PropertyMetadata(0, (s, a) =>
            {
                ResetDetailsGridRow(s as Grid, (int)a.NewValue);
            }));

        private static void ResetDetailsGridRow(Grid grid, int count)
        {
            if (grid == null)
                return;

            while (grid.RowDefinitions.Count > count * 2 - 1)
                grid.RowDefinitions.RemoveAt(grid.RowDefinitions.Count - 1);
            while (grid.RowDefinitions.Count < count * 2 - 1)
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });

            int index = 0;
            foreach (var col in grid.RowDefinitions)
                col.Height = (index++) % 2 == 0 ?
                    new GridLength(1, GridUnitType.Star) :
                    new GridLength(1, GridUnitType.Auto);
        }

        #endregion Properties - DetailsRowCount

        #region Properties - RowCount

        public static int GetRowCount(DependencyObject obj)
        {
            return (int)obj.GetValue(RowCountProperty);
        }

        public static void SetRowCount(DependencyObject obj, int value)
        {
            obj.SetValue(RowCountProperty, value);
        }

        public static readonly DependencyProperty RowCountProperty =
            DependencyProperty.RegisterAttached("RowCount", typeof(int), typeof(PropertyGridLayoutAttacher), new PropertyMetadata(0, (s, a) =>
            {
                ResetGridRow(s as Grid, (int)a.NewValue);
            }));

        private static void ResetGridRow(Grid grid, int count)
        {
            if (grid == null)
                return;

            while (grid.RowDefinitions.Count > count)
                grid.RowDefinitions.RemoveAt(grid.RowDefinitions.Count - 1);
            while (grid.RowDefinitions.Count < count)
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });
        }

        #endregion Properties - RowCount

        #region Properties - ColumnCount

        public static int GetColumnCount(DependencyObject obj)
        {
            return (int)obj.GetValue(ColumnCountProperty);
        }

        public static void SetColumnCount(DependencyObject obj, int value)
        {
            obj.SetValue(ColumnCountProperty, value);
        }

        public static readonly DependencyProperty ColumnCountProperty =
            DependencyProperty.RegisterAttached("ColumnCount", typeof(int), typeof(PropertyGridLayoutAttacher), new PropertyMetadata(0, (s, a) =>
            {
                ResetGridColumn(s as Grid, (int)a.NewValue);
            }));

        private static void ResetGridColumn(Grid grid, int count)
        {
            if (grid == null)
                return;

            while (grid.ColumnDefinitions.Count > count * 2 - 1)
                grid.ColumnDefinitions.RemoveAt(grid.ColumnDefinitions.Count - 1);
            while (grid.ColumnDefinitions.Count < count * 2 - 1)
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            var spacing = PropertyGridLayoutAttacher.GetColumnSpacing(grid);
            spacing = spacing <= 0 ? 50 : spacing;

            int index = 0;
            foreach (var col in grid.ColumnDefinitions)
                col.Width = (index++) % 2 == 0 ?
                    new GridLength(1, GridUnitType.Star) :
                    new GridLength(spacing, GridUnitType.Pixel);
        }

        #endregion Properties - ColumnCount
    }
}