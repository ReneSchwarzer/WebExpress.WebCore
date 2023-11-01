using System.Text;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Steht für ein Feld für Benutzereingaben eines bestimmten Typs. Der Typ (Radiobutton, Ankreuzfeld, Texteingabe, etc.) wird anhand des type-Attributs angegeben.
    /// </summary>
    public class HtmlElementFieldInput : HtmlElement, IHtmlFormularItem
    {
        /// <summary>
        /// Returns or sets the name of the input field.
        /// </summary>
        public string Name
        {
            get => GetAttribute("name");
            set => SetAttribute("name", value);
        }

        /// <summary>
        /// Liefert oder setzt den Mindestwert
        /// </summary>
        public string Type
        {
            get => GetAttribute("type");
            set => SetAttribute("type", value);
        }

        /// <summary>
        /// Returns or sets the value. des Eingabefeldes
        /// </summary>
        public string Value
        {
            get => GetAttribute("value");
            set => SetAttribute("value", value?.Replace("'", "&#39;")?.Replace("\"", "&#34;"));
        }

        /// <summary>
        /// Liefert oder setzt die Zeichenlänge bei text, search, tel, url, email, oder password 
        /// Falls kein Wert angegeben wird, wird der Standardwert 20 verwendet.
        /// </summary>
        public string Size
        {
            get => GetAttribute("size");
            set => SetAttribute("size", value);
        }

        /// <summary>
        /// Liefert oder setzt ob das Felf schreibgeschützt ist
        /// </summary>
        public string Readonly
        {
            get => GetAttribute("readonly");
            set => SetAttribute("readonly", value);
        }

        /// <summary>
        /// Liefert oder setzt ob das Eingabefeld verwendet werden kann
        /// </summary>
        public bool Disabled
        {
            get => HasAttribute("disabled");
            set { if (value) { SetAttribute("disabled"); } else { RemoveAttribute("disabled"); } }
        }

        /// <summary>
        /// Liefert oder setzt den Mindestwert
        /// </summary>
        public string Min
        {
            get => GetAttribute("min");
            set => SetAttribute("min", value);
        }

        /// <summary>
        /// Liefert oder setzt den Maximalenwert
        /// </summary>
        public string Max
        {
            get => GetAttribute("max");
            set => SetAttribute("max", value);
        }

        /// <summary>
        /// Liefert oder setzt die Schrittweite bei numerische, Datums- oder Zeitangaben 
        /// </summary>
        public string Step
        {
            get => GetAttribute("step");
            set => SetAttribute("step", value);
        }

        /// <summary>
        /// Returns or sets the name. einer Liste (datalist)
        /// </summary>
        public string List
        {
            get => GetAttribute("list");
            set => SetAttribute("list", value);
        }

        /// <summary>
        /// Liefert oder setzt ob mehrfacheingaben von Datei-Uploads und Emaileingaben möglich sind
        /// </summary>
        public string Multiple
        {
            get => GetAttribute("multiple");
            set => SetAttribute("multiple", value);
        }

        /// <summary>
        /// Returns or sets the minimum length.
        /// </summary>
        public string MinLength
        {
            get => GetAttribute("minlength");
            set => SetAttribute("minlength", value);
        }

        /// <summary>
        /// Returns or sets the maximum length.
        /// </summary>
        public string MaxLength
        {
            get => GetAttribute("maxlength");
            set => SetAttribute("maxlength", value);
        }

        /// <summary>
        /// Returns or sets whether inputs are enforced.
        /// </summary>
        public bool Required
        {
            get => HasAttribute("required");
            set { if (value) { SetAttribute("required"); } else { RemoveAttribute("required"); } }
        }

        /// <summary>
        /// Liefert oder setzt ob eine Auswahl erfolt (nur bei Radio- und Check)
        /// </summary>
        public bool Checked
        {
            get => HasAttribute("checked");
            set { if (value) { SetAttribute("checked"); } else { RemoveAttribute("checked"); } }
        }

        /// <summary>
        /// Returns or sets a search pattern that checks the content.
        /// </summary>
        public string Pattern
        {
            get => GetAttribute("pattern");
            set => SetAttribute("pattern", value);
        }

        /// <summary>
        /// Returns or sets a placeholder text.
        /// </summary>
        public string Placeholder
        {
            get => GetAttribute("placeholder");
            set => SetAttribute("placeholder", value);
        }

        /// <summary>
        /// Liefert oder setzt die Eingabemethode (hilft mobilen Geräten, die richtige Tastatur-(belegung) zu wählen)
        /// </summary>
        public string Inputmode
        {
            get => GetAttribute("inputmode");
            set => SetAttribute("inputmode", value);
        }

        /// <summary>
        /// Returns or sets the identification name of the form element to which it is associated.
        /// </summary>
        public string Form
        {
            get => GetAttribute("form");
            set => SetAttribute("form", value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementFieldInput()
            : base("input")
        {
            CloseTag = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementFieldInput(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public override void ToString(StringBuilder builder, int deep)
        {
            base.ToString(builder, deep);
        }
    }
}
