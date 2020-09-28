using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Windows.Core;

namespace Windows.Control
{
    internal class PropertyGridShell : TaskContainer
    {
        #region Fields

        private PropertyGrid pg;
        internal Dictionary<string, PropertyGridTabItemContent> tabs;

        private Type lastObjectType;
        private string lastPropertyClass;
        private bool isSameObjectType;

        #endregion Fields

        #region Ctor

        public PropertyGridShell(PropertyGrid pg)
            : base(new TaskQueueDispatcher(pg.Dispatcher))
        {
            this.pg = pg;
            tabs = new Dictionary<string, PropertyGridTabItemContent>();
        }

        #endregion Ctor

        #region Methods

        #region Methods - Override

        protected override void OnStarted(TaskStartedEventArgs e)
        {
            pg.BeginInit();
            pg.RaiseInitializeBegin();

            foreach (var prop in pg._properties)
                prop.Uninstall();

            object value = e.Instance.Argument.UserState;
            var currentObjectType = value != null ? value.GetType() : null;
            var currentClass = pg.PropertyClass;
            isSameObjectType =
                pg.EnableObjectMetadataCache &&
                currentObjectType == lastObjectType &&
                currentObjectType != null && (currentClass == lastPropertyClass);

            if (isSameObjectType)
                return;

            pg._properties.ToList().ForEach(c => c.Dispose());
            pg._properties.Clear();
            pg.BindingGroup.BindingExpressions.Clear();

            foreach (var tab in tabs)
                tab.Value.BindingGroup.BindingExpressions.Clear();
            tabs.Clear();

            while (pg.tabControl.Items.Count > 0)
                pg.tabControl.Items.RemoveAt(0);

            pg.dataGrid.ItemsSource = null;
        }

        protected override void OnEnded(TaskEndedEventArgs e)
        {
            pg.RaiseInitializeEnd();
            pg.EndInit();
        }

        protected override void OnCompleted(TaskCompletedEventArgs e)
        {
            //if (e.Instance.IsStopPending)
            //    return;
            if (isSameObjectType)
                return;

            object value = e.Instance.Argument.UserState;
            if (value == null)
                lastObjectType = null;
            else
                lastObjectType = value.GetType();

            lastPropertyClass = pg.PropertyClass;

            if (!pg.IsGroupingEnabled)
            {
                pg.Dispatcher.Invoke(new Action(() => { pg.UpdateGridRowColumn(); }));
                pg.dataGrid.ItemsSource = pg._properties;
                return;
            }

            var metas = pg._properties.ToList();
            if (metas == null)
                return;

            OnCompletedGrouping(metas);
        }

        protected override void OnTerminated(TaskTerminatedEventArgs e)
        {
            lastObjectType = null;
            Console.WriteLine(e);
        }

        protected override void OnProgressChanged(TaskProgressChangedEventArgs e)
        {
        }

        protected override void OnGo(TaskGoEventArgs e)
        {
            if (!isSameObjectType)
                OnGoCreateNew(e);
            else
                OnGoCreateLast(e);
        }

        #endregion Methods - Override

        #region Methods - Private

        private void OnGoCreateLast(TaskGoEventArgs e)
        {
            object value = e.Instance.Argument.UserState;
            if (value == null)
                return;

            object valueDefault = pg.ObjectDefalult;
            Type type = null;
            PropertyInfo[] pis = null;
            if (valueDefault != null)
                type = valueDefault.GetType();
            if (type != null)
                pis = type.GetProperties();

            var list = new List<PropertyDescriptor>();
            foreach (var item in pg._properties)
            {
                if (e.Instance.IsStopPending)
                    return;

                PropertyDescriptor meta = item;
                meta.Object = value;
                meta.Value = value.GetPropertyValue(meta.Name);

                if (pis != null && pis.Any(c => c.Name == meta.Name))
                    meta.DefaultValue = valueDefault.GetPropertyValue(meta.Name);

                list.Add(meta);
            }

            var broadcast = false;
            Application.Current.Dispatcher.Invoke(new Action(() => broadcast = pg.BroadcastPropertyChanged));

            list.ForEach(c =>
            {
                c.Install();

                if (broadcast)
                    list.ForEach(d => c.PostPropertyValueChangedAsync(d.Name, true));
            });

            //e.Instance.Argument.UserState = list;
        }

        private void OnGoCreateNew(TaskGoEventArgs e)
        {
            object value = e.Instance.Argument.UserState;
            if (value == null)
                return;

            object valueDefault = pg.ObjectDefalult;
            Type type = null;
            PropertyInfo[] pis = null;
            if (valueDefault != null)
                type = valueDefault.GetType();
            if (type != null)
                pis = type.GetProperties();

            var list = new List<PropertyDescriptor>();

            value.TraversalPropertiesInfo((PropertyInfo pi, object val) =>
            {
                if (e.Instance.IsStopPending)
                    return false;
                if (!pi.CanWrite)
                    return true;

                PropertyDescriptor meta = null;

                Application.Current.Dispatcher.Invoke(
                    new Action(() => meta = PropertyDescriptor.Create(pg, value, pi.Name, pg.Editable, pg.PropertyClass, pg.ContainerType)));

                if (meta == null)
                    return true;

                //meta.Object = value;
                meta.PropertyGrid = pg;

                if (pis != null && pis.Any(c => c.Name == pi.Name))
                    meta.DefaultValue = valueDefault.GetPropertyValue(pi.Name);

                var args = new InitializePropertyDescriptorEventArgs(meta);
                pg.RaiseInitializeProperty(args);
                if (!args.Cancel)
                {
                    list.Add(meta);
                    return true;
                }

                var attr = pi.GetAttribute<PropertyDescriptorAttribute>();
                if (attr == null)
                {
                    list.Add(meta);
                    return true;
                }

                var builder = attr.CreateBuilder();
                if (builder == null)
                {
                    list.Add(meta);
                    return true;
                }

                meta = builder.Build(meta);
                if (meta != null)
                {
                    list.Add(meta);
                    return true;
                }

                return true;
            });

            var broadcast = false;
            Application.Current.Dispatcher.Invoke(new Action(() => broadcast = pg.BroadcastPropertyChanged));

            list.ForEach(c =>
            {
                Application.Current.Dispatcher.Invoke(new Action(() => pg._properties.Add(c)));
                c.Install();

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    if (c.BindingExpression != null && !pg.IsGroupingEnabled && c.Editable)
                        pg.BindingGroup.BindingExpressions.Add(c.BindingExpression);
                }));

