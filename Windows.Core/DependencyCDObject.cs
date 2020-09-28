using System;
using System.Reflection;
using System.Windows;
using Core;

namespace Windows.Core
{
    public abstract class DependencyCDObject : DependencyObject, ICloneable, IDisposable
    {
        #region Ctor

        public DependencyCDObject()
        {
        }

        #endregion Ctor

        #region Methods

        #region Methods - Virtual

        public virtual object Clone()
        {
            object newObj = MemberwiseClone();

            newObj.TraversalPropertiesInfo(ClonePropertyHandler, newObj);

            return newObj;
        }

        public virtual void Dispose()
        {
            //this.TraversalPropertiesInfo(DisposePropertyHandler);
        }

        #endregion Methods - Virtual

        #region Methods - Private

        private bool ClonePropertyHandler(PropertyInfo pi, object value, object target)
        {
            if (!pi.CanWrite)
                return true;

            pi.SetValue(target, CDObject.TryClone(value), null);

            return true;
        }

        private bool DisposePropertyHandler(string name, object value)
        {
            IDisposable id = value as IDisposable;
            if (id == null)
                return true;

            id.Dispose();

            return true;
        }

        #endregion Methods - Private

        #endregion Methods
    }
}