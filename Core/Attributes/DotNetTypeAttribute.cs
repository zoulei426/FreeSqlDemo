using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class DotNetTypeAttribute : Attribute
    {
        #region Properties

        public Type Type { get; set; }

        #endregion Properties

        #region Fields

        private static Dictionary<eDataType, Type> dicDataTypeToType = new Dictionary<eDataType, Type>();
        private static Dictionary<Type, eDataType> dicTypeToDataType = new Dictionary<Type, eDataType>();

        #endregion Fields

        #region Ctor

        static DotNetTypeAttribute()
        {
            Install();
        }

        public DotNetTypeAttribute(Type type)
        {
            Type = type;
        }

        #endregion Ctor

        #region Methods

        private static void Install()
        {
            eDataType.Object.TraversalFieldsInfo((fi, val) =>
            {
                if (!fi.FieldType.IsEnum)
                    return true;

                var attr = fi.GetAttribute<DotNetTypeAttribute>();
                if (attr == null)
                    attr = new DotNetTypeAttribute(typeof(object));

                dicDataTypeToType[(eDataType)val] = attr.Type;
                if (!dicTypeToDataType.ContainsKey(attr.Type))
                    dicTypeToDataType[attr.Type] = (eDataType)val;

                return true;
            });
        }

        public static Type GetType(eDataType type)
        {
            return dicDataTypeToType[type];
        }

        public static eDataType GetType(Type type)
        {
            while (type != null)
            {
                if (type.IsEnum)
                    return eDataType.Int32;

                if (type.IsNullableGeneric())
                    type = type.GetGenericArguments()[0];

                if (type.IsEnum)
                    return eDataType.Int32;

                if (dicTypeToDataType.ContainsKey(type))
                    return dicTypeToDataType[type];

                type = type.BaseType;
            }

            throw new ArgumentNullException("type");
        }

        #endregion Methods
    }
}