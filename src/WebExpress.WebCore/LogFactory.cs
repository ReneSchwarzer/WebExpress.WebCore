﻿using Microsoft.Extensions.Logging;

namespace WebExpress.WebCore
{
    public class LogFactory : ILoggerFactory, ILoggerProvider
    {
        /// <summary>
        /// Adds an ILoggerProvider to the logging system.
        /// </summary>
        /// <param name="provider">The ILoggerProvider.</param>
        public void AddProvider(ILoggerProvider provider)
        {

        }

        /// <summary>
        /// Creates a new ILogger instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages generated by logging.</param>
        /// <returns>A new ILogger instance.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            // use an existing logging instance
            return Log.Current;
        }

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or 
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }
    }
}
