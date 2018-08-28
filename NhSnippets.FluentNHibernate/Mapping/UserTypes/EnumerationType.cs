using System;
using System.Data;
using Headspring;
using NHibernate.Dialect;
using NHibernate.SqlTypes;
using NHibernate.Type;

namespace NhSnippets.FluentNHibernate.Mapping.UserTypes
{
    public class EnumerationType<T> : PrimitiveType where T : Enumeration<T>
    {
        public EnumerationType()
            : base(new SqlType(DbType.Int32))
        {
        }

        public override Type ReturnedClass
        {
            get { return typeof(T); }
        }

        public override string Name
        {
            get { return "enumeration"; }
        }

        public override Type PrimitiveClass
        {
            get { return typeof(int); }
        }

        public override object DefaultValue
        {
            get { return 0; }
        }

        public override object Get(IDataReader rs, int index)
        {
            object o = rs[index];
            int value = Convert.ToInt32(o);
            return Enumeration<T>.FromInt32(value);
        }

        public override object Get(IDataReader rs, string name)
        {
            int ordinal = rs.GetOrdinal(name);
            return Get(rs, ordinal);
        }

        public override object FromStringValue(string xml)
        {
            return int.Parse(xml);
        }

        public override void Set(IDbCommand cmd, object value, int index)
        {
            var parameter = (IDataParameter) cmd.Parameters[index];

            var val = (Enumeration<T>) value;

            parameter.Value = val.Value;
        }

        public override string ObjectToSQLString(object value, Dialect dialect)
        {
            return value.ToString();
        }
    }
}