using NhSnippets.JetBrains.Annotations;

namespace NhSnippets
{
    public static class Extensions
    {
        [StringFormatMethod("format")]
        public static string With(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }
}