using System.Collections.Generic;
using log4net.Config;
using NhSnippets.Domain.Tenant;
using NhSnippets.NhXml.Mapping.UserTypes;
using Xunit;
using Xunit.Abstractions;

namespace NhSnippets.NhXml.Tests
{
    public class Tests : DatabaseFixture
    {
        public Tests(ITestOutputHelper output): base(typeof(ValueListUserType).Assembly)
        {
            BasicConfigurator.Configure(new TestOutputAppender(output));
        }

        [Fact]
        public void CanSave()
        {
            var t = new Tenant
            {
                Name = "Test",
                Code = "TEST",
                SupportedLocales = new List<string> {"en-CA", "en-US"}
            };

            t.DeliveryChannels.Add(DeliveryChannelType.Email);
            t.DeliveryChannels.Add(DeliveryChannelType.Csr);

            t.VerificationModes.Add(VerificationModes.Automated);
            t.VerificationModes.Add(VerificationModes.Manual);

            Session.Save(t);
            Session.Flush();

            var fromDb = Session.Load<Tenant>(t.Id);

            Assert.NotNull(fromDb);
            Assert.Equal(t.Name, fromDb.Name);
            Assert.Equal(t.Code, fromDb.Code);
            Assert.Equal(t.SupportedLocales, fromDb.SupportedLocales);
            Assert.Equal(t.DeliveryChannels, fromDb.DeliveryChannels);
            Assert.Equal(t.VerificationModes, fromDb.VerificationModes);
        }
    }
}
