﻿using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebResource
{
    public interface IResource : II18N
    {
        /// <summary>
        /// Returns the module context where the resource exists.
        /// </summary>
        IResourceContext ResourceContext { get; }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="resourceContext">The context of the resource.</param>
        void Initialization(IResourceContext resourceContext);

        /// <summary>
        /// Preprocessing of the resource.
        /// </summary>
        /// <param name="request">The request.</param>
        void PreProcess(Request request);

        /// <summary>
        /// Processing of the resource.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        Response Process(Request request);

        /// <summary>
        /// Post-processing of the resource.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <returns>The response.</returns>
        Response PostProcess(Request request, Response response);
    }
}
