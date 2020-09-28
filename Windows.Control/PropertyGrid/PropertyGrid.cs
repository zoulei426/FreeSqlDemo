using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Core;

namespace Windows.Control
{
    public class PropertyGrid : ContentControl
    {
        #region Properties

        public string PropertyClass
        {
            get { return (string)GetValue(PropertyClassProperty); }
            set { SetValue(PropertyClassProperty, value); }
        }

        public static readonly DependencyProperty PropertyClassProperty =
            DependencyProperty.Register("PropertyClass", typeof(string), typeof(PropertyGrid), new PropertyMetadata("*"));

        public object Object
        {
            get { return (object)GetValue(ObjectProperty); }
            set { SetValue(ObjectProperty, value); }
        }

        public static readonly DependencyProperty ObjectProperty =
            DependencyProperty.Register("Object", typeof(object), typeof(PropertyGrid), new PropertyMetadata((s, e) =>
            {
                var propertyGrid = s as PropertyGrid;
                if (propertyGrid.tabControl == null)
                    propertyGrid.ApplyTemplate();

                propertyGrid.shell.Reset(e.NewValue);
            }));

        public Dictionary<string, object> Properties { get; private set; }

        public List<PropertyDescriptor> PropertyDescriptors { get { return _properties.ToList(); } }

        public object ObjectDefalult { get; set; }

        public bool Editable { get; set; }

        public bool EnableObjectMetadataCache { get; set; }

        public Visibility DefaultValueColumnVisibility
        {
            get { return (Visibility)GetValue(DefaultValueColumnVisibilityProperty); }
            set { SetValue(DefaultValueColumnVisibilityProperty, value); }
        }

        public static readonly DependencyProperty DefaultValueColumnVisibilityProperty =
            DependencyProperty.Register("DefaultValueColumnVisibility", typeof(Visibility), typeof(PropertyGrid), new PropertyMetadata(Visibility.Collapsed));

        public Visibility StateColumnVisibility
        {
            get { return (Visibility)GetValue(StateColumnVisibilityProperty); }
            set { SetValue(StateColumnVisibilityProperty, value); }
        }

        public static readonly DependencyProperty StateColumnVisibilityProperty =
            DependencyProperty.Register("StateColumnVisibility", typeof(Visibility), typeof(PropertyGrid), new PropertyMetadata(Visibility.Collapsed));

