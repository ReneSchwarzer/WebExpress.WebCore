using WebExpress.WebMessage;

namespace WebExpress.WebCondition
{
    /// <summary>
    /// Stellt eine Bedingiung dar, die erfüllt sein muss
    /// </summary>
    public interface ICondition
    {
        /// <summary>
        /// Die Bedingung
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>true wenn die Bedingung erfüllt ist, false sonst</returns>
        bool Fulfillment(Request request);
    }
}
