using System.Text.RegularExpressions;
using WebExpress.WebCore.WebHtml;

namespace WebExpress.WebCore.Test.Fixture
{
    /// <summary>
    /// Provides extension methods for assertions.
    /// </summary>
    public static partial class AssertExtensions
    {
        /// <summary>
        /// Gets a regular expression that matches whitespace between '>' and '<' characters.
        /// </summary>
        /// <returns>A <see cref="Regex"/> object that matches whitespace between '>' and '<' characters.</returns>
        [GeneratedRegex(@">\s+<")]
        private static partial Regex WhitespaceRegex();

        /// <summary>
        /// Asserts that the actual string is equal to the expected string, allowing for placeholders.
        /// </summary>
        /// <param name="assert">The Assert instance (not used, but required for extension method).</param>
        /// <param name="expected">The expected string with placeholders.</param>
        /// <param name="actual">The actual string to compare.</param>
        public static void EqualWithPlaceholders(string expected, string actual)
        {
            if (expected is null && actual is null)
            {
                return;
            }

            Assert.NotNull(expected);
            Assert.NotNull(actual);

            var str = RemoveLineBreaks(actual?.ToString());
            Assert.True(AreEqualWithPlaceholders(expected, str), $"Expected: {expected}{Environment.NewLine}Actual:   {str}");
        }

        /// <summary>
        /// Asserts that the actual node is equal to the expected string, allowing for placeholders.
        /// </summary>
        /// <param name="assert">The Assert instance (not used, but required for extension method).</param>
        /// <param name="expected">The expected string with placeholders.</param>
        /// <param name="actual">The actual string to compare.</param>
        public static void EqualWithPlaceholders(string expected, IHtmlNode actual)
        {
            var str = RemoveLineBreaks(actual?.ToString());
            Assert.True(AreEqualWithPlaceholders(expected, str), $"Expected: {expected}{Environment.NewLine}Actual:   {str}");
        }

        /// <summary>
        /// Compares two strings, allowing for placeholders in the expected string.
        /// </summary>
        /// <param name="expected">The expected string, which may contain '*' as a wildcard character.</param>
        /// <param name="actual">The actual string to compare against the expected string.</param>
        /// <returns>True if the actual string matches the expected string with placeholders; otherwise, false.</returns>
        private static bool AreEqualWithPlaceholders(string expected, string actual)
        {
            if (expected is null && actual is null)
            {
                return true;
            }
            else if (expected is not null && actual is null)
            {
                return false;
            }
            else if (expected is null && actual is not null)
            {
                return false;
            }

            var pattern = "^" + Regex.Escape(expected).Replace(@"\*", ".*") + "$";

            return Regex.IsMatch(actual, pattern);
        }

        /// <summary>
        /// Removes all line breaks from the input string.
        /// </summary>
        /// <param name="input">The input string from which to remove line breaks.</param>
        /// <returns>A string with all line breaks removed.</returns>
        public static string RemoveLineBreaks(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            // remove all line breaks
            string result = input.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");

            // remove whitespace of any length between '>' and '<'
            result = WhitespaceRegex().Replace(result, "><");

            return result;
        }
    }
}
