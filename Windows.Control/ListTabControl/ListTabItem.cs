using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;

namespace Windows.Control
{
    public class ListTabItem : ContentControl, INotifyPropertyChanged
    {
        #region Properties

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(ListTabItem), new UIPropertyMetadata((s, a) =>
            {
                var item = s as ListTabItem;
                item.OnChildChanged(a.OldValue, a.NewValue);
            }));

        public bool Selectable { get; set; }

        public bool IsSelected
        {
            get { return _IsSelected; }
            internal set { SetIsSelected(value); }
        }

        internal ListBoxItem ListBoxItem { get; private set; }
        internal Visibility UnSelectedState { get; set; }
        internal bool AnimationEnabled { get; set; }
        internal bool AnimationSlide { get; set; }
        internal double AnimationSlideLength { get; set; }
        internal bool? Before { get; set; }

        #endregion Properties

        #region Fields

        private bool hasAnimation = false;
        private bool _IsSelected;

        #endregion Fields

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Ctor

        public ListTabItem()
        {
            AnimationSlideLength = 50.0;
            SnapsToDevicePixels = true;
            Selectable = true;
            Focusable = false;
            UnSelectedState = Visibility.Collapsed;

            ListBoxItem = new ListBoxItem();
            ListBoxItem.Tag = this;
            ListBoxItem.SetResourceReference(ListBoxItem.StyleProperty, "Metro_ListTabControl_BoxItem_Style");
            //ListBoxItem.Style = Application.Current.TryFindResource("Metro_ListTabControl_BoxItem_Style") as Style;

            Binding b = new Binding("Header");
            b.Source = this;
            b.Mode = BindingMode.TwoWay;
            ListBoxItem.SetBinding(ListBoxItem.ContentProperty, b);

            b = new Binding("IsEnabled");
            b.Source = this;
            b.Mode = BindingMode.TwoWay;
            ListBoxItem.SetBinding(ListBoxItem.IsEnabledProperty, b);

            b = new Binding("DataContext");
            b.Source = this;
            b.Mode = BindingMode.TwoWay;
            ListBoxItem.SetBinding(ListBoxItem.DataContextProperty, b);

            //Selected += MetroListTabItem_Selected;
            //Unselected += MetroListTabItem_Unselected;
        }

        #endregion Ctor

        #region Methods

        #region Methods - Public

        public void Hide()
        {
            var element = this;
            element.Opacity = 0;
            element.Visibility = UnSelectedState;
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

        #endregion Methods - Protected

        #region Methods - Events

        private void MetroListTabItem_Unselected(object sender, RoutedEventArgs e)
        {
            BeginHideAnimation();
        }

        private void MetroListTabItem_Selected(object sender, RoutedEventArgs e)
        {
            BeginShowAnimation();
        }

        #endregion Methods - Events

        #region Method - Private

        private void OnChildChanged(object oldValue, object newValue)
        {
            //base.RemoveLogicalChild(oldValue);
            //base.AddLogicalChild(newValue);
        }

        private void SetIsSelected(bool value)
        {
            _IsSelected = value;

            if (value)
                BeginShowAnimation();
            else
                BeginHideAnimation();

            NotifyPropertyChanged("IsSelected");
        }

        private void BeginShowAnimation()
        {
            if (!AnimationEnabled)
            {
                var element = this;
                element.Opacity = 1;
                element.Visibility = System.Windows.Visibility.Visible;
                element.Margin = new Thickness(0);
                return;
            }

            lock (this)
            {
                if (!AnimationSlide)
                {
                    var element = this;

                    element.Opacity = 0;
                    element.Visibility = System.Windows.Visibility.Visible;

                    element.BeginAnimation(FrameworkElement.OpacityProperty, null);

                    var da = new DoubleAnimation(0, 1, MetroParameters.AnimationDurationX4);
                    element.BeginAnimation(FrameworkElement.OpacityProperty, da);

                    var da2 = new ThicknessAnimation(new Thickness(20, 0, -20, 0), new Thickness(0), MetroParameters.AnimationDurationX4);
                    da2.AccelerationRatio = .3;
                    da2.DecelerationRatio = .7;
                    element.BeginAnimation(FrameworkElement.MarginProperty, da2);

                    hasAnimation = true;
                }
                else if (AnimationSlide && !Before.HasValue)
                {
                    var element = this;
                    element.Opacity = 1;
                    element.Visibility = System.Windows.Visibility.Visible;
                    element.Margin = new Thickness(0);
                }
                else if (AnimationSlide && Before.Value)
                {
                    var element = this;

                    element.Opacity = 0;
                    element.Visibility = System.Windows.Visibility.Visible;

                    element.BeginAnimation(FrameworkElement.OpacityProperty, null);

                    var da = new DoubleAnimation(0, 1, MetroParameters.AnimationDurationX4);
                    element.BeginAnimation(FrameworkElement.OpacityProperty, da);

                    var da2 = new ThicknessAnimation(new Thickness(AnimationSlideLength, 0, -AnimationSlideLength, 0), new Thickness(0), MetroParameters.AnimationDurationX8);
                    da2.AccelerationRatio = .3;
                    da2.DecelerationRatio = .7;
                    element.BeginAnimation(FrameworkElement.MarginProperty, da2);

                    hasAnimation = true;
                }
                else
                {
                    var element = this;

                    element.Opacity = 0;
                    element.Visibility = System.Windows.Visibility.Visible;

                    element.BeginAnimation(FrameworkElement.OpacityProperty, null);

                    var da = new DoubleAnimation(0, 1, MetroParameters.AnimationDurationX4);
                    element.BeginAnimation(FrameworkElement.OpacityProperty, da);

                    var da2 = new ThicknessAnimation(new Thickness(-AnimationSlideLength, 0, AnimationSlideLength, 0), new Thickness(0), MetroParameters.AnimationDurationX8);
                    da2.AccelerationRatio = .3;
                    da2.DecelerationRatio = .7;
                    element.BeginAnimation(FrameworkElement.MarginProperty, da2);

                    hasAnimation = true;
                }
            }
        }

        private void BeginHideAnimation()
        {
            if (!AnimationEnabled)
            {
                var element = this;
                element.Opacity = 0;
                element.Visibility = UnSelectedState;
                return;
            }

            lock (this)
            {
                var element = this;

                element.BeginAnimation(FrameworkElement.OpacityProperty, null);

                var da = new DoubleAnimation(1, 0, MetroParameters.AnimationDurationX4);
                da.Completed += (s, e) =>
                {
                    lock (this)
                    {
                        if (hasAnimation)
                            return;
                        element.Visibility = UnSelectedState;
                    }
                };

                element.BeginAnimation(FrameworkElement.OpacityProperty, da);
                hasAnimation = false;
            }
        }

        #endregion Method - Private

        #endregion Methods
    }
}