using System.Collections.Generic;

namespace WebExpress.Internationalization
{
    /// <summary>
    /// key = language (ISO 639-1 two-letter)
    /// value = { key = , value = }
    /// </summary>
    internal class InternationalizationDictionary : Dictionary<string, InternationalizationItem>
    {
    }
}
