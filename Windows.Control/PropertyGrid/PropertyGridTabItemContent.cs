using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Windows.Control
{
    public class PropertyGridTabItemContent : ContentControl
    {
        #region Properties

        public ObservableCollection<PropertyGridCatalogMetadata> Catalogs
        {
            get { return (ObservableCollection<PropertyGridCatalogMetadata>)GetValue(CatalogsProperty); }
            set { SetValue(CatalogsProperty, value); }
        }

        public static readonly DependencyProperty CatalogsProperty =
            DependencyProperty.Register("Catalogs", typeof(ObservableCollection<PropertyGridCatalogMetadata>), typeof(PropertyGridTabItemContent));

        public DataGridLength NameWidth
        {
            get { return (DataGridLength)GetValue(NameWidthProperty); }
            set { SetValue(NameWidthProperty, value); }
        }

        public static readonly DependencyProperty NameWidthProperty =
            DependencyProperty.Register("NameWidth", typeof(DataGridLength), typeof(PropertyGridTabItemContent), new PropertyMetadata(new DataGridLength(), (s, e) =>
            {
                var content = s as PropertyGridTabItemContent;
                var widthNew = (DataGridLength)e.NewValue;
                var widthOld = (DataGridLength)e.OldValue;

                if (widthNew.Value < widthOld.Value)
                    content.NameWidth = widthOld;

                //Console.WriteLine(width);
                //content.NameWidthMin = width.Value;
                //content.NameWidthMax = width.Value;
            }));

        public double NameWidthMin
        {
            get { return (double)GetValue(NameWidthMinProperty); }
            set { SetValue(NameWidthMinProperty, value); }
        }

        public static readonly DependencyProperty NameWidthMinProperty =
            DependencyProperty.Register("NameWidthMin", typeof(double), typeof(PropertyGridTabItemContent), new PropertyMetadata(0.0));

        public double NameWidthMax
        {
            get { return (double)GetValue(NameWidthMaxProperty); }
            set { SetValue(NameWidthMaxProperty, value); }
        }

        public static readonly DependencyProperty NameWidthMaxProperty =
            DependencyProperty.Register("NameWidthMax", typeof(double), typeof(PropertyGridTabItemContent), new PropertyMetadata(double.MaxValue));

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(PropertyGridTabItemContent));

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(PropertyGridTabItemContent));


        public ePropertyGridContainerType ContainerType
        {
            get { return (ePropertyGridContainerType)GetValue(ContainerTypeProperty); }
            set { SetValue(ContainerTypeProperty, value); }
        }

        public static readonly DependencyProperty ContainerTypeProperty =
            DependencyProperty.Register("ContainerType", typeof(ePropertyGridContainerType), typeof(PropertyGridTabItemContent), new PropertyMetadata(ePropertyGridContainerType.DataGrid));



        public PropertyGrid PropertyGrid { get; internal set; }

        #endregion

        #region Ctor

        public PropertyGridTabItemContent()
        {
            Catalogs = new ObservableCollection<PropertyGridCatalogMetadata>();
            DataContext = this;
            BindingGroup = new System.Windows.Data.BindingGroup();
        }

        #endregion

        #region Methods

        internal void InstallSelectedIndexSyncHandler()
        {
            foreach (var catalog in Catalogs)
            {
                catalog.SelectedIndexChangedCallback = (s, index) =>
                {
                    if (index < 0)
                        return;

                    foreach (var cl in Catalogs)
                    {
                        if (cl == s)
                            continue;

                        cl.SelectedIndex = -1;
                    }
                };
            }
        }

        #endregion
    }
}
