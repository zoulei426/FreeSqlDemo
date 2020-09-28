using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Core
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PropertyChangedHandlerAttribute : Attribute
    {
        #region Properties

        public string PropertyName { get; set; }
        public MethodInfo Method { get; set; }

        #endregion Properties

        #region Ctor

        public PropertyChangedHandlerAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        #endregion Ctor

        #region Methods

        #region Methods - Helper

        public static Dictionary<string, PropertyChangedHandlerAttribute> Create(Type type)
        {
            var dic = new Dictionary<string, PropertyChangedHandlerAttribute>();

            var methods = type.GetMethods(BindingFlags.Instance |
                                          BindingFlags.Public |
                                          BindingFlags.NonPublic);

            foreach (var m in methods)
            {
                var attrs = m.GetAttributes<PropertyChangedHandlerAttribute>();

                foreach (var attr in attrs)
                {
                    if (attr.PropertyName.IsNullOrBlank())
                        continue;

                    attr.Method = m;
                    dic[attr.PropertyName] = attr;
                }
            }

            return dic;
        }

        #endregion Methods - Helper

        #endregion Methods
    }
}