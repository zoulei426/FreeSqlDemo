using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Windows.Control
{
    public class ListBoxEx : ListBox
    {
        #region Properties

        public bool CanScrollContent
        {
            get { return (bool)GetValue(CanScrollContentProperty); }
            set { SetValue(CanScrollContentProperty, value); }
        }

        public static readonly DependencyProperty CanScrollContentProperty =
            DependencyProperty.Register("CanScrollContent", typeof(bool), typeof(ListBoxEx), new PropertyMetadata(true));

        public Thickness GridLineThickness
        {
            get { return (Thickness)GetValue(GridLineThicknessProperty); }
            set { SetValue(GridLineThicknessProperty, value); }
        }

        public static readonly DependencyProperty GridLineThicknessProperty =
            DependencyProperty.Register("GridLineThickness", typeof(Thickness), typeof(ListBoxEx));

        public Thickness ItemBorderThickness
        {
            get { return (Thickness)GetValue(ItemBorderThicknessProperty); }
            set { SetValue(ItemBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty ItemBorderThicknessProperty =
            DependencyProperty.Register("ItemBorderThickness", typeof(Thickness), typeof(ListBoxEx));

        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register("ItemMargin", typeof(Thickness), typeof(ListBoxEx));

        public Thickness ItemPadding
        {
            get { return (Thickness)GetValue(ItemPaddingProperty); }
            set { SetValue(ItemPaddingProperty, value); }
        }

        public static readonly DependencyProperty ItemPaddingProperty =
            DependencyProperty.Register("ItemPadding", typeof(Thickness), typeof(ListBoxEx));

        public Brush ItemForegroundDefault
        {
            get { return (Brush)GetValue(ItemForegroundDefaultProperty); }
            set { SetValue(ItemForegroundDefaultProperty, value); }
        }

        public static readonly DependencyProperty ItemForegroundDefaultProperty =
            DependencyProperty.Register("ItemForegroundDefault", typeof(Brush), typeof(ListBoxEx));

        public Brush ItemForegroundHover
        {
            get { return (Brush)GetValue(ItemForegroundHoverProperty); }
            set { SetValue(ItemForegroundHoverProperty, value); }
        }

        public static readonly DependencyProperty ItemForegroundHoverProperty =
            DependencyProperty.Register("ItemForegroundHover", typeof(Brush), typeof(ListBoxEx));

        public Brush ItemForegroundPressed
        {
            get { return (Brush)GetValue(ItemForegroundPressedProperty); }
            set { SetValue(ItemForegroundPressedProperty, value); }
        }

        public static readonly DependencyProperty ItemForegroundPressedProperty =
            DependencyProperty.Register("ItemForegroundPressed", typeof(Brush), typeof(ListBoxEx));

        public Brush ItemForegroundDisable
        {
            get { return (Brush)GetValue(ItemForegroundDisableProperty); }
            set { SetValue(ItemForegroundDisableProperty, value); }
        }

        public static readonly DependencyProperty ItemForegroundDisableProperty =
            DependencyProperty.Register("ItemForegroundDisable", typeof(Brush), typeof(ListBoxEx));

        public Brush ItemBackgroundDefault
        {
            get { return (Brush)GetValue(ItemBackgroundDefaultProperty); }
            set { SetValue(ItemBackgroundDefaultProperty, value); }
        }

        public static readonly DependencyProperty ItemBackgroundDefaultProperty =
            DependencyProperty.Register("ItemBackgroundDefault", typeof(Brush), typeof(ListBoxEx));

        public Brush ItemBackgroundHover
        {
            get { return (Brush)GetValue(ItemBackgroundHoverProperty); }
            set { SetValue(ItemBackgroundHoverProperty, value); }
        }

        public static readonly DependencyProperty ItemBackgroundHoverProperty =
            DependencyProperty.Register("ItemBackgroundHover", typeof(Brush), typeof(ListBoxEx));

        public Brush ItemBackgroundPressed
        {
            get { return (Brush)GetValue(ItemBackgroundPressedProperty); }
            set { SetValue(ItemBackgroundPressedProperty, value); }
        }

        public static readonly DependencyProperty ItemBackgroundPressedProperty =
            DependencyProperty.Register("ItemBackgroundPressed", typeof(Brush), typeof(ListBoxEx));

        public Brush ItemBackgroundDisable
        {
            get { return (Brush)GetValue(ItemBackgroundDisableProperty); }
            set { SetValue(ItemBackgroundDisableProperty, value); }
        }

        public static readonly DependencyProperty ItemBackgroundDisableProperty =
            DependencyProperty.Register("ItemBackgroundDisable", typeof(Brush), typeof(ListBoxEx));

        public Brush ItemBorderBrushDefault
        {
            get { return (Brush)GetValue(ItemBorderBrushDefaultProperty); }
            set { SetValue(ItemBorderBrushDefaultProperty, value); }
        }

        public static readonly DependencyProperty ItemBorderBrushDefaultProperty =
            DependencyProperty.Register("ItemBorderBrushDefault", typeof(Brush), typeof(ListBoxEx));

        public Brush ItemBorderBrushHover
        {
            get { return (Brush)GetValue(ItemBorderBrushHoverProperty); }
            set { SetValue(ItemBorderBrushHoverProperty, value); }
        }

        public static readonly DependencyProperty ItemBorderBrushHoverProperty =
            DependencyProperty.Register("ItemBorderBrushHover", typeof(Brush), typeof(ListBoxEx));

        public Brush ItemBorderBrushPressed
        {
            get { return (Brush)GetValue(ItemBorderBrushPressedProperty); }
            set { SetValue(ItemBorderBrushPressedProperty, value); }
        }

        public static readonly DependencyProperty ItemBorderBrushPressedProperty =
            DependencyProperty.Register("ItemBorderBrushPressed", typeof(Brush), typeof(ListBoxEx));

        public Brush ItemBorderBrushDisable
        {
            get { return (Brush)GetValue(ItemBorderBrushDisableProperty); }
            set { SetValue(ItemBorderBrushDisableProperty, value); }
        }

        public static readonly DependencyProperty ItemBorderBrushDisableProperty =
            DependencyProperty.Register("ItemBorderBrushDisable", typeof(Brush), typeof(ListBoxEx));

        public HorizontalAlignment ItemHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(ItemHorizontalAlignmentProperty); }
            set { SetValue(ItemHorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty ItemHorizontalAlignmentProperty =
            DependencyProperty.Register("ItemHorizontalAlignment", typeof(HorizontalAlignment), typeof(ListBoxEx), new PropertyMetadata(HorizontalAlignment.Stretch));

        public VerticalAlignment ItemVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(ItemVerticalAlignmentProperty); }
            set { SetValue(ItemVerticalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty ItemVerticalAlignmentProperty =
            DependencyProperty.Register("ItemVerticalAlignment", typeof(VerticalAlignment), typeof(ListBoxEx), new PropertyMetadata(VerticalAlignment.Stretch));

        public Visibility GridLineVisibility
        {
            get { return (Visibility)GetValue(GridLineVisibilityProperty); }
            set { SetValue(GridLineVisibilityProperty, value); }
        }

        public static readonly DependencyProperty GridLineVisibilityProperty =
            DependencyProperty.Register("GridLineVisibility", typeof(Visibility), typeof(ListBoxEx), new PropertyMetadata(Visibility.Collapsed));

        public ScrollViewer ScrollViewer { get; private set; }

        #endregion Properties

        #region Ctor

        public ListBoxEx()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            //Style = TryFindResource("Metro_ListBox_Style") as Style;
        }

        #endregion Ctor

        #region Methods

        #region Methods - Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ScrollViewer = GetTemplateChild("ScrollViewer") as ScrollViewer;
        }

        #endregion Methods - Override

        #endregion Methods
    }
}