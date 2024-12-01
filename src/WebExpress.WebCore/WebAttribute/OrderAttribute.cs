namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Attribute used to identify a class as a plugin component.
    /// </summary>
    public class OrderAttribute : System.Attribute, IFragmentAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="order">The order within the section.</param>
        public OrderAttribute(int order)
        {
        }

    }
}
