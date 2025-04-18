namespace WebExpress.WebCore.WebLog
{
    /// <summary>
    /// Enumerations of the log mode.
    /// </summary>
    public enum LogMode
    {
        /// <summary>
        /// Logging is turned off.
        /// </summary>
        Off,

        /// <summary>
        /// Logs are appended to any existing logs.
        /// </summary>
        Append,

        /// <summary>
        /// Any existing logs are overridden.
        /// </summary>
        Override
    }
}
