using System;
using System.Runtime.CompilerServices;

namespace WebExpress.WebCore.WebLog
{
    /// <summary>
    /// Creates a frame of log entries.
    /// </summary>
    public class LogFrameSimple : IDisposable
    {
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
        protected ILog Log { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">The log entry.</param>
        /// <param name="instance">Method that wants to log.</param>
        /// <param name="line">The line number.</param>
        /// <param name="file">The source file.</param>
        public LogFrameSimple(ILog log, [CallerMemberName] string instance = null, [CallerLineNumber] int? line = null, [CallerFilePath] string file = null)
        {
            Instance = instance;
            Log = log;
            Line = line ?? 0;
            File = file;

            Log.Info("".PadRight(80, '>'), instance, line, file);
        }

        /// <summary>
        /// Release unmanaged resources that were reserved during initialization.
        /// </summary>
        /// <param name="data">The input data.</param>
        /// <returns>The output data.</returns>
        public virtual void Dispose()
        {
            Log.Info("".PadRight(80, '<'), Instance, Line, File);
        }
    }
}
