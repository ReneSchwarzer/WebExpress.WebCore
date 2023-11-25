using System;
using WebExpress.WebCore.WebApplication;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Application assignment attribute of the application ID.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ApplicationAttribute : Attribute, IModuleAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="applicationId">A specific ApplicationId, regular expression, or * for any application.</param>
        public ApplicationAttribute(string applicationId)
        {

        }
    }

    /// <summary>
    /// An application expression attribute, which is determined by the type.
    /// </summary>
    /// <typeparamref name="T">The type of the application.</typeparamref/>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ApplicationAttribute<T> : Attribute, IModuleAttribute where T : class, IApplication
    {

    }
}
