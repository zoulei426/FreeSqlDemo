using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core
{
    public static class ICustomAttributeExtension
    {
        #region Methods

        public static T GetAttribute<T>(this ICustomAttributeProvider source) where T : Attribute
        {
            object[] objs = source.GetCustomAttributes(typeof(T), true);
            if (objs.Length == 0)
                return null;

            return (T)objs[0];
        }

        public static T[] GetAttributes<T>(this ICustomAttributeProvider source) where T : Attribute
        {
            object[] objs = source.GetCustomAttributes(typeof(T), true);

            List<T> listAttr = new List<T>();
            foreach (object obj in objs)
                listAttr.Add((T)obj);

            return listAttr.ToArray<T>();
        }

        public static Attribute GetAttribute(this ICustomAttributeProvider source, Type attrType)
        {
            object[] objs = source.GetCustomAttributes(attrType, true);
            if (objs.Length == 0)
                return null;

            return (Attribute)objs[0];
        }

        #endregion Methods
    }
}