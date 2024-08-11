using WebExpress.WebCore.WebResource;

namespace WebExpress.WebCore.WebPage
{
    public interface IPage : IResource
    {
        /// <summary>
        /// Returns the resource context where the resource exists.
        /// </summary>
        public IResourceContext ResourceContext { get; }

        /// <summary>
        /// Returns or sets the page title.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Redirect to another page.
        /// The function throws the RedirectException.
        /// </summary>
        /// <param name="uri">The uri to redirect to.</param>
        void Redirecting(string uri);
    }
}
