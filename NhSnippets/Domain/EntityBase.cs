using System;

namespace NhSnippets.Domain
{
    public abstract class EntityBase<T> : IAuditable, IEntity where T : EntityBase<T>
    {
        private int? oldHashCode;
// ReSharper disable UnusedMember.Global Version is set by NHibernate and vired by entity mapping files ({entityname}.hbm.xml)
        public virtual byte[] Version { get; set; }
// ReSharper restore UnusedMember.Global
        public virtual DateTime? CreatedOn { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? UpdatedOn { get; set; }
        public virtual string UpdatedBy { get; set; }

        /// <summary>
        ///     Unique Id.
        ///     Can have Guid.Empty or null value if this instance is "transient" (Id not assigned yet).
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        ///     Returns true if this instance has a Guid.Empty Id
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTransient()
        {
            return Equals(Id, Guid.Empty);
        }

        /// <summary>
        ///     Returns true if both instances are:
        ///     1. Transient and are in fact the same instance (ReferenceEqual)
        ///     2. Have the same Id (one GUID equals another GUID)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var other = obj as T;
            if (other == null)
            {
                return false;
            }

            if (IsTransient() && other.IsTransient())
            {
                return ReferenceEquals(other, this);
            }
            return other.Id.Equals(Id);
        }

        /// <summary>
        ///     If oldHashCode is not null - returns the oldHashCode value (i.e. once assigned, the hash code never changes).
        ///     If oldHashCode is null - assignes the base.GetHashCode() value to oldHashCode.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable BaseObjectGetHashCodeCallInGetHashCode
        // ReSharper disable NonReadonlyFieldInGetHashCode
        public override int GetHashCode()
        {
            //don't change the hash code

            if (oldHashCode.HasValue)
            {
                return oldHashCode.Value;
            }

            //When we are transient, we use the base GetHashCode()
            //and remember it, so an instance can't change its hash code.
            if (IsTransient())
            {

                oldHashCode = base.GetHashCode();

                return oldHashCode.Value;
            }
            return Id.GetHashCode();
        }
        // ReSharper restore BaseObjectGetHashCodeCallInGetHashCode
        // ReSharper restore NonReadonlyFieldInGetHashCode

        /// <summary>
        ///     Equality operator so we can have == semantics
        /// </summary>
        public static bool operator ==(EntityBase<T> x, EntityBase<T> y)
        {
            return Equals(x, y);
        }

        /// <summary>
        ///     Inequality operator so we can have != semantics
        /// </summary>
        public static bool operator !=(EntityBase<T> x, EntityBase<T> y)
        {
            return !(x == y);
        }
    }

    public class NamedEntity<T> : EntityBase<T> where T : NamedEntity<T>
    {
        public virtual string Name { get; set; }

        public override string ToString()
        {
            return "{0}: {1}".With(GetType().Name, Name);
        }
    }
}