﻿using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Indicates that a page or component can be reused
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CacheAttribute : System.Attribute, IResourceAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CacheAttribute()
        {

        }
    }
}
