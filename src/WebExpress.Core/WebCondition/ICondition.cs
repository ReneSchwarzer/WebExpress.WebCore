using WebExpress.Core.WebMessage;

namespace WebExpress.Core.WebCondition
{
    /// <summary>
    /// Represents a condition that must be met.
    /// </summary>
    public interface ICondition
    {
        /// <summary>
        /// Verifies that the condition is met.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>True if the condition is met, false otherwise.</returns>
        bool Fulfillment(Request request);
    }
}
