using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Core
{
    /// <summary>
    /// 核心类
    /// </summary>
    [Serializable]
    public class CDObject : ICloneable, IDisposable
    {
        #region Methods

        #region Methods - Static

        public static object TryClone(object obj)
        {
            ICloneable ic = obj as ICloneable;
            if (ic != null)
                return ic.Clone();

            IList list = obj as IList;
            if (list != null)
                return list.Clone();

            return obj;
        }

        #endregion Methods - Static

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

            pi.SetValue(target, TryClone(value), null);

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