                if (broadcast)
                    list.ForEach(d => c.PostPropertyValueChangedAsync(d.Name, true));
            });

            //e.Instance.Argument.UserState = list;
        }

        private void OnCompletedGrouping(List<PropertyDescriptor> metas)
        {
            metas.ForEach(c =>
            {
                var info = c.Object.GetType().GetProperty(c.Name);

                var attr = info.GetAttribute<PropertyDescriptorAttribute>();
                if (attr == null)
                    attr = new PropertyDescriptorAttribute();
                if (attr.Gallery.IsNullOrBlank())
                    attr.Gallery = "默认";

                var container = GetPropertyContainer(attr);

                if (c.BindingExpression != null && pg.IsGroupingEnabled && pg.Editable)
                    tabs[attr.Gallery].BindingGroup.BindingExpressions.Add(c.BindingExpression);

                container.Properties.Add(c);
            });

            if (pg.ContainerType == ePropertyGridContainerType.Grid ||
                pg.ContainerType == ePropertyGridContainerType.Form ||
                pg.ContainerType == ePropertyGridContainerType.FormWithoutScrollViewer ||
                pg.ContainerType == ePropertyGridContainerType.Details ||
                pg.ContainerType == ePropertyGridContainerType.DetailsWithoutScrollViewer)
            {
                var attr = lastObjectType == null ? new GridDescriptorAttribute() :
                    lastObjectType.GetAttribute<GridDescriptorAttribute>();

                if (attr == null)
                    attr = new GridDescriptorAttribute();

                foreach (var tab in tabs)
                {
                    tab.Value.ContainerType = pg.ContainerType;
                    foreach (var cata in tab.Value.Catalogs)
                    {
                        cata.GridColumnSpacing = pg.GridColumnSpacing;
                        cata.UpdateGridRowColumn(attr.Column);
                    }
                }
            }

            pg.tabControl.HeaderVisibility =
                    tabs.Count > 1 ? Visibility.Visible : Visibility.Collapsed;

            if (pg.tabControl.HeaderVisibility != Visibility.Visible)
                pg.tabControl.BorderThickness = new Thickness(0);
            else if (pg.tabControl.Direction == eDirection.Top)
                pg.tabControl.BorderThickness = new Thickness(0, 1, 0, 0);
            else if (pg.tabControl.Direction == eDirection.Left)
                pg.tabControl.BorderThickness = new Thickness(1, 0, 0, 0);
            else if (pg.tabControl.Direction == eDirection.Right)
                pg.tabControl.BorderThickness = new Thickness(0, 0, 1, 0);
            else if (pg.tabControl.Direction == eDirection.Bottom)
                pg.tabControl.BorderThickness = new Thickness(0, 0, 0, 1);

            var contentMargin = pg.tabControl.HeaderVisibility ==
                Visibility.Visible ? new Thickness(0, 5, 0, 5) : new Thickness(0);

            foreach (var tab in tabs)
            {
                tab.Value.Margin = contentMargin;
                tab.Value.Catalogs[0].HeaderVisibility =
                    tab.Value.Catalogs.Count > 1 ? Visibility.Visible : Visibility.Collapsed;

                tab.Value.InstallSelectedIndexSyncHandler();
            }

            pg.tabControl.SelectedIndex = -1;
            pg.tabControl.SelectedIndex = 0;
        }

        private PropertyGridCatalogMetadata GetPropertyContainer(PropertyDescriptorAttribute attr)
        {
            if (!tabs.ContainsKey(attr.Gallery))
            {
                var content = new PropertyGridTabItemContent() { PropertyGrid = pg };
                var item = new ListTabItem()
                {
                    Name = attr.Gallery.Replace(' ', '_'),
                    Header = new TextBox()
                    {
                        Text = attr.Gallery,
                        ToolTip = attr.Gallery,
                    },
                    Content = content,
                };

                tabs[attr.Gallery] = content;
                pg.tabControl.Items.Add(item);
            }

            var tab = tabs[attr.Gallery];
            if (!tab.Catalogs.Any(c => c.Name == attr.Catalog))
            {
                tab.Catalogs.Add(new PropertyGridCatalogMetadata()
                {
                    Name = attr.Catalog,
                    Image = attr.CatalogImage16.IsNullOrBlank() ? null : BitmapFrame.Create(new Uri(attr.CatalogImage16)),
                    PropertyGrid = pg,
                });
            }

            return tab.Catalogs.FirstOrDefault(c => c.Name == attr.Catalog);
        }

        #endregion Methods - Private

        #endregion Methods
    }
}