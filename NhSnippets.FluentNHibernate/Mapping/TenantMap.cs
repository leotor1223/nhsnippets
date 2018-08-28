using FluentNHibernate.Mapping;
using NhSnippets.Domain.Tenant;
using NhSnippets.FluentNHibernate.Mapping.UserTypes;

namespace NhSnippets.FluentNHibernate.Mapping
{
    public class TenantMap : ClassMap<Tenant>
    {
        public TenantMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Code);
            Map(x => x.SupportedLocales).CustomType<ValueListUserType>();
            
            HasMany(x => x.DeliveryChannels).AsSet()
                .Table("TenantTwoFactorDeliveryChannel")
                .KeyColumn("TenantID")
                .Element("TwoFactorDeliveryChannelID", e => e.Type<EnumerationType<DeliveryChannelType>>());

            HasMany(x => x.VerificationModes).AsSet()
                .Table("TenantVerificationMode")
                .KeyColumn("TenantID")
                .Element("VerificationModeID");
        }
    }
}