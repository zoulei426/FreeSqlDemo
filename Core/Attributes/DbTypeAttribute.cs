using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Core
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class DbTypeAttribute : Attribute
    {
        #region Properties

        public DbType Type { get; set; }

        #endregion Properties

        #region Fields

        private static Dictionary<eDataType, DbType> dicDataTypeToType = new Dictionary<eDataType, DbType>();
        private static Dictionary<DbType, eDataType> dicTypeToDataType = new Dictionary<DbType, eDataType>();

        #endregion Fields

        #region Ctor

        static DbTypeAttribute()
        {
            Install();
        }

        public DbTypeAttribute(DbType type)
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

                var attr = fi.GetAttribute<DbTypeAttribute>();
                if (attr == null)
                    attr = new DbTypeAttribute(DbType.Object);

                dicDataTypeToType[(eDataType)val] = attr.Type;
                if (!dicTypeToDataType.ContainsKey(attr.Type))
                    dicTypeToDataType[attr.Type] = (eDataType)val;

                return true;
            });
        }

        public static DbType GetType(eDataType type)
        {
            return dicDataTypeToType[type];
        }

        public static eDataType GetType(DbType type)
        {
            return dicTypeToDataType[type];
        }

        #endregion Methods
    }
}