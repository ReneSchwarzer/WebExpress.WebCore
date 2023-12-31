﻿using System.Globalization;

namespace WebExpress.WebCore.Internationalization
{
    public interface II18N
    {
        /// <summary>
        /// Returns or sets the culture.
        /// </summary>
        CultureInfo Culture { get; set; }
    }
}
