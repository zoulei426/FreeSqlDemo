using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;

namespace Core
{
    /// <summary>
    /// INotifyPropertyChanged实现类
    /// </summary>
    [Serializable]
    public class NotifyCDObject : CDObject, INotifyPropertyChanged
    {
        #region Events

        /// <summary>
        /// 属性变化事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Ctor

        /// <summary>
        /// 构造
        /// </summary>
        public NotifyCDObject()
        {
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

        protected void NotifyPropertyChanged<T>(Expression<Func<T>> lambda)
        {
            LambdaPropertyNotifier.NotifyPropertyChanged(
                lambda, name => NotifyPropertyChanged(name));
        }

        #endregion Methods - Protected

        #endregion Methods
    }
}