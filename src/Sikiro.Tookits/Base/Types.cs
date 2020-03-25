using System;
using System.Collections;
using System.Collections.Generic;

namespace Sikiro.Tookits.Base
{
    public static class Types
    {
        /// <summary>
        /// Object 类型
        /// </summary>
        public static readonly Type Object = typeof(object);

        /// <summary>
        /// Type 类型
        /// </summary>
        public static readonly Type Type = typeof(Type);

        /// <summary>
        /// Type 类型
        /// </summary>
        public static readonly Type IList = typeof(IList);

        /// <summary>
        /// Type 类型
        /// </summary>
        public static readonly Type Long = typeof(long);

        /// <summary>
        /// Type 类型
        /// </summary>
        public static readonly Type Float = typeof(float);

        /// <summary>
        /// Stirng 类型
        /// </summary>
        public static readonly Type String = typeof(string);

        /// <summary>
        /// Char 类型
        /// </summary>
        public static readonly Type Char = typeof(char);

        /// <summary>
        /// Boolean 类型
        /// </summary>
        public static readonly Type Boolean = typeof(bool);

        /// <summary>
        /// Byte 类型
        /// </summary>
        public static readonly Type Byte = typeof(byte);


        /// <summary>
        /// Byte 数组类型
        /// </summary>
        public static readonly Type ByteArray = typeof(byte[]);

        /// <summary>
        /// SByte 类型
        /// </summary>
        public static readonly Type SByte = typeof(sbyte);

        /// <summary>
        /// UInt16 类型
        /// </summary>
        public static readonly Type UInt16 = typeof(ushort);

        /// <summary>
        /// Int32 类型
        /// </summary>
        public static readonly Type Int = typeof(int);

        /// <summary>
        /// UInt32 类型
        /// </summary>
        public static readonly Type UInt32 = typeof(uint);

        /// <summary>
        /// UInt64 类型
        /// </summary>
        public static readonly Type UInt64 = typeof(ulong);

        /// <summary>
        /// Double 类型
        /// </summary>
        public static readonly Type Double = typeof(double);

        /// <summary>
        /// Single 类型
        /// </summary>
        public static readonly Type Single = typeof(float);

        /// <summary>
        /// Decimal 类型
        /// </summary>
        public static readonly Type Decimal = typeof(decimal);

        /// <summary>
        /// Guid 类型
        /// </summary>
        public static readonly Type Guid = typeof(Guid);

        /// <summary>
        /// DateTime 类型
        /// </summary>
        public static readonly Type DateTime = typeof(DateTime);

        /// <summary>
        /// TimeSpan 类型
        /// </summary>
        public static readonly Type TimeSpan = typeof(TimeSpan);

        /// <summary>
        /// Nullable 类型
        /// </summary>
        public static readonly Type Nullable = typeof(Nullable<>);

        /// <summary>
        /// ValueType 类型
        /// </summary>
        public static readonly Type ValueType = typeof(ValueType);

        /// <summary>
        /// void 类型
        /// </summary>
        public static readonly Type Void = typeof(void);

        /// <summary>
        /// DbNull 类型
        /// </summary>
        public static readonly Type DbNull = typeof(DBNull);

        /// <summary>
        /// Delegate 类型
        /// </summary>
        public static readonly Type Delegate = typeof(Delegate);

        /// <summary>
        /// ByteEnumerable 类型
        /// </summary>
        public static readonly Type ByteEnumerable = typeof(IEnumerable<Byte>);

        /// <summary>
        /// IEnumerable 类型
        /// </summary>
        public static readonly Type IEnumerableOfT = typeof(IEnumerable<>);

        /// <summary>
        /// IEnumerable 类型
        /// </summary>
        public static readonly Type IEnumerable = typeof(IEnumerable);

        /// <summary>
        /// IListSource 类型
        /// </summary>
        public static readonly Type IListSource = typeof(System.ComponentModel.IListSource);

        /// <summary>
        /// IDictionary 类型
        /// </summary>
        public static readonly Type IDictionary = typeof(IDictionary);

        /// <summary>
        /// IDictionary 类型
        /// </summary>
        public static readonly Type IDictionaryOfT = typeof(IDictionary<,>);

        /// <summary>
        /// Dictionary 类型
        /// </summary>
        public static readonly Type DictionaryOfT = typeof(Dictionary<,>);

        /// <summary>
        /// IDataReader 类型
        /// </summary>
        public static readonly Type IDataReader = typeof(System.Data.IDataReader);

        /// <summary>
        /// DataTable 类型
        /// </summary>
        public static readonly Type DataTable = typeof(System.Data.DataTable);

        /// <summary>
        /// DataRow 类型
        /// </summary>
        public static readonly Type DataRow = typeof(System.Data.DataRow);

        /// <summary>
        /// IDictionary 类型
        /// </summary>
        public static readonly Type IDictionaryOfStringAndObject = typeof(IDictionary<string, object>);

        /// <summary>
        /// IDictionary 类型
        /// </summary>
        public static readonly Type IDictionaryOfStringAndString = typeof(IDictionary<string, string>);
    }
}
