using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Core;

namespace Windows.Control
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PropertyDescriptorCreatorHandlerAttribute : Attribute
    {
        #region Properties

        public Type PropertyType { get; set; }
        internal MethodInfo Method { get; set; }

        #endregion Properties

        #region Ctor

        public PropertyDescriptorCreatorHandlerAttribute(Type propertyType)
        {
            PropertyType = propertyType;
        }

        #endregion Ctor

        #region Methods

        #region Methods - Helper

        public static Dictionary<Type, PropertyDescriptorCreatorHandlerAttribute> Create(Type type)
        {
            var dic = new Dictionary<Type, PropertyDescriptorCreatorHandlerAttribute>();

            var methods = type.GetMethods(BindingFlags.Static |
                                          BindingFlags.Public |
                                          BindingFlags.NonPublic);

            foreach (var m in methods)
            {
                var attrs = m.GetAttributes<PropertyDescriptorCreatorHandlerAttribute>();

                foreach (var attr in attrs)
                {
                    if (attr.PropertyType == null)
                        continue;

                    attr.Method = m;
                    dic[attr.PropertyType] = attr;
                }
            }

            return dic;
        }

        #endregion Methods - Helper

        #endregion Methods
    }
}