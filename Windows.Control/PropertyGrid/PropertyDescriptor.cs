using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Windows.Core;

namespace Windows.Control
{
    public class PropertyDescriptor : INotifyPropertyChanged, IDisposable
    {
        #region Properties

        public string Gallery { get; set; }
        public string Name { get; set; }

        public string AliasName
        {
            get { return _AliasName; }
            set { _AliasName = value; NotifyPropertyChanged("AliasName"); }
        }

        public string Description { get; set; }
        public string Watermask { get; set; }
        public int Length { get; set; }

        public bool Editable { get; set; }

        public bool Required
        {
            get { return _Required; }
            set { _Required = value; NotifyPropertyChanged("Required"); }
        }

        private bool _Required;

        public bool Nullable
        {
            get { return _Nullable; }
            set { _Nullable = value; NotifyPropertyChanged("Nullable"); }
        }

        private bool _Nullable;

        public eDataType Type { get; set; }

        public UIElement Designer
        {
            get { return _Designer; }
            set { _Designer = value; BindingExpression = null; }
        }

        private UIElement _Designer;
        public PropertyInfo PropertyInfo { get; private set; }
        public object Object { get; set; }
        public ImageSource Image { get; set; }
        public PropertyTrigger Trigger { get; private set; }
        public PropertyGrid PropertyGrid { get; internal set; }

        public BindingExpressionBase BindingExpression { get; set; }

        public ImageSource ImageState
        {
            get { return _ImageState; }
            set { _ImageState = value; NotifyPropertyChanged("ImageState"); }
        }

        public object DefaultValue
        {
            get { return _valueDefault; }
            set { _valueDefault = value; NotifyPropertyChanged("DefaultValue"); }
        }

        public object Value
        {
            get { return _value; }
            set { SetValue(value); }
        }

        public Visibility ImageVisibility
        {
            get { return IsBusy ? Visibility.Hidden : Visibility.Visible; }
        }

        public Visibility LoadingVisibility
        {
            get { return IsBusy ? Visibility.Visible : Visibility.Hidden; }
        }

        public bool IsBusy
        {
            get { return GetIsBusy(); ; }
            set { SetIsBusy(value); }
        }

        public Visibility Visibility
        {
            get { return _Visibility; }
            set { _Visibility = value; NotifyPropertyChanged("Visibility"); }
        }

        public eMessageGrade Grade
        {
            get { return _Grade; }
            set { _Grade = value; NotifyPropertyChanged("Grade"); }
        }

        private eMessageGrade _Grade;

        public string DescriptionState
        {
            get { return _DescriptionState; }
            set { _DescriptionState = value; NotifyPropertyChanged("DescriptionState"); }
        }

        private string _DescriptionState;

        #endregion Properties

        #region Fields

        private static Dictionary<Type, PropertyDescriptorCreatorHandlerAttribute> handlers = null;

        //private DetentionReporter reporter;
        //private AutoResetEvent are;
        //private Exception exSetValue = null;
        private TaskQueue tq = new TaskQueueDispatcher(Application.Current.Dispatcher);

        private object _value;
        private object _valueDefault;
        private ImageSource _ImageState;
        private int cntBusy;
        private Visibility _Visibility;
        private String _AliasName;

        #endregion Fields

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Ctor

        static PropertyDescriptor()
        {
            handlers = PropertyDescriptorCreatorHandlerAttribute.Create(typeof(PropertyDescriptor));
        }

