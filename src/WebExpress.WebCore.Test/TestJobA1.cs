using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebJob;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy job for testing purposes.
    /// </summary>
    [Application<TestApplicationA>()]
    [Name("module.namea1")]
    [Description("module.descriptiona1")]
    [Icon("/assets/img/Logo.png")]
    [ContextPath("/mca")]
    [AssetPath("/maa")]
    [DataPath("/mda")]
    public sealed class TestJobA1 : IJob
    {
        /// <summary>
        /// Initialization of the job.
        /// </summary>
        /// <param name="jobContext">The module context, for testing the injection.</param>
        private TestJobA1(IJobContext jobContext)
        {
            // test the injection
            if (jobContext == null)
            {
                throw new ArgumentNullException(nameof(jobContext), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Called when the jobs starts working. The call is concurrent.
        /// </summary>
        public void Process()
        {
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
