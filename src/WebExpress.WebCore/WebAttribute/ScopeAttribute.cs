﻿using System;
using WebExpress.WebCore.WebScope;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// The range in which the component is valid.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ScopeAttribute<T> : Attribute, IResourceAttribute where T : class, IScope
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScopeAttribute()
        {

        }
    }
}