        public PropertyDescriptor(object obj, string propertyName)
        {
            //reporter = DetentionReporter.Create(c => SetObjectPropertyValue(c), 1, 1, false);
            //are = new AutoResetEvent(false);

            Editable = true;

            var pi = obj.GetType().GetProperty(propertyName);
            var dt = pi.PropertyType.IsNullableGeneric() ?
                DotNetTypeAttribute.GetType(pi.PropertyType.GetGenericTypeInNullable()) :
                DotNetTypeAttribute.GetType(pi.PropertyType);

            Object = obj;
            PropertyInfo = pi;
            Type = dt;
            Name = pi.Name;
            Nullable = pi.PropertyType.IsNullable();

            var ra = pi.GetAttribute<RequiredAttribute>();
            Nullable = Nullable ? ra == null : Nullable;
            Required = ra != null;

            var dta = pi.GetAttribute<DataColumnAttribute>();
            Nullable = Nullable && dta != null ? dta.Nullable : Nullable;

            var wa = pi.GetAttribute<WatermaskLanguageAttribute>();
            Watermask = wa == null ? string.Empty : wa.Name;

            var ga = pi.GetAttribute<GalleryLanguageAttribute>();
            Gallery = ga == null ? string.Empty : ga.Name;

            var da = pi.GetAttribute<DisplayLanguageAttribute>();
            AliasName = da == null ? null : da.Name;
            var da1 = pi.GetAttribute<DisplayNameAttribute>();
            AliasName = AliasName.IsNullOrBlank() && da1 != null ? da1.DisplayName : AliasName;
            var da2 = pi.GetAttribute<DisplayAttribute>();
            AliasName = AliasName.IsNullOrBlank() && da2 != null ? da2.Name : AliasName;
            AliasName = AliasName.IsNullOrBlank() ? pi.Name : AliasName;

            var pa = pi.GetAttribute<DescriptionLanguageAttribute>();
            Description = pa == null ? AliasName : pa.Description;

            var ppa = pi.GetAttribute<PropertyDescriptorAttribute>();
            Image = ppa != null && !ppa.UriImage16.IsNullOrBlank() ?
                BitmapFrame.Create(new Uri(ppa.UriImage16)) : null;

            var ia = pi.GetAttribute<ImageAttribute>();
            Image = Image == null ? (ia == null || ia.ImageUri.IsNullOrBlank() ?
                Image : (BitmapFrame.Create(new Uri(ia.ImageUri)))) : Image;

            var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
            if (pda != null)
            {
                var t = pda.CreateTrigger();
                if (t != null)
                    Trigger = t;
            }

            if (Trigger == null)
                Trigger = new PropertyTrigger();

            _value = obj.GetPropertyValue(propertyName);

            var b = new Binding("Value");
            b.Source = this;

            var tb = new TextBlock();
            tb.HorizontalAlignment = HorizontalAlignment.Stretch;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.SetBinding(TextBlock.TextProperty, b);
            tb.SetBinding(TextBlock.ToolTipProperty, b);

            Designer = tb;
        }

        internal void Install()
        {
            var nt = Object as INotifyPropertyChanged;
            if (nt == null)
                return;
            nt.PropertyChanged += nt_PropertyChanged;
        }

        internal void Uninstall()
        {
            var nt = Object as INotifyPropertyChanged;
            if (nt == null)
                return;
            nt.PropertyChanged -= nt_PropertyChanged;
        }

        #endregion Ctor

        #region Methods

        #region Methods - Static

        public static PropertyDescriptor Create(PropertyGrid pg, object obj, string propertyName, bool editable, string propClass, ePropertyGridContainerType type)
        {
            var pi = obj.GetType().GetProperty(propertyName);

            var ea = pi.GetAttribute<EnabledAttribute>();
            if (ea != null && !ea.Value)
                return null;

            var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
            if (propClass != "*" && pda.Class != propClass)
                return null;

            if ((!editable || (pda != null && !pda.Editable)) && (
                type == ePropertyGridContainerType.Details ||
                type == ePropertyGridContainerType.DetailsWithoutScrollViewer))
                return new PropertyDescriptorReadOnlyNoFrame(pg, obj, propertyName);

            if (!editable || (pda != null && !pda.Editable))
                return new PropertyDescriptorReadOnly(pg, obj, propertyName);

            if (!handlers.ContainsKey(pi.PropertyType))
                return new PropertyDescriptor(obj, propertyName);

            return handlers[pi.PropertyType].Method.Invoke(
                null, new object[] { pg, obj, propertyName }) as PropertyDescriptor;
        }

