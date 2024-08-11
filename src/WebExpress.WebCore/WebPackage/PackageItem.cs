using System.Collections.Generic;

namespace WebExpress.WebCore.WebPackage
{
    public class PackageItem
    {
        /// <summary>
        /// Returns or sets the package file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Returns or sets Returns or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Returns or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Returns or sets the titles.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Returns or sets the authors.
        /// </summary>
        public string Authors { get; set; }

        /// <summary>
        /// Returns or sets the license.
        /// </summary>
        public string License { get; set; }

        /// <summary>
        /// Returns or sets the package icon.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Returns or sets the readme file of the package (md format).
        /// </summary>
        public string Readme { get; set; }

        /// <summary>
        /// Returns or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Returns or sets the tags.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Returns or sets the plugin sources.
        /// </summary>
        public IEnumerable<string> PluginSources { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        internal PackageItem()
        {
        }
    }
}
