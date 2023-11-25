using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a collection of metadata of the document. This includes links to or definitions 
    /// of scripts and style sheets.
    /// </summary>
    public class HtmlElementMetadataHead : HtmlElement, IHtmlElementMetadata
    {
        /// <summary>
        /// Returns or sets the title.
        /// </summary>
        public string Title
        {
            get => ElementTitle.Title;
            set => ElementTitle.Title = value;
        }

        /// <summary>
        /// Returns or sets the title element.
        /// </summary>
        private HtmlElementMetadataTitle ElementTitle { get; set; }

        /// <summary>
        /// Returns or sets the base.
        /// </summary>
        public string Base
        {
            get => ElementBase.Href;
            set => ElementBase.Href = value;
        }

        /// <summary>
        /// Returns or sets the element base.
        /// </summary>
        private HtmlElementMetadataBase ElementBase { get; set; }

        /// <summary>
        /// Returns or sets the favicon.
        /// </summary>
        public IEnumerable<Favicon> Favicons
        {
            get => (from x in ElementFavicons select new Favicon(x.Href, x.Type)).ToList();
            set
            {
                ElementFavicons.Clear();
                ElementFavicons.AddRange
                (
                    from x in value
                    select new HtmlElementMetadataLink()
                    {
                        Href = x.Url,
                        Rel = "icon",
                        Type = x.Mediatype != TypeFavicon.Default ? x.GetMediatyp() : ""
                    });
            }
        }

        /// <summary>
        /// Returns or sets the favicon link.
        /// </summary>
        private List<HtmlElementMetadataLink> ElementFavicons { get; set; }

        /// <summary>
        /// Returns or sets the internal stylesheet.
        /// </summary>
        public IEnumerable<string> Styles
        {
            get => (from x in ElementStyles select x.Code).ToList();
            set { ElementStyles.Clear(); ElementStyles.AddRange(from x in value select new HtmlElementMetadataStyle(x)); }
        }

        /// <summary>
        /// Returns or sets the style elements.
        /// </summary>
        private List<HtmlElementMetadataStyle> ElementStyles { get; set; }

        /// <summary>
        /// Returns or sets the scripts.
        /// </summary>
        public IEnumerable<string> Scripts
        {
            get => (from x in ElementScripts select x.Code).ToList();
            set { ElementScripts.Clear(); ElementScripts.AddRange(from x in value select new HtmlElementScriptingScript(x)); }
        }

        /// <summary>
        /// Returns or sets the script elements.
        /// </summary>
        private List<HtmlElementScriptingScript> ElementScripts { get; set; }

        /// <summary>
        /// Returns or sets the text/javascript.
        /// </summary>
        public IEnumerable<string> ScriptLinks
        {
            get => (from x in ElementScriptLinks select x.Src).ToList();
            set
            {
                ElementScriptLinks.Clear(); ElementScriptLinks.AddRange(from x in value
                                                                        select new HtmlElementScriptingScript() { Language = "javascript", Src = x, Type = "text/javascript" });
            }
        }

        /// <summary>
        /// Returns or sets the external scripts.
        /// </summary>
        private List<HtmlElementScriptingScript> ElementScriptLinks { get; set; }

        /// <summary>
        /// Returns or sets the internal stylesheet.
        /// </summary>
        public IEnumerable<string> CssLinks
        {
            get => (from x in ElementCssLinks select x.Href).ToList();
            set
            {
                ElementCssLinks.Clear(); ElementCssLinks.AddRange(from x in value
                                                                  select new HtmlElementMetadataLink() { Rel = "stylesheet", Href = x, Type = "text/css" });
            }
        }

        /// <summary>
        /// Returns or sets the css link.
        /// </summary>
        private List<HtmlElementMetadataLink> ElementCssLinks { get; set; }

        /// <summary>
        /// Returns or sets the metadata.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Meta
        {
            get => (from x in ElementMeta select new KeyValuePair<string, string>(x.Key, x.Value)).ToList();
            set
            {
                ElementMeta.Clear(); ElementMeta.AddRange(from x in value
                                                          select new HtmlElementMetadataMeta(x.Key, x.Value));
            }
        }

        /// <summary>
        /// Returns or sets the metadata elements.
        /// </summary>
        private List<HtmlElementMetadataMeta> ElementMeta { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementMetadataHead()
            : base("head")
        {
            ElementTitle = new HtmlElementMetadataTitle();
            ElementBase = new HtmlElementMetadataBase();
            ElementFavicons = new List<HtmlElementMetadataLink>();
            ElementStyles = new List<HtmlElementMetadataStyle>();
            ElementScripts = new List<HtmlElementScriptingScript>();
            ElementScriptLinks = new List<HtmlElementScriptingScript>();
            ElementCssLinks = new List<HtmlElementMetadataLink>();
            ElementMeta = new List<HtmlElementMetadataMeta>();
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public override void ToString(StringBuilder builder, int deep)
        {
            ToPreString(builder, deep);

            if (!string.IsNullOrWhiteSpace(Title))
            {
                ElementTitle.ToString(builder, deep + 1);
            }

            if (!string.IsNullOrWhiteSpace(Base))
            {
                //ElementBase.ToString(builder, deep + 1);
            }

            foreach (var v in ElementFavicons)
            {
                v.ToString(builder, deep + 1);
            }

            foreach (var v in ElementStyles)
            {
                v.ToString(builder, deep + 1);
            }

            foreach (var v in ElementScriptLinks)
            {
                v.ToString(builder, deep + 1);
            }

            foreach (var v in ElementScripts)
            {
                v.ToString(builder, deep + 1);
            }

            foreach (var v in ElementCssLinks)
            {
                v.ToString(builder, deep + 1);
            }

            foreach (var v in ElementMeta)
            {
                v.ToString(builder, deep + 1);
            }

            ToPostString(builder, deep, true);
        }
    }
}