        [PropertyDescriptorCreatorHandler(typeof(string))]
        private static PropertyDescriptor CreateString(PropertyGrid pg, object obj, string propertyName)
        {
            return new PropertyDescriptorString(pg, obj, propertyName);
        }

        #endregion Methods - Static

        #region Methods - Internal

        internal void PostPropertyValueChangedAsync(string propertyName, bool install)
        {
            if (Trigger == null)
                return;

            try
            {
                PropertyGrid.Dispatcher.Invoke(new Action(() => { IsBusy = true; }));

                if (install)
                    Trigger.PostPropertyValueInstalled(this, propertyName);
                else
                    Trigger.PostPropertyValueChanged(this, propertyName);

                //if (propertyName == Name)
                //    Trigger.RefreshPropertyState(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                PropertyGrid.Dispatcher.Invoke(new Action(() => { IsBusy = false; }));
            }
        }

        #endregion Methods - Internal

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

        private void nt_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!PropertyGrid.PropertyDescriptors.Any(c => c.Name == e.PropertyName))
                return;

            var broadcast = false;

            if (Application.Current.Dispatcher.CheckAccess())
                broadcast = PropertyGrid.BroadcastPropertyChanged;
            else
                Application.Current.Dispatcher.Invoke(
                    new Action(() => broadcast = PropertyGrid.BroadcastPropertyChanged));

            if (broadcast)
                tq.Do(go => { PostPropertyValueChangedAsync(e.PropertyName, false); });

            if (e.PropertyName != Name)
                return;

            var val = Object.GetPropertyValue(e.PropertyName);
            //if (object.Equals(val, Value))
            //    return;

