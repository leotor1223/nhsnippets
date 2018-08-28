using System.Collections.Generic;

namespace NhSnippets.Domain.Tenant
{
     public class Tenant : NamedEntity<Tenant>, IAggregateRoot
    {
        public Tenant()
        {
            Init();
        }

        public virtual string Code { get; set; }
        public virtual IList<string> SupportedLocales { get; set; }
        public virtual ICollection<DeliveryChannelType> DeliveryChannels { get; set; }
        public virtual ICollection<VerificationModes> VerificationModes { get; set; }

        private void Init()
        {
            SupportedLocales = new List<string>();
            DeliveryChannels = new HashSet<DeliveryChannelType>();
            VerificationModes = new HashSet<VerificationModes>();
        }  
    }
}