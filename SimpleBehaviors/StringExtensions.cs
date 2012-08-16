using System.Globalization;
using System.Text.RegularExpressions;

namespace SimpleBehaviors
{
    public enum StringCase
    {
        Title,
        Lower
    }

    public static class StringExtensions
    {
        public static string Wordify(this string input, StringCase casing = StringCase.Lower)
        {
            string output = input.Replace("_", " ");

            var r = new Regex("(?<=[a-z])(?<x>[A-Z])|(?<=.)(?<x>[A-Z])(?=[a-z])");
            output = r.Replace(output, " ${x}");

            switch (casing)
            {
                case StringCase.Lower:
                    output = output.ToLower();
                    break;
                case StringCase.Title: 
                    output = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(output.ToLower());
                    break;
            }

            return output;
        }
    }
}
