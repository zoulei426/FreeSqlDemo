using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Windows.Core
{
    public abstract class NotifyDependencyCDObject : DependencyCDObject, INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Ctor

        public NotifyDependencyCDObject()
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

        protected void NotifyPropertyChanged<T>(System.Linq.Expressions.Expression<Func<T>> lambda)
        {
            LambdaPropertyNotifier.NotifyPropertyChanged(
                lambda, name => NotifyPropertyChanged(name));
        }

        #endregion Methods - Protected

        #endregion Methods
    }
}