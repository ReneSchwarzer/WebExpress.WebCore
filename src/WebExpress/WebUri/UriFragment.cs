namespace WebExpress.WebUri
{
    /// <summary>
    /// Uri which consists only of the fragment (e.g. #).
    /// </summary>
    public class UriFragment : UriResource
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UriFragment()
        {
        }

        /// <summary>
        /// Converts the uri to a string.
        /// </summary>
        /// <returns>The string representation of the uri.</returns>
        public override string ToString()
        {
            return "#" + Fragment;
        }


    }
}