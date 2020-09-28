using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Windows.Core;
using Core;

namespace Windows.Control
{
    public class PropertyGridCatalogMetadata : NotifyDependencyCDObject
    {
        #region Properties

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(PropertyGridCatalogMetadata));

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(PropertyGridCatalogMetadata));

        public int GridRowCount
        {
            get { return (int)GetValue(GridRowCountProperty); }
            set { SetValue(GridRowCountProperty, value); }
        }

        public static readonly DependencyProperty GridRowCountProperty =
            DependencyProperty.Register("GridRowCount", typeof(int), typeof(PropertyGridCatalogMetadata), new PropertyMetadata(0));

        public int GridColumnCount
        {
            get { return (int)GetValue(GridColumnCountProperty); }
            set { SetValue(GridColumnCountProperty, value); }
        }

        public static readonly DependencyProperty GridColumnCountProperty =
            DependencyProperty.Register("GridColumnCount", typeof(int), typeof(PropertyGridCatalogMetadata), new PropertyMetadata(0));

        public double GridColumnSpacing
        {
            get { return (double)GetValue(GridColumnSpacingProperty); }
            set { SetValue(GridColumnSpacingProperty, value); }
        }

        public static readonly DependencyProperty GridColumnSpacingProperty =
            DependencyProperty.Register("GridColumnSpacing", typeof(double), typeof(PropertyGridCatalogMetadata), new PropertyMetadata(50.0));

        public Visibility HeaderVisibility
        {
            get { return _HeaderVisibility; }
            set { _HeaderVisibility = value; NotifyPropertyChanged("HeaderVisibility"); }
        }

        private Visibility _HeaderVisibility;

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(PropertyGridCatalogMetadata), new PropertyMetadata(-1, (s, e) =>
            {
                var meta = s as PropertyGridCatalogMetadata;
                if (meta.SelectedIndexChangedCallback == null)
                    return;

                meta.SelectedIndexChangedCallback(meta, (int)e.NewValue);
            }));

        public ObservableCollection<PropertyDescriptor> Properties { get; private set; }
        public ObservableCollection<object> DisplayItems { get; private set; }

        public PropertyGrid PropertyGrid { get; internal set; }

        public Action<PropertyGridCatalogMetadata, int> SelectedIndexChangedCallback { get; set; }

        #endregion Properties

        #region Ctor

        public PropertyGridCatalogMetadata()
        {
            DisplayItems = new ObservableCollection<object>();
            Properties = new ObservableCollection<PropertyDescriptor>();
        }

        #endregion Ctor

        #region Methods

        internal void UpdateGridRowColumn(int cntCol)
        {
            DisplayItems.Clear();

            if (PropertyGrid.ContainerType == ePropertyGridContainerType.Details ||
                PropertyGrid.ContainerType == ePropertyGridContainerType.DetailsWithoutScrollViewer)
            {
                int row = 0;
                int col = 0;
                for (int i = 0; i < Properties.Count; i++)
                {
                    var prop = Properties[i];
                    var pda = prop.PropertyInfo.GetAttribute<PropertyDescriptorAttribute>();
                    if (pda == null)
                        pda = new PropertyDescriptorAttribute();

                    if (col >= cntCol)
                    {
                        row++;
                        col = 0;
                    }

                    Grid.SetRow(prop.Designer, row * 2);
                    Grid.SetColumn(prop.Designer, col * 2);

                    var colSpan = pda.ColumnSpan;
                    Grid.SetColumnSpan(prop.Designer, colSpan * 2 - 1);

                    col += colSpan;

                    if (pda.Height > 0)
                        (prop.Designer as FrameworkElement).Height = pda.Height;
                }

                GridColumnCount = cntCol;
                GridRowCount = row + 1;

                foreach (var item in Properties)
                    DisplayItems.Add(item);

                for (int i = 0; i < row; i++)
                {
                    var separator = new SeparatorH();
                    separator.Margin = new Thickness(0);
                    Grid.SetRow(separator, i * 2 + 1);
                    Grid.SetColumnSpan(separator, cntCol * 2 - 1);

                    DisplayItems.Add(new PropertyGridSeparatorItem { Designer = separator });
                }
            }
            else
            {
                int row = 0;
                int col = 0;
                for (int i = 0; i < Properties.Count; i++)
                {
                    var prop = Properties[i];
                    var pda = prop.PropertyInfo.GetAttribute<PropertyDescriptorAttribute>();
                    if (pda == null)
                        pda = new PropertyDescriptorAttribute();

                    if (col >= cntCol)
                    {
                        row++;
                        col = 0;
                    }

                    Grid.SetRow(prop.Designer, row);
                    Grid.SetColumn(prop.Designer, col * 2);

                    var colSpan = pda.ColumnSpan;
                    Grid.SetColumnSpan(prop.Designer, colSpan * 2 - 1);

                    col += colSpan;

                    if (pda.Height > 0)
                        (prop.Designer as FrameworkElement).Height = pda.Height;
                }

                GridColumnCount = cntCol;
                GridRowCount = row + 1;
            }
        }

        #endregion Methods
    }
}