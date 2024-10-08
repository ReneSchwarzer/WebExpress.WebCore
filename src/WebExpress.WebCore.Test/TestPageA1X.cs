﻿using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Title("webindex:homepage.label")]
    [Segment("pa1x", "webindex:homepage.label")]
    [ContextPath(null)]
    [Module<TestModuleA1>]
    public sealed class TestPageA1X : IPage<RenderContext>
    {
        /// <summary>
        /// Returns or sets the title of the page.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Returns or sets the page context.
        /// </summary>
        public IPageContext PageContext { get; private set; }

        /// <summary>
        /// Instillation of the page. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="pageContext">The context of the page.</param>
        public TestPageA1X(IPageContext pageContext)
        {
            PageContext = pageContext;

            // test the injection
            if (pageContext == null)
            {
                throw new ArgumentNullException(nameof(pageContext), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Redirects to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to redirect to.</param>
        public void Redirecting(string uri)
        {

        }

        /// <summary>
        /// Processing of the page.
        /// </summary>
        /// <param name="context">The context for rendering the page.</param>
        public void Process(IRenderContext context)
        {
            // test the context
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
        }
    }
}