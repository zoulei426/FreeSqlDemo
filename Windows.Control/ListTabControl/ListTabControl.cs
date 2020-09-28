using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Windows.Core;

namespace Windows.Control
{
    public partial class ListTabControl : ItemsControl, INotifyPropertyChanged
    {
        #region Properties

        public eDirection Direction
        {
            get { return (eDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(eDirection), typeof(ListTabControl), new PropertyMetadata((s, a) =>
            {
                (s as ListTabControl).SelectorController.Refresh();
            }));

        public double SelectorLength
        {
            get { return (double)GetValue(SelectorLengthProperty); }
            set { SetValue(SelectorLengthProperty, value); }
        }

        public static readonly DependencyProperty SelectorLengthProperty =
            DependencyProperty.Register("SelectorLength", typeof(double), typeof(ListTabControl), new PropertyMetadata(3.0, (s, a) =>
            {
                (s as ListTabControl).SelectorController.Refresh();
            }));

        public Visibility SelectorVisibility
        {
            get { return (Visibility)GetValue(SelectorVisibilityProperty); }
            set { SetValue(SelectorVisibilityProperty, value); }
        }

        public static readonly DependencyProperty SelectorVisibilityProperty =
            DependencyProperty.Register("SelectorVisibility", typeof(Visibility), typeof(ListTabControl), new PropertyMetadata(Visibility.Collapsed, (s, a) =>
            {
                (s as ListTabControl).SelectorController.Refresh();
            }));

        public bool SelectorReverse
        {
            get { return (bool)GetValue(SelectorReverseProperty); }
            set { SetValue(SelectorReverseProperty, value); }
        }

        public static readonly DependencyProperty SelectorReverseProperty =
            DependencyProperty.Register("SelectorReverse", typeof(bool), typeof(ListTabControl), new PropertyMetadata(false, (s, a) =>
            {
                (s as ListTabControl).SelectorController.Refresh();
            }));

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        public static readonly DependencyProperty HeaderBackgroundProperty =
            DependencyProperty.Register("HeaderBackground", typeof(Brush), typeof(ListTabControl));

        public Visibility HeaderVisibility
        {
            get { return (Visibility)GetValue(HeaderVisibilityProperty); }
            set { SetValue(HeaderVisibilityProperty, value); }
        }

        public static readonly DependencyProperty HeaderVisibilityProperty =
            DependencyProperty.Register("HeaderVisibility", typeof(Visibility), typeof(ListTabControl));

        public Visibility HeaderGridLineVisibility
        {
            get { return (Visibility)GetValue(HeaderGridLineVisibilityProperty); }
            set { SetValue(HeaderGridLineVisibilityProperty, value); }
        }

        public static readonly DependencyProperty HeaderGridLineVisibilityProperty =
            DependencyProperty.Register("HeaderGridLineVisibility", typeof(Visibility), typeof(ListTabControl), new PropertyMetadata(Visibility.Collapsed));

        public double HeaderWidth
        {
            get { return (double)GetValue(HeaderWidthProperty); }
            set { SetValue(HeaderWidthProperty, value); }
        }

        public static readonly DependencyProperty HeaderWidthProperty =
            DependencyProperty.Register("HeaderWidth", typeof(double), typeof(ListTabControl), new PropertyMetadata(double.NaN));

        public double HeaderHeight
        {
            get { return (double)GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }

        public static readonly DependencyProperty HeaderHeightProperty =
            DependencyProperty.Register("HeaderHeight", typeof(double), typeof(ListTabControl), new PropertyMetadata(double.NaN));

        public Thickness HeaderMargin
        {
            get { return (Thickness)GetValue(HeaderMarginProperty); }
            set { SetValue(HeaderMarginProperty, value); }
        }

        public static readonly DependencyProperty HeaderMarginProperty =
            DependencyProperty.Register("HeaderMargin", typeof(Thickness), typeof(ListTabControl));

        public Thickness HeaderGridLineThickness
        {
            get { return (Thickness)GetValue(HeaderGridLineThicknessProperty); }
            set { SetValue(HeaderGridLineThicknessProperty, value); }
        }

        public static readonly DependencyProperty HeaderGridLineThicknessProperty =
            DependencyProperty.Register("HeaderGridLineThickness", typeof(Thickness), typeof(ListTabControl));

        public Thickness ItemBorderThickness
        {
            get { return (Thickness)GetValue(ItemBorderThicknessProperty); }
            set { SetValue(ItemBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty ItemBorderThicknessProperty =
            DependencyProperty.Register("ItemBorderThickness", typeof(Thickness), typeof(ListTabControl));

        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register("ItemMargin", typeof(Thickness), typeof(ListTabControl));

        public Thickness ItemPadding
        {
            get { return (Thickness)GetValue(ItemPaddingProperty); }
            set { SetValue(ItemPaddingProperty, value); }
        }

        public static readonly DependencyProperty ItemPaddingProperty =
            DependencyProperty.Register("ItemPadding", typeof(Thickness), typeof(ListTabControl));

        public Brush ItemForegroundDefault
        {
            get { return (Brush)GetValue(ItemForegroundDefaultProperty); }
            set { SetValue(ItemForegroundDefaultProperty, value); }
        }

        public static readonly DependencyProperty ItemForegroundDefaultProperty =
            DependencyProperty.Register("ItemForegroundDefault", typeof(Brush), typeof(ListTabControl));

        public Brush ItemForegroundHover
        {
            get { return (Brush)GetValue(ItemForegroundHoverProperty); }
            set { SetValue(ItemForegroundHoverProperty, value); }
        }

        public static readonly DependencyProperty ItemForegroundHoverProperty =
            DependencyProperty.Register("ItemForegroundHover", typeof(Brush), typeof(ListTabControl));

        public Brush ItemForegroundPressed
        {
            get { return (Brush)GetValue(ItemForegroundPressedProperty); }
            set { SetValue(ItemForegroundPressedProperty, value); }
        }

        public static readonly DependencyProperty ItemForegroundPressedProperty =
            DependencyProperty.Register("ItemForegroundPressed", typeof(Brush), typeof(ListTabControl));

        public Brush ItemForegroundDisable
        {
            get { return (Brush)GetValue(ItemForegroundDisableProperty); }
            set { SetValue(ItemForegroundDisableProperty, value); }
        }

        public static readonly DependencyProperty ItemForegroundDisableProperty =
            DependencyProperty.Register("ItemForegroundDisable", typeof(Brush), typeof(ListTabControl));

        public Brush ItemBackgroundDefault
        {
            get { return (Brush)GetValue(ItemBackgroundDefaultProperty); }
            set { SetValue(ItemBackgroundDefaultProperty, value); }
        }

        public static readonly DependencyProperty ItemBackgroundDefaultProperty =
            DependencyProperty.Register("ItemBackgroundDefault", typeof(Brush), typeof(ListTabControl));

        public Brush ItemBackgroundHover
        {
            get { return (Brush)GetValue(ItemBackgroundHoverProperty); }
            set { SetValue(ItemBackgroundHoverProperty, value); }
        }

        public static readonly DependencyProperty ItemBackgroundHoverProperty =
            DependencyProperty.Register("ItemBackgroundHover", typeof(Brush), typeof(ListTabControl));

        public Brush ItemBackgroundPressed
        {
            get { return (Brush)GetValue(ItemBackgroundPressedProperty); }
            set { SetValue(ItemBackgroundPressedProperty, value); }
        }

        public static readonly DependencyProperty ItemBackgroundPressedProperty =
            DependencyProperty.Register("ItemBackgroundPressed", typeof(Brush), typeof(ListTabControl));

        public Brush ItemBackgroundDisable
        {
            get { return (Brush)GetValue(ItemBackgroundDisableProperty); }
            set { SetValue(ItemBackgroundDisableProperty, value); }
        }

        public static readonly DependencyProperty ItemBackgroundDisableProperty =
            DependencyProperty.Register("ItemBackgroundDisable", typeof(Brush), typeof(ListTabControl));

        public Brush ItemBorderBrushDefault
        {
            get { return (Brush)GetValue(ItemBorderBrushDefaultProperty); }
            set { SetValue(ItemBorderBrushDefaultProperty, value); }
        }

        public static readonly DependencyProperty ItemBorderBrushDefaultProperty =
            DependencyProperty.Register("ItemBorderBrushDefault", typeof(Brush), typeof(ListTabControl));

        public Brush ItemBorderBrushHover
        {
            get { return (Brush)GetValue(ItemBorderBrushHoverProperty); }
            set { SetValue(ItemBorderBrushHoverProperty, value); }
        }

        public static readonly DependencyProperty ItemBorderBrushHoverProperty =
            DependencyProperty.Register("ItemBorderBrushHover", typeof(Brush), typeof(ListTabControl));

        public Brush ItemBorderBrushPressed
        {
            get { return (Brush)GetValue(ItemBorderBrushPressedProperty); }
            set { SetValue(ItemBorderBrushPressedProperty, value); }
        }

        public static readonly DependencyProperty ItemBorderBrushPressedProperty =
            DependencyProperty.Register("ItemBorderBrushPressed", typeof(Brush), typeof(ListTabControl));

        public Brush ItemBorderBrushDisable
        {
            get { return (Brush)GetValue(ItemBorderBrushDisableProperty); }
            set { SetValue(ItemBorderBrushDisableProperty, value); }
        }

        public static readonly DependencyProperty ItemBorderBrushDisableProperty =
            DependencyProperty.Register("ItemBorderBrushDisable", typeof(Brush), typeof(ListTabControl));

        public Visibility UnSelectedState
        {
            get { return (Visibility)GetValue(UnSelectedStateProperty); }
            set { SetValue(UnSelectedStateProperty, value); }
        }

        public static readonly DependencyProperty UnSelectedStateProperty =
            DependencyProperty.Register("UnSelectedState", typeof(Visibility), typeof(ListTabControl), new PropertyMetadata(Visibility.Collapsed));

        public bool AnimationEnabled
        {
            get { return (bool)GetValue(AnimationEnabledProperty); }
            set { SetValue(AnimationEnabledProperty, value); }
        }

        public static readonly DependencyProperty AnimationEnabledProperty =
            DependencyProperty.Register("AnimationEnabled", typeof(bool), typeof(ListTabControl), new PropertyMetadata(true));

        public bool AnimationEnabledWhenShown
        {
            get { return (bool)GetValue(AnimationEnabledWhenShownProperty); }
            set { SetValue(AnimationEnabledWhenShownProperty, value); }
        }

        public static readonly DependencyProperty AnimationEnabledWhenShownProperty =
            DependencyProperty.Register("AnimationEnabledWhenShown", typeof(bool), typeof(ListTabControl), new PropertyMetadata(true));

        public bool AnimationSlide
        {
            get { return (bool)GetValue(AnimationSlideProperty); }
            set { SetValue(AnimationSlideProperty, value); }
        }

        public static readonly DependencyProperty AnimationSlideProperty =
            DependencyProperty.Register("AnimationSlide", typeof(bool), typeof(ListTabControl), new PropertyMetadata(true));

        public double AnimationSlideLength
        {
            get { return (double)GetValue(AnimationSlideLengthProperty); }
            set { SetValue(AnimationSlideLengthProperty, value); }
        }

        public static readonly DependencyProperty AnimationSlideLengthProperty =
            DependencyProperty.Register("AnimationSlideLength", typeof(double), typeof(ListTabControl), new PropertyMetadata(50.0));

        public IEnumerable TabsSource
        {
            get { return (IEnumerable)GetValue(TabsSourceProperty); }
            set { SetValue(TabsSourceProperty, value); }
        }

        public static readonly DependencyProperty TabsSourceProperty =
            DependencyProperty.Register("TabsSource", typeof(IEnumerable), typeof(ListTabControl), new PropertyMetadata((s, a) =>
            {
                var view = s as ListTabControl;
                view.SetTabsSource(a);
            }));

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(ListTabControl), new PropertyMetadata(-1, (s, a) =>
            {
                var c = (s as ListTabControl);
                c.Select((int)a.NewValue);
            }));

        public ListTabControlItemSelectorController SelectorController
        {
            get { return _SelectorController; }
            private set { _SelectorController = value; NotifyPropertyChanged(() => SelectorController); }
        }

        private ListTabControlItemSelectorController _SelectorController;

        public ListTabItem SelectedItem
        {
            get { return currentSelectedItem; }
            set { SetSelectedItem(value); }
        }

        public ListBoxEx HeaderContainer { get { return tabHeaders; } }

        #endregion Properties

        #region Fields

        private ListTabItem currentSelectedItem = null;
        //private int indexSelected = -1;
        //private int indexLeftCurrent = -1;
        //private int indexLeftLast = -1;

        private ListBoxEx tabHeaders;
        private ObservableCollection<object> listHeader;
        private List<ListTabItem> listTabs;
        private Dictionary<ListTabItem, bool> dicShown;

        private bool isTemplateApplied = false;
        private bool handleSelectable = true;

        #endregion Fields

        #region Events

        public event EventHandler SelectedIndexChanged;

        public event EventHandler<MsgEventArgs<ListTabItem>> TabShown;

        public event EventHandler ItemClick;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Ctor

        public ListTabControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            _SelectorController = new ListTabControlItemSelectorController(this);
            //MetroManager.TryInstallDesignStyles(this);
            listTabs = new List<ListTabItem>();
            listHeader = new ObservableCollection<object>();
            dicShown = new Dictionary<ListTabItem, bool>();
            (Items as INotifyCollectionChanged).CollectionChanged += MetroListTabControl_CollectionChanged;

            //Style = TryFindResource("Metro_ListTabControl_Style") as Style;

            isTemplateApplied = this.ApplyTemplate();
        }

        #endregion Ctor

        #region Methods

        #region Methods - Public

        public void SelectFirst()
        {
            foreach (var item in Items)
            {
                ListTabItem mlti = item as ListTabItem;
                if (mlti == null)
                    continue;
                if (!mlti.Selectable)
                    continue;
                if (!mlti.IsEnabled)
                    continue;

                tabHeaders.SelectedItem = mlti.ListBoxItem;
                mlti.IsSelected = true;

                currentSelectedItem = mlti;
                break;
            }
        }

        public void Select(string tabName)
        {
            foreach (var item in Items)
            {
                ListTabItem mlti = item as ListTabItem;
                if (mlti == null)
                    continue;
                if (!mlti.Selectable)
                    continue;
                if (!mlti.IsEnabled)
                    continue;
                if (mlti.Name != tabName)
                    continue;

                tabHeaders.SelectedItem = mlti.ListBoxItem;
                mlti.IsSelected = true;

                currentSelectedItem = mlti;
                break;
            }
        }

        #endregion Methods - Public

        #region Methods - Protected

        protected void NotifyPropertyChanged(string propertyName)
        {
            var evt = PropertyChanged;
            if (evt == null)
                return;

            evt(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void NotifyPropertyChanged<T>(System.Linq.Expressions.Expression<Func<T>> lambda)
        {
            LambdaPropertyNotifier.NotifyPropertyChanged(
                lambda, name => NotifyPropertyChanged(name));
        }

        #endregion Methods - Protected

        #region Methods - Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (tabHeaders != null)
            {
                tabHeaders.SelectionChanged -= tabs_SelectionChanged;
                tabHeaders.ItemsSource = null;
                listHeader.Clear();
                listTabs.Clear();
            }

            tabHeaders = GetTemplateChild("tabs") as ListBoxEx;
            if (tabHeaders == null)
                return;

            OnResetItem(Items);

            var val = SelectedIndex;

            tabHeaders.SelectionChanged += tabs_SelectionChanged;
            tabHeaders.ItemsSource = listHeader;
            tabHeaders.SelectedIndex = -1;

            Select(val);
        }

        #endregion Methods - Override

        #region Methods - Events

        private void tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool hasSelectable = listTabs.Any(c => c.Selectable);
            var id = listTabs.IndexOf(currentSelectedItem);

            int index = tabHeaders.SelectedIndex;
            if (index < 0 && hasSelectable)
            {
                tabHeaders.SelectedIndex = id;
                return;
            }
            else if (index < 0 && !hasSelectable)
            {
                SelectedIndex = -1;
                if (currentSelectedItem != null)
                {
                    currentSelectedItem.IsSelected = false;
                    currentSelectedItem = null;
                }

                RaiseSelectedIndexChangedEvent();
                return;
            }

            ListTabItem mlti = null;
            foreach (ListTabItem item in Items)
                if (item.ListBoxItem == tabHeaders.SelectedItem)
                {
                    mlti = item;
                    break;
                }

            if (mlti != null && mlti.Selectable)
            {
                if (!handleSelectable)
                    return;

                if (currentSelectedItem != null)
                    currentSelectedItem.IsSelected = false;

                mlti.Before = !AnimationEnabledWhenShown && !HasShown(mlti) ? (null as bool?) : (SelectedIndex <= index);
                SelectedIndex = index;
                currentSelectedItem = mlti;
                mlti.IsSelected = true;
                RaiseSelectedIndexChangedEvent();
                return;
            }

            handleSelectable = false;
            tabHeaders.SelectedIndex = SelectedIndex;
            handleSelectable = true;
            RaiseItemClickEvent(mlti == null ? null : mlti.Header);
        }

        private void MetroListTabControl_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    OnAddItem(e);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    OnRemoveItem(e);
                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Replace:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    OnClearItem(e);
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        private void OnResetItem(System.Collections.IList list)
        {
            int index = 0;
            foreach (object item in list)
                InstallItem(item, index++);
        }

        private void OnClearItem(NotifyCollectionChangedEventArgs e)
        {
            var list = listTabs.ToList();
            foreach (object item in list)
                UninstallItem(item);

            Select(SelectedIndex);
        }

        private void OnRemoveItem(NotifyCollectionChangedEventArgs e)
        {
            foreach (object item in e.OldItems)
                UninstallItem(item);

            Select(SelectedIndex);
        }

        private void OnAddItem(NotifyCollectionChangedEventArgs e)
        {
            int index = e.NewStartingIndex;
            foreach (object item in e.NewItems)
                InstallItem(item, index++);

            var id = listTabs.IndexOf(currentSelectedItem);
            if (id < 0)
                return;

            SelectedIndex = id;
        }

        #endregion Methods - Events

        #region Methods - Private

        private void RaiseSelectedIndexChangedEvent()
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, new EventArgs());

            if (currentSelectedItem == null)
                return;

            bool shown = false;
            if (dicShown.ContainsKey(currentSelectedItem))
                shown = dicShown[currentSelectedItem];

            if (!shown)
            {
                TryShown(currentSelectedItem);
                dicShown[currentSelectedItem] = true;
            }

            TryActivate(currentSelectedItem);
        }

        private bool HasShown(ListTabItem item)
        {
            if (item == null)
                return false;

            bool shown = false;
            if (dicShown.ContainsKey(item))
                shown = dicShown[item];

            return shown;
        }

        private void TryActivate(ListTabItem currentSelectedItem)
        {
            var l = currentSelectedItem.Content as ITabLifetime;
            l?.Activated();

            var f = currentSelectedItem.Content as FrameworkElement;
            if (f != null && f != f.DataContext)
            {
                l = f.DataContext as ITabLifetime;
                l?.Activated();
            }
        }

        private void TryShown(ListTabItem currentSelectedItem)
        {
            var l = currentSelectedItem.Content as ITabLifetime;
            l?.Shown();

            var f = currentSelectedItem.Content as FrameworkElement;
            if (f != null && f != f.DataContext)
            {
                l = f.DataContext as ITabLifetime;
                l?.Shown();
            }

            TabShown?.Invoke(this, new MsgEventArgs<ListTabItem>() { Parameter = currentSelectedItem });
        }

        private void RaiseItemClickEvent(object item)
        {
            if (ItemClick != null)
                ItemClick(item, new EventArgs());
        }

        private void InstallItem(object item, int index)
        {
            ListTabItem tab = item as ListTabItem;
            //if (tab == null && ItemTemplate != null)
            //    tab = ItemTemplate.LoadContent() as MetroListTabItem;
            if (tab == null)
                return;

            if (listTabs.Contains(tab))
                return;

            tab.UnSelectedState = UnSelectedState;
            tab.AnimationEnabled = AnimationEnabled;
            tab.AnimationSlide = AnimationSlide;
            tab.AnimationSlideLength = AnimationSlideLength;
            listTabs.Insert(index, tab);
            listHeader.Insert(index, tab.ListBoxItem);
            tab.Hide();

            if (tabHeaders != null && tabHeaders.SelectedIndex < 0)
                tabHeaders.SelectedIndex = 0;
        }

        private void UninstallItem(object item)
        {
            ListTabItem tab = item as ListTabItem;
            if (tab == null)
                return;

            dicShown.Remove(tab);
            listTabs.Remove(tab);
            listHeader.Remove(tab.ListBoxItem);

            if (tabHeaders == null)
                return;
        }

        private int Select(int newValue)
        {
            bool modify = false;

            if (newValue < -1)
            {
                newValue = -1;
                modify = true;
            }
            if (newValue >= listTabs.Count)
            {
                newValue = listTabs.Count - 1;
                modify = true;
            }

            if (tabHeaders == null)
                return newValue;

            tabHeaders.SelectedIndex = newValue;

            if (modify)
                SelectedIndex = newValue;

            return newValue;
        }

        private void SetSelectedItem(ListTabItem value)
        {
            if (value == null)
                return;
            if (!value.Selectable)
                return;
            if (!value.IsEnabled)
                return;

            if (!isTemplateApplied)
                isTemplateApplied = this.ApplyTemplate();

            tabHeaders.SelectedItem = value.ListBoxItem;
            currentSelectedItem = value;
        }

        private void SetTabsSource(DependencyPropertyChangedEventArgs a)
        {
            while (Items.Count > 0)
                Items.RemoveAt(0);

            Uninstall(a.OldValue as IEnumerable);

            var roots = a.NewValue as IEnumerable;
            if (roots == null)
                return;

            TraversalTabs(roots);

            Install(roots);
        }

        private void Uninstall(IEnumerable roots)
        {
            var ncc = roots as INotifyCollectionChanged;
            if (ncc != null)
                ncc.CollectionChanged -= Ncc_CollectionChanged;
        }

        private void Install(IEnumerable roots)
        {
            var ncc = roots as INotifyCollectionChanged;
            if (ncc != null)
                ncc.CollectionChanged += Ncc_CollectionChanged;
        }

        private void Ncc_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddChildren(e);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    RemoveChildren(e);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    ResetChildren(e);
                    break;

                case NotifyCollectionChangedAction.Move:
                //MoveChildren(e);
                //break;
                case NotifyCollectionChangedAction.Replace:
                default:
                    throw new NotSupportedException();
            }
        }

        private void TraversalTabs(IEnumerable roots)
        {
            foreach (var item in roots)
            {
                var tab = ItemTemplate.LoadContent() as ListTabItem;
                if (tab == null)
                    continue;

                tab.DataContext = item;
                Items.Add(tab);
            }
        }

        private void AddChildren(NotifyCollectionChangedEventArgs e)
        {
            int index = e.NewStartingIndex;

            foreach (var item in e.NewItems)
            {
                var tab = ItemTemplate.LoadContent() as ListTabItem;
                if (tab == null)
                    continue;

                tab.DataContext = item;
                Items.Insert(index++, tab);
            }
        }

        private void RemoveChildren(NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.OldItems)
            {
                var tab = listTabs.FirstOrDefault(c => c.DataContext == item);
                Items.Remove(tab);
            }
        }

        private void ResetChildren(NotifyCollectionChangedEventArgs e)
        {
            Items.Clear();
        }

        #endregion Methods - Private

        #endregion Methods
    }
}