namespace ConsoleConfigurator.Extensions
{
    public static class StringExtension
    {
        public static string StripPrefix(this string text, string prefix)
        {
            return text.StartsWith(prefix) ? text.Substring( prefix.Length) : text;
        }

        public static string AddQuotesIfNotInt(this string input)
        {
            int intResult;
            if (int.TryParse(input, out intResult))
                return input;
            else
                return $"'{input}'";
        }
    }
}
