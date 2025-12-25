using System.Collections.Generic;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebFragment
{
    /// <summary>
    /// Provides extension methods for checking conditions.
    /// </summary>
    public static class FragmentConditionExtentsion
    {
        /// <summary>
        /// Checks if all conditions in the collection are fulfilled for the given request.
        /// </summary>
        /// <param name="conditions">The collection of conditions to check.</param>
        /// <param name="request">The request to evaluate the conditions against.</param>
        /// <returns>True if all conditions are fulfilled; otherwise, false.</returns>
        public static bool Check(this IEnumerable<ICondition> conditions, IRequest request)
        {
            foreach (var condition in conditions)
            {
                if (!condition.Fulfillment(request))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
