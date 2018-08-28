using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace NhSnippets.FluentNHibernate.Mapping.UserTypes
{
    internal class ValueListUserType : IUserType
    {
        private const char Separator = ',';

        public new bool Equals(object x, object y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }
            
            var xl = (IList<string>) x;
            var yl = (IList<string>) y;
            if (xl.Count != yl.Count)
            {
                return false;
            }
            
            return !xl.Except(yl).Any();
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            object obj = NHibernateUtil.String.NullSafeGet(rs, names[0]);
            if (obj == null)
            {
                return new List<string>();
            }

            return ((string) obj).Split(Separator).ToList();
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            object paramVal = DBNull.Value;

            var list = value as IList<string>;
            if (list != null && list.Count > 0)
            {
                paramVal = string.Join(Separator.ToString(CultureInfo.InvariantCulture), list);
            }
            var parameter = (IDataParameter)cmd.Parameters[index];
            parameter.Value = paramVal;
        }

        public object DeepCopy(object value)
        {
            return value == null ? null : ((IList<string>)value).ToList();
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public Type ReturnedType
        {
            get { return typeof (List<string>); }
        }

        public SqlType[] SqlTypes
        {
            get { return new[] { NHibernateUtil.String.SqlType }; }
        }

        public bool IsMutable
        {
            get { return true; }
        }
    }
}