using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebHtml;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebFragment.Model
{
    /// <summary>
    /// Fragments are components that can be integrated into pages to dynamically expand functionalities.
    /// </summary>
    internal class FragmentItem : IDisposable
    {
        private IFragment _instance;
        private readonly IComponentHub _componentHub;
        private readonly IHttpServerContext _httpServerContext;

        /// <summary>
        /// Returns the context of the associated plugin.
        /// </summary>
        public IPluginContext PluginContext { get; set; }

        /// <summary>
        /// Returns the application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; set; }

        /// <summary>
        /// Returns the fragment context.
        /// </summary>
        public IFragmentContext FragmentContext { get; set; }

        /// <summary>
        /// The type of fragment.
        /// </summary>
        public Type FragmentClass { get; set; }

        /// <summary>
        /// Returns the section.
        /// </summary>
        public Type Section { get; set; }

        /// <summary>
        /// Returns the scope.
        /// </summary>
        public Type Scope { get; set; }

        /// <summary>
        /// Returns the conditions that must be met for the component to be active.
        /// </summary>
        public ICollection<ICondition> Conditions { get; set; }

        /// <summary>
        /// The order of the fragment.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Determines whether the component is created once and reused on each execution.
        /// </summary>
        public bool Cache { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="fragmentManager">The fragment manager responsible for managing web fragments.</param>
        /// <param name="httpServerContext">The context of the HTTP server.</param>
        public FragmentItem(IComponentHub componentHub, IHttpServerContext httpServerContext)
        {
            _componentHub = componentHub;
            _httpServerContext = httpServerContext;
        }
        /// <summary>
        /// Processes the fragments for a given section within the specified render context.
        /// </summary>
        /// <param name="renderContext">The context in which rendering occurs.</param>
        /// <returns>An HTML node representing the rendered fragments. Can be null if no nodes are present.</returns>
        public IHtmlNode Render(IRenderContext renderContext)
        {
            var instance = _instance;
            instance ??= ComponentActivator.CreateInstance<IFragment, IFragmentContext>(FragmentClass, FragmentContext, _httpServerContext, _componentHub, FragmentContext);

            if (Cache)
            {
                _instance = instance;
            }

            if (CheckControl(renderContext))
            {
                return instance.Render(renderContext);
            }

            return null;
        }

        /// <summary>
        /// Checks the component to see if they are displayed or disabled.
        /// </summary>
        /// <param name="renderContext">The context in which checking occurs.</param>
        /// <returns>True if the fragment is active, false otherwise.</returns>
        private bool CheckControl(IRenderContext renderContext)
        {
            return FragmentContext.Conditions.Count == 0 || FragmentContext.Conditions.All(x => x.Fulfillment(renderContext?.Request));
        }

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Convert the resource element to a string.
        /// </summary>
        /// <returns>The resource element in its string representation.</returns>
        public override string ToString()
        {
            return $"Fragment: '{FragmentContext.FragmentId}'";
        }
    }
}
