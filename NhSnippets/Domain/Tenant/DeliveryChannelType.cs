using Headspring;

namespace NhSnippets.Domain.Tenant
{
    public abstract class DeliveryChannelType : Enumeration<DeliveryChannelType>
    {
        public static readonly DeliveryChannelType Email = new ConcreteDeliveryChannelType(1, "Email", true);

        public static readonly DeliveryChannelType Sms = new ConcreteDeliveryChannelType(2, "SMS", true);

        public static readonly DeliveryChannelType Csr = new ConcreteDeliveryChannelType(2, "CSR", false);

        public static readonly DeliveryChannelType Totp = new ConcreteDeliveryChannelType(2, "TOTP", false);

        private DeliveryChannelType(int value, string displayName) : base(value, displayName)
        {
        }

        public bool RequiresVerification { get; protected set; }

        private class ConcreteDeliveryChannelType : DeliveryChannelType
        {
            public ConcreteDeliveryChannelType(int value, string displayName, bool requiresVerification) : base(value, displayName)
            {
                RequiresVerification = requiresVerification;
            }
        }
    }
}