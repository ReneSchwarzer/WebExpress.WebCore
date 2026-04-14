using System;

namespace WebExpress.WebCore
{
    /// <summary>
    /// Represents a single data point for server statistics.
    /// </summary>
    public class HttpServerStatisticItem
    {
        /// <summary>
        /// Gets or sets the timestamp (grouped by minute).
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the total number of requests.
        /// </summary>
        public int Requests { get; set; }

        /// <summary>
        /// Gets or sets the number of erroneous requests (status code >= 400).
        /// </summary>
        public int Errors { get; set; }

        /// <summary>
        /// Gets or sets the minimum response time in milliseconds.
        /// </summary>
        public long MinDuration { get; set; }

        /// <summary>
        /// Gets or sets the maximum response time in milliseconds.
        /// </summary>
        public long MaxDuration { get; set; }

        /// <summary>
        /// Gets or sets the total duration of all requests in this interval in milliseconds.
        /// </summary>
        public long TotalDuration { get; set; }

        /// <summary>
        /// Gets the average response time in milliseconds.
        /// </summary>
        public double AverageDuration => Requests > 0 ? (double)TotalDuration / Requests : 0;

        /// <summary>
        /// Gets or sets the average CPU usage in percentage (0-100) during this interval.
        /// </summary>
        public double CpuUsage { get; set; }

        /// <summary>
        /// Gets or sets the average memory usage in MB during this interval.
        /// </summary>
        public double MemoryUsage { get; set; }
    }
}
