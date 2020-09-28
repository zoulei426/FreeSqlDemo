using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class DataColumnAttribute : CDAttribute
    {
        #region Properties

        public string ColumnName { get; set; }
        public eDataType ColumnType { get; set; }

        public int Size { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }

        public bool PrimaryKey { get; set; }
        public bool Nullable { get; set; }
        public bool Auto { get; set; }
        public bool Enabled { get; set; }

        public string AliasName
        {
            get { return _AliasName.IsNullOrBlank() ? ColumnName : _AliasName; }
            set { _AliasName = value; }
        }

        public string MemberName
        {
            get { return _MemberName.IsNullOrBlank() ? ColumnName : _MemberName; }
            set { _MemberName = value; }
        }

        #endregion Properties

        #region Fields

        private string _AliasName;
        private string _MemberName;

        #endregion Fields

        #region Ctor

        public DataColumnAttribute()
            : this(string.Empty, eDataType.Object, -1, 0, 0, false, true, false, true)
        {
        }

        public DataColumnAttribute(string name)
            : this(name, eDataType.Object, -1, 0, 0, false, true, false, true)
        {
        }

        public DataColumnAttribute(string name, eDataType type, int size, int precision, int scale, bool primaryKey, bool nullable, bool auto, bool enable)
        {
            ColumnName = name;
            ColumnType = type;
            Size = size;
            PrimaryKey = primaryKey;
            Precision = precision;
            Scale = scale;
            Nullable = nullable;
            Auto = auto;
            Enabled = enable;
        }

        #endregion Ctor

        #region Methods

        public override string ToString()
        {
            return string.Format("{0}, {1}", ColumnName, ColumnType);
        }

        #endregion Methods
    }
}