using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Windows.Control
{
    public static class PropertyGridAttacher
    {
        #region Methods

        public static bool GetHasError(this PropertyGrid source)
        {
            bool has = false;

            //foreach (var item in source.Items)
            //{
            //    var content = item as ContentControl;
            //    if (content == null)
            //        continue;

            //var ui = content.Content as DependencyObject;
            //if (ui == null)
            //    continue;

            has = has || (
                source.IsGroupingEnabled ?
                source.tabControl.GetHasError() :
                System.Windows.Controls.Validation.GetHasError(source));

            //if (has)
            //    break;
            //}

            return has;
        }

        public static string GetError(this PropertyGrid source)
        {
            //foreach (var item in source.Items)
            //{
            //    var content = item as ContentControl;
            //    if (content == null)
            //        continue;

            //    var ui = content.Content as DependencyObject;
            //    if (ui == null)
            //        continue;

            if (source.IsGroupingEnabled)
                return source.tabControl.GetError();

            if (System.Windows.Controls.Validation.GetHasError(source))
                return System.Windows.Controls.Validation.GetErrors(source)[0].ErrorContent.ToString();
            //}

            return null;
        }

        public static ValidationError[] GetErrors(this PropertyGrid source)
        {
            List<ValidationError> list = new List<ValidationError>();

            //foreach (var item in source.Items)
            //{
            //    var content = item as ContentControl;
            //    if (content == null)
            //        continue;

            //    var ui = content.Content as DependencyObject;
            //    if (ui == null)
            //        continue;

            if (source.IsGroupingEnabled)
                list.AddRange(source.tabControl.GetErrors());
            else if (System.Windows.Controls.Validation.GetHasError(source))
                list.AddRange(System.Windows.Controls.Validation.GetErrors(source));
            //}

            return list.ToArray();
        }

        public static bool Validate(this PropertyGrid source)
        {
            //foreach (var item in source.Items)
            //{
            //    var content = item as ContentControl;
            //    if (content == null)
            //        continue;

            //    var ui = content.Content as FrameworkElement;
            //    if (ui == null)
            //        continue;
            if (source.IsGroupingEnabled)
                return source.tabControl.Validate();

            foreach (var exp in source.BindingGroup.BindingExpressions)
            {
                Console.WriteLine(exp);
                exp.UpdateSource();
            }
            //}

            return source.GetHasError();
        }

        public static List<BindingExpressionBase> ExtractBindingExpressions(this PropertyGrid source)
        {
            if (source.IsGroupingEnabled)
                return source.tabControl.ExtractBindingExpressions();

            var list = source.BindingGroup.BindingExpressions.ToList();
            source.BindingGroup.BindingExpressions.Clear();
            return list;
        }

        #endregion Methods

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
            DependencyProperty.RegisterAttached("RowCount", typeof(int), typeof(PropertyGridAttacher), new PropertyMetadata(0, (s, a) =>
            {
            }));

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
            DependencyProperty.RegisterAttached("ColumnCount", typeof(int), typeof(PropertyGridAttacher), new PropertyMetadata(0, (s, a) =>
            {
            }));

        #endregion Properties - ColumnCount
    }
}