using System;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebResource;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Activation of options (e.g. 'webexpress.webapp.settinglog' or 'webexpress.webapp.*').
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class OptionAttribute : Attribute, IApplicationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="option">The option to activate.</param>
        public OptionAttribute(string option)
        {

        }
    }

    /// <summary>
    /// Activation of options (e.g. 'webexpress.webapp.settinglog' or 'webexpress.webapp.*').
    /// </summary>
    /// <typeparamref name="M">The module or resource class.</typeparamref/>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class WebExOptionAttribute<M> : Attribute, IApplicationAttribute where M : class, IModule
    {

    }

    /// <summary>
    /// Activation of options (e.g. 'webexpress.webapp.settinglog' or 'webexpress.webapp.*').
    /// <typeparamref name="M">The module or resource class.</typeparamref/>
    /// <typeparamref name="R">The resource or resource class.</typeparamref>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class WebExOptionAttribute<M, R> : Attribute, IApplicationAttribute where M : class, IModule where R : class, IResource
    {
    }
}
