using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Windows.Control
{
    public static class TabControlAttacher
    {
        #region Methods

        public static bool GetHasError(this ListTabControl source)
        {
            bool has = false;

            foreach (var item in source.Items)
            {
                var content = item as ContentControl;
                if (content == null)
                    continue;

                var ui = content.Content as DependencyObject;
                if (ui == null)
                    continue;

                has = has || System.Windows.Controls.Validation.GetHasError(ui);
                if (has)
                    break;
            }

            return has;
        }

        public static string GetError(this ListTabControl source)
        {
            foreach (var item in source.Items)
            {
                var content = item as ContentControl;
                if (content == null)
                    continue;

                var ui = content.Content as DependencyObject;
                if (ui == null)
                    continue;

                if (System.Windows.Controls.Validation.GetHasError(ui))
                    return System.Windows.Controls.Validation.GetErrors(ui)[0].ErrorContent.ToString();
            }

            return null;
        }

        public static ValidationError[] GetErrors(this ListTabControl source)
        {
            List<ValidationError> list = new List<ValidationError>();

            foreach (var item in source.Items)
            {
                var content = item as ContentControl;
                if (content == null)
                    continue;

                var ui = content.Content as DependencyObject;
                if (ui == null)
                    continue;

                if (System.Windows.Controls.Validation.GetHasError(ui))
                    list.AddRange(System.Windows.Controls.Validation.GetErrors(ui));
            }

            return list.ToArray();
        }

        public static bool Validate(this ListTabControl source)
        {
            foreach (var item in source.Items)
            {
                var content = item as ContentControl;
                if (content == null)
                    continue;

                var ui = content.Content as FrameworkElement;
                if (ui == null)
                    continue;
                if (ui.BindingGroup == null)
                    continue;

                foreach (var exp in ui.BindingGroup.BindingExpressions)
                    exp.UpdateSource();
            }

            return source.GetHasError();
        }

        public static List<BindingExpressionBase> ExtractBindingExpressions(this ListTabControl source)
        {
            List<BindingExpressionBase> list = new List<BindingExpressionBase>();

            foreach (var item in source.Items)
            {
                var content = item as ContentControl;
                if (content == null)
                    continue;

                var ui = content.Content as FrameworkElement;
                if (ui == null)
                    continue;
                if (ui.BindingGroup == null)
                    continue;

                list.AddRange(ui.BindingGroup.BindingExpressions.ToList());
                ui.BindingGroup.BindingExpressions.Clear();
            }

            return list;
        }

        #endregion Methods
    }
}