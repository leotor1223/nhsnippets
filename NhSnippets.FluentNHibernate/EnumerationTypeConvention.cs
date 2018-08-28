using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using Headspring;
using NhSnippets.FluentNHibernate.Mapping.UserTypes;

namespace NhSnippets.FluentNHibernate
{
    public class EnumerationTypeConvention : IPropertyConvention, IPropertyConventionAcceptance
    {
        private static readonly Type openType = typeof(EnumerationType<>);

        public void Apply(IPropertyInstance instance)
        {
            Type closedType = openType.MakeGenericType(instance.Property.PropertyType);

            instance.CustomType(closedType);
        }

        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => IsEnumerationType(x.Property.PropertyType));
        }

        private bool IsEnumerationType(Type type)
        {
            return GetTypeHierarchy(type)
                .Where(t => t.IsGenericType)
                .Select(t => t.GetGenericTypeDefinition())
                .Any(t => t == typeof(Enumeration<>));
        }

        private IEnumerable<Type> GetTypeHierarchy(Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}