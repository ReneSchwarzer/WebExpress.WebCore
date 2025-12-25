using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// Represents a test condition that never becomes true.
    /// </summary>
    public class TestConditionAlwaysFalse : ICondition
    {
        /// <summary>
        /// Check whether the condition is fulfilled.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>True if the condition is fulfilled, false otherwise.</returns>
        public bool Fulfillment(IRequest request)
        {
            return false;
        }
    }
}
