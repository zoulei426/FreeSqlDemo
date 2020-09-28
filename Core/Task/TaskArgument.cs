using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Core
{
    [Serializable]
    public class TaskArgument : CDObject, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Properties

        public object UserState
        {
            get { return _UserState; }
            set { _UserState = value; NotifyPropertyChanged("UserState"); }
        }

        public Dictionary<string, object> Properties
        {
            get { return _Properties; }
            set { _Properties = value; NotifyPropertyChanged("Properties"); }
        }

        public virtual string Error { get { return _Error; } }
        private string _Error;

        public virtual string this[string columnName] { get { return Validate(columnName); } }

        #endregion Properties

        #region Fields

        internal object _UserState;
        private Dictionary<string, object> _Properties;
        private Dictionary<string, PropertyChangedHandlerAttribute> handlers;

        #endregion Fields

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Ctor

        public TaskArgument()
        {
            handlers = PropertyChangedHandlerAttribute.Create(this.GetType());
            Properties = new Dictionary<string, object>();
            PropertyChanged += TaskArgument_PropertyChanged;
        }

        #endregion Ctor

        #region Methods

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

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        protected virtual string OnValidate(string columnName)
        {
            return null;
        }

        #endregion Methods - Protected

        #region Methods - Events

        private void TaskArgument_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(sender, e);

            if (!handlers.ContainsKey(e.PropertyName))
                return;

            handlers[e.PropertyName].Method.Invoke(
                this, new object[] { e.PropertyName, this.GetPropertyValue(e.PropertyName) });
        }

        #endregion Methods - Events

        #region Methods - Private

        private string Validate(string columnName)
        {
            try
            {
                if (new System.Diagnostics.StackTrace().GetFrames().
                    Any(c => c.GetMethod().Name == "ValidateOnTargetUpdated"))
                    return null;

                var attrs = this.GetType().GetProperty(
                    columnName).GetAttributes<ValidationAttribute>();

                foreach (var item in attrs)
                {
                    if (item.IsValid(this.GetPropertyValue(columnName)))
                        continue;

                    _Error = item.ErrorMessage;
                    return _Error;
                }

                return OnValidate(columnName);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion Methods - Private

        #endregion Methods
    }
}