            _value = val;
            NotifyPropertyChanged("Value");
        }

        #endregion Methods - Events

        #region Methods - Private

        public void Dispose()
        {
            //reporter.Dispose();
            //reporter = null;
        }

        private void SetValue(object value)
        {
            _value = value;
            NotifyPropertyChanged("Value");

            Object.SetPropertyValue(Name, value);

            var info = Object as IDataErrorInfo;
            if (info != null)
            {
                var piGet = info.GetType().GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);
                var error = piGet.Invoke(Object, new object[] { Name }) as string;
                if (!error.IsNullOrBlank())
                    throw new Exception(error);
            }
        }

        private void SetIsBusy(bool value)
        {
            lock (this)
                cntBusy += value ? 1 : -1;

            NotifyPropertyChanged("IsBusy");
            NotifyPropertyChanged("ImageVisibility");
            NotifyPropertyChanged("LoadingVisibility");
        }

        private bool GetIsBusy()
        {
            lock (this)
                return cntBusy > 0;
        }

        #endregion Methods - Private

        #endregion Methods
    }

    public class PropertyDescriptorReadOnly : PropertyDescriptor
    {
        #region Ctor

        public PropertyDescriptorReadOnly(PropertyGrid pg, object obj, string propertyName)
            : base(obj, propertyName)
        {
            int maxLength = int.MaxValue;
            var pi = obj.GetType().GetProperty(propertyName);
            var attrColumn = pi.GetAttribute<DataColumnAttribute>();
            if (attrColumn != null)
                maxLength = attrColumn.Size <= 0 ? int.MaxValue : attrColumn.Size / 2;
            var lengthAttr = pi.GetAttribute<StringLengthAttribute>();
            if (lengthAttr != null)
                maxLength = maxLength > lengthAttr.MaximumLength ? lengthAttr.MaximumLength : maxLength;
            var pda = pi.GetAttribute<PropertyDescriptorAttribute>();

            Editable = false;

            var b = new Binding("Value");
            b.Source = this;
            b.Mode = BindingMode.TwoWay;
            b.ValidatesOnExceptions = true;
            b.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            if (pda != null && pda.Converter != null)
            {
                b.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
                b.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
            }

            var tb = new TextBox();
            tb.IsReadOnly = true;
            tb.MaxLength = maxLength;
            tb.VerticalContentAlignment = VerticalAlignment.Center;
            tb.SetBinding(TextBox.TextProperty, b);
            tb.SetBinding(TextBox.ToolTipProperty, b);

            Designer = tb;
            BindingExpression = tb.GetBindingExpression(TextBox.TextProperty);
        }

        #endregion Ctor
    }

    public class PropertyDescriptorReadOnlyNoFrame : PropertyDescriptor
    {
        #region Ctor

        public PropertyDescriptorReadOnlyNoFrame(PropertyGrid pg, object obj, string propertyName)
            : base(obj, propertyName)
        {
            int maxLength = int.MaxValue;
            var pi = obj.GetType().GetProperty(propertyName);
            var attrColumn = pi.GetAttribute<DataColumnAttribute>();
            if (attrColumn != null)
                maxLength = attrColumn.Size <= 0 ? int.MaxValue : attrColumn.Size / 2;
            var lengthAttr = pi.GetAttribute<StringLengthAttribute>();
            if (lengthAttr != null)
                maxLength = maxLength > lengthAttr.MaximumLength ? lengthAttr.MaximumLength : maxLength;
            var pda = pi.GetAttribute<PropertyDescriptorAttribute>();

            Editable = false;

            var b = new Binding("Value");
            b.Source = this;
            b.Mode = BindingMode.TwoWay;
            b.ValidatesOnExceptions = true;
            b.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            if (pda != null && pda.Converter != null)
            {
                b.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
                b.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
            }

            var tb = new TextBox();
            tb.IsReadOnly = true;
            tb.MaxLength = maxLength;
            tb.Background = Brushes.Transparent;
            tb.BorderThickness = new Thickness(0);

            if (pda != null && pda.Height > 30)
            {
                tb.VerticalContentAlignment = VerticalAlignment.Stretch;
                tb.AcceptsReturn = true;
            }

            tb.SetBinding(TextBox.TextProperty, b);
            tb.SetBinding(TextBox.ToolTipProperty, b);

            Designer = tb;
            BindingExpression = tb.GetBindingExpression(TextBox.TextProperty);
        }

        #endregion Ctor
    }

    public class PropertyDescriptorString : PropertyDescriptor
    {
        #region Ctor

        public PropertyDescriptorString(PropertyGrid pg, object obj, string propertyName)
            : base(obj, propertyName)
        {
            int maxLength = int.MaxValue;
            var pi = obj.GetType().GetProperty(propertyName);
            var attrColumn = pi.GetAttribute<DataColumnAttribute>();
            if (attrColumn != null)
                maxLength = attrColumn.Size <= 0 ? int.MaxValue : attrColumn.Size / 2;
            var lengthAttr = pi.GetAttribute<StringLengthAttribute>();
            if (lengthAttr != null)
                maxLength = maxLength > lengthAttr.MaximumLength ? lengthAttr.MaximumLength : maxLength;
            var pda = pi.GetAttribute<PropertyDescriptorAttribute>();

            var b = new Binding("Value");
            b.Source = this;
            b.Mode = BindingMode.TwoWay;
            b.ValidatesOnExceptions = true;
            b.UpdateSourceTrigger = pda == null ? UpdateSourceTrigger.LostFocus : pda.UpdateSourceTrigger;
            if (pda != null && pda.Converter != null)
            {
                b.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
                b.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
            }

            var tb = new TextBox();
            tb.MaxLength = maxLength;
            tb.VerticalContentAlignment = VerticalAlignment.Center;

            if (pda != null && pda.Height > 30)
            {
                tb.VerticalContentAlignment = VerticalAlignment.Stretch;
                tb.AcceptsReturn = true;
            }

            tb.SetBinding(TextBox.TextProperty, b);
            tb.SetBinding(TextBox.ToolTipProperty, b);

            Designer = tb;
            BindingExpression = tb.GetBindingExpression(TextBox.TextProperty);
        }

        #endregion Ctor
    }
}