using System.Linq;
using System.Text.Json;
using WebExpress.WebMessage;

namespace WebExpress.WebResource
{
    public abstract class ResourceRest : Resource
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResourceRest()
        {

        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Initialization(IResourceContext context)
        {
            base.Initialization(context);
        }

        /// <summary>
        /// Processing of the resource that was called via the get request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>An enumeration of which json serializer can be serialized.</returns>
        public virtual object GetData(Request request)
        {
            return null;
        }

        /// <summary>
        /// Processing of the resource that was called via the delete request.
        /// </summary>
        /// <param name="id">The id to delete.</param>
        /// <param name="request">The request.</param>
        /// <returns>The result of the deletion.</returns>
        public virtual bool DeleteData(string id, Request request)
        {
            return false;
        }

        /// <summary>
        /// Processing of the resource.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        public override Response Process(Request request)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var response = new ResponseOK();
            var content = string.Empty;

            switch (request.Method)
            {
                case RequestMethod.GET:
                    {
                        content = JsonSerializer.Serialize(GetData(request), options);

                        break;
                    }
                case RequestMethod.DELETE:
                    {
                        content = JsonSerializer.Serialize(DeleteData(request.Uri.PathSegments.Last()?.ToString(), request), options);

                        break;
                    }
            };

            response.Header.ContentLength = content.Length;
            response.Content = content;

            return response;
        }
    }
}
