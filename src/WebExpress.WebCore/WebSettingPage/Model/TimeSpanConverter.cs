using System;

namespace WebExpress.WebCore.WebSettingPage.Model
{

    /// <summary>
    /// Converts a TimeSpan object to a formatted string and vice versa.
    /// </summary>
    public class TimeSpanConverter
    {
        /// <summary>
        /// Converts a TimeSpan object to a formatted string.
        /// </summary>
        /// <param name="value">The TimeSpan object to convert.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">Optional parameter for conversion.</param>
        /// <param name="language">The language to use in the converter.</param>
        /// <returns>A formatted string representing the TimeSpan.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is null)
            {
                return null;
            }

            var ts = TimeSpan.Parse(value.ToString());
            return string.Format
                (
                    "{0}d {1:D2}h {2:D2}m {3:D2}s {4:D2}ms",
                    ts.Days,
                    ts.Hours,
                    ts.Minutes,
                    ts.Seconds,
                    ts.Milliseconds
                );
        }

        /// <summary>
        /// Converts a formatted string back to a TimeSpan object.
        /// </summary>
        /// <param name="value">The formatted string to convert.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">Optional parameter for conversion.</param>
        /// <param name="language">The language to use in the converter.</param>
        /// <returns>The TimeSpan object.</returns>
        /// <exception cref="NotImplementedException">Thrown when the method is not implemented.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
