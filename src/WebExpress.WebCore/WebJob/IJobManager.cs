using System.Collections.Generic;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.WebJob
{
    /// <summary>
    /// Processing of cyclic jobs.
    /// </summary>
    public interface IJobManager : IComponentManager
    {
        /// <summary>
        /// Returns all jobs contextes.
        /// </summary>
        IEnumerable<IJobContext> Jobs { get; }
    }
}
