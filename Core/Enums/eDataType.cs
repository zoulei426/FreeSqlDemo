using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public enum eDataType
    {
        [DotNetType(typeof(object))]
        [DbType(System.Data.DbType.Object)]
        Object = 0,

        [DotNetType(typeof(bool))]
        [DbType(System.Data.DbType.Boolean)]
        Boolean,

        [DotNetType(typeof(float))]
        [DbType(System.Data.DbType.Single)]
        Float,

        [DotNetType(typeof(double))]
        [DbType(System.Data.DbType.Double)]
        Double,

        [DotNetType(typeof(decimal))]
        [DbType(System.Data.DbType.Decimal)]
        Decimal,

        [DotNetType(typeof(byte))]
        [DbType(System.Data.DbType.Byte)]
        Byte,

        [DotNetType(typeof(short))]
        [DbType(System.Data.DbType.Int16)]
        Int16,

        [DotNetType(typeof(int))]
        [DbType(System.Data.DbType.Int32)]
        Int32,

        [DotNetType(typeof(long))]
        [DbType(System.Data.DbType.Int64)]
        Int64,

        [DotNetType(typeof(string))]
        [DbType(System.Data.DbType.String)]
        String,

        [DotNetType(typeof(Guid))]
        [DbType(System.Data.DbType.Guid)]
        Guid,

        [DotNetType(typeof(DateTime))]
        [DbType(System.Data.DbType.DateTime)]
        DateTime,

        [DotNetType(typeof(byte[]))]
        [DbType(System.Data.DbType.Binary)]
        Binary,

        [DotNetType(typeof(byte[]))]
        [DbType(System.Data.DbType.Binary)]
        Image
    }
}