        public bool IsGroupingEnabled
        {
            get { return (bool)GetValue(IsGroupingEnabledProperty); }
            set { SetValue(IsGroupingEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsGroupingEnabledProperty =
            DependencyProperty.Register("IsGroupingEnabled", typeof(bool), typeof(PropertyGrid), new PropertyMetadata(true));

        public bool BroadcastPropertyChanged
        {
            get { return (bool)GetValue(BroadcastPropertyChangedProperty); }
            set { SetValue(BroadcastPropertyChangedProperty, value); }
        }

        public static readonly DependencyProperty BroadcastPropertyChangedProperty =
            DependencyProperty.Register("BroadcastPropertyChanged", typeof(bool), typeof(PropertyGrid), new PropertyMetadata(true));

        public ePropertyGridContainerType ContainerType
        {
            get { return (ePropertyGridContainerType)GetValue(ContainerTypeProperty); }
            set { SetValue(ContainerTypeProperty, value); }
        }

        public static readonly DependencyProperty ContainerTypeProperty =
            DependencyProperty.Register("ContainerType", typeof(ePropertyGridContainerType), typeof(PropertyGrid), new PropertyMetadata(ePropertyGridContainerType.DataGrid));

        public int GridRowCount
        {
            get { return (int)GetValue(GridRowCountProperty); }
            set { SetValue(GridRowCountProperty, value); }
        }

        public static readonly DependencyProperty GridRowCountProperty =
            DependencyProperty.Register("GridRowCount", typeof(int), typeof(PropertyGrid), new PropertyMetadata(0));

        public int GridColumnCount
        {
            get { return (int)GetValue(GridColumnCountProperty); }
            set { SetValue(GridColumnCountProperty, value); }
        }

        public static readonly DependencyProperty GridColumnCountProperty =
            DependencyProperty.Register("GridColumnCount", typeof(int), typeof(PropertyGrid), new PropertyMetadata(0));

        public double GridColumnSpacing
        {
            get { return (double)GetValue(GridColumnSpacingProperty); }
            set { SetValue(GridColumnSpacingProperty, value); }
        }

        public static readonly DependencyProperty GridColumnSpacingProperty =
            DependencyProperty.Register("GridColumnSpacing", typeof(double), typeof(PropertyGrid), new PropertyMetadata(50.0));

        public double NameWidth
        {
            get { return (double)GetValue(NameWidthProperty); }
            set { SetValue(NameWidthProperty, value); }
        }

        public static readonly DependencyProperty NameWidthProperty =
            DependencyProperty.Register("NameWidth", typeof(double), typeof(PropertyGrid), new PropertyMetadata(120.0));

        public Thickness RowSeparatorMargin
        {
            get { return (Thickness)GetValue(RowSeparatorMarginProperty); }
            set { SetValue(RowSeparatorMarginProperty, value); }
        }

        public static readonly DependencyProperty RowSeparatorMarginProperty =
            DependencyProperty.Register("RowSeparatorMargin", typeof(Thickness), typeof(PropertyGrid), new PropertyMetadata(new Thickness(0, 0, 0, 15)));

        public Style PairItemStyle
        {
            get { return (Style)GetValue(PairItemStyleProperty); }
            set { SetValue(PairItemStyleProperty, value); }
        }

        public static readonly DependencyProperty PairItemStyleProperty =
            DependencyProperty.Register("PairItemStyle", typeof(Style), typeof(PropertyGrid));

        public Style OptionsContainerStyle
        {
            get { return (Style)GetValue(OptionsContainerStyleProperty); }
            set { SetValue(OptionsContainerStyleProperty, value); }
        }

        public static readonly DependencyProperty OptionsContainerStyleProperty =
            DependencyProperty.Register("OptionsContainerStyle", typeof(Style), typeof(PropertyGrid));

        #endregion Properties

        #region Fields

        //private object _object;
        internal ObservableCollection<PropertyDescriptor> _properties;

        internal ListTabControl tabControl;
        internal ItemsControl dataGrid;

        private PropertyGridShell shell = null;

        #endregion Fields

        #region Events

        public event EventHandler InitializeBegin;

        public event EventHandler InitializeEnd;

        public event EventHandler<InitializePropertyDescriptorEventArgs> InitializeProperty;

        public event EventHandler<PropertyGridAlertEventArgs> Alert;

        #endregion Events

        #region Ctor

        public PropertyGrid()
        {
            Editable = true;
            _properties = new ObservableCollection<PropertyDescriptor>();
            Properties = new Dictionary<string, object>();
            shell = new PropertyGridShell(this);
            EnableObjectMetadataCache = true;
            BindingGroup = new BindingGroup();
        }

        #endregion Ctor

        #region Methods

        #region Methods - Internal

        internal void RaiseInitializeBegin()
        {
            if (InitializeBegin != null)
                InitializeBegin(this, new EventArgs());
        }

        internal void RaiseInitializeEnd()
        {
            if (InitializeEnd != null)
                InitializeEnd(this, new EventArgs());
        }

        internal void RaiseInitializeProperty(InitializePropertyDescriptorEventArgs e)
        {
            if (InitializeProperty != null)
                InitializeProperty(this, e);
        }

        internal void RaiseAlert(PropertyGridAlertEventArgs e)
        {
            if (Alert != null)
                Alert(this, e);
        }

        #endregion Methods - Internal

        #region Methods - Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            tabControl = GetTemplateChild("tabControl") as ListTabControl;
            dataGrid = GetTemplateChild("dg") as ItemsControl;
        }

        #endregion Methods - Override

        #region Methods - Private

        internal void UpdateGridRowColumn()
        {
            var attr = Object == null ? new GridDescriptorAttribute() :
                Object.GetType().GetAttribute<GridDescriptorAttribute>();

            if (attr == null)
                attr = new GridDescriptorAttribute();

            int column = attr.Column;
            int row = (_properties.Count - 1) / column + 1;

            GridColumnCount = column;
            GridRowCount = row;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    var index = i * column + j;
                    if (index >= _properties.Count)
                        continue;

                    var prop = _properties[index];
                    var pda = prop.PropertyInfo.GetAttribute<PropertyDescriptorAttribute>();
                    if (pda == null)
                        pda = new PropertyDescriptorAttribute();

                    Grid.SetRow(prop.Designer, i);
                    Grid.SetColumn(prop.Designer, j * 2);
                    Grid.SetColumnSpan(prop.Designer, pda.ColumnSpan * 2 - 1);

                    if (pda.Height > 0)
                        (prop.Designer as FrameworkElement).Height = pda.Height;
                }
            }
        }

        //private static void DefaultValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var pg = d as PropertyGrid;
        //    if (pg == null)
        //        return;
        //    if (pg.tabControl == null)
        //        pg.ApplyTemplate();

        //    foreach (var item in pg.shell.tabs)
        //    {
        //     item.Value.Catalogs.p
        //    }
        //    pg.shell.tabs.Columns[1].Visibility = (Visibility)e.NewValue;
        //}

        //private static void StateChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var pg = d as PropertyGrid;
        //    if (pg == null)
        //        return;
        //    if (pg.tabControl == null)
        //        pg.ApplyTemplate();

        //    pg._dg.Columns[3].Visibility = (Visibility)e.NewValue;
        //}

        #endregion Methods - Private

        #endregion Methods
    }
}