using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using Xunit.Abstractions;

namespace NhSnippets.NhXml.Tests
{
    public class TestOutputAppender : AppenderSkeleton
    {
        private readonly ITestOutputHelper xunitTestOutputHelper;

        public TestOutputAppender(ITestOutputHelper xunitTestOutputHelper)
        {
            this.xunitTestOutputHelper = xunitTestOutputHelper;
            Name = "TestOutputAppender";
            Layout = new PatternLayout("%date [%thread] %-5level %logger - %message");
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            xunitTestOutputHelper.WriteLine(RenderLoggingEvent(loggingEvent));
        }
    }
}