using System;
using WebExpress.WebCore.WebApplication;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// An application expression attribute, which is determined by the type.
    /// </summary>
    /// <typeparamref name="TApplication">The type of the application.</typeparamref>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ApplicationAttribute<TApplication> : Attribute, IPluginAttribute where TApplication : class, IApplication
    {

    }
}
