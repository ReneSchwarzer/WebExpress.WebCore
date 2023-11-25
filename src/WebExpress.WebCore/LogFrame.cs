using System;
using System.Runtime.CompilerServices;

namespace WebExpress.WebCore
{
    /// <summary>
    /// Creates a frame of log entries.
    /// </summary>
    public class LogFrame : IDisposable
    {
        /// <summary>
        /// The status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Method that wants to log.
        /// </summary>
        protected string Instance { get; set; }

        /// <summary>
        /// The line number.
        /// </summary>
        protected int Line { get; set; }

        /// <summary>
        /// The source file.
        /// </summary>
        protected string File { get; set; }

        /// <summary>
        /// The log entry.
        /// </summary>
        protected Log Log { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">The log entry.</param>
        /// <param name="name">The name.</param>
        /// <param name="additionalHeading">An additional heading or zero.</param>
        /// <param name="instance">Method that wants to log.</param>
        /// <param name="line">The line number.</param>
        /// <param name="file">The source file.</param>
        public LogFrame(Log log, string name, string additionalHeading = null, [CallerMemberName] string instance = null, [CallerLineNumber] int? line = null, [CallerFilePath] string file = null)
        {
            Instance = instance;
            Status = string.Format("{0} abgeschlossen. ", name);

            Log = log;
            Log.Seperator();
            Log.Info(string.Format("Beginne mit {0}", name) + (!string.IsNullOrWhiteSpace(additionalHeading) ? " " + additionalHeading : ""), instance, line, file);
            Log.Info("".PadRight(80, '-'), instance, line, file);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="additionalHeading">The additional heading or zero.</param>
        /// <param name="instance">Method that wants to log.</param>
        /// <param name="line">The line number.</param>
        /// <param name="file">The source file.</param>
        public LogFrame(string name, string additionalHeading = null, [CallerMemberName] string instance = null, [CallerLineNumber] int? line = null, [CallerFilePath] string file = null)
            : this(Log.Current, name, additionalHeading, instance, line, file)
        {
        }

        /// <summary>
        /// Release unmanaged resources that were reserved during initialization.
        /// </summary>
        /// <param name="data">The input data.</param>
        /// <returns>The output data.</returns>
        public virtual void Dispose()
        {
            Log.Info("".PadRight(80, '='), Instance, Line, File);
            Log.Info(Status, Instance, Line, File);
        }
    }
}
