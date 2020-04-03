namespace ConsoleConfigurator.Extensions
{
    public static class StringExtension
    {
        public static string StripPrefix(this string text, string prefix)
        {
            return text.StartsWith(prefix) ? text.Substring( prefix.Length) : text;
        }
    }
}
