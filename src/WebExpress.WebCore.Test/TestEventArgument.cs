using WebExpress.WebCore.WebEvent;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// Represents a test event argument.
    /// </summary>
    public class TestEventArgument : IEventArgument
    {
        /// <summary>
        /// Returns or sets a boolean value for testing purposes.
        /// </summary>
        public bool TestProperty { get; set; }
    }
}