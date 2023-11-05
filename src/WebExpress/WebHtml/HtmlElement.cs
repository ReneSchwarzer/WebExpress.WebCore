using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebExpress.WebHtml
{
    public class HtmlElement : IHtmlNode
    {
        /// <summary>
        /// Returns or sets the name. des Attributes
        /// </summary>
        protected string ElementName { get; set; }

        /// <summary>
        /// Liefert oder setzt die Attribute
        /// </summary>
        protected List<IHtmlAttribute> Attributes { get; private set; }

        /// <summary>
        /// Liefert oder setzt die Attribute
        /// </summary>
        protected List<IHtmlNode> Elements { get; private set; }

        /// <summary>
        /// Returns or sets Returns or sets the id.
        /// </summary>
        public string Id
        {
            get => GetAttribute("id");
            set => SetAttribute("id", value);
        }

        /// <summary>
        /// Liefert oder setzt die Css-Klasse
        /// </summary>
        public string Class
        {
            get => GetAttribute("class");
            set => SetAttribute("class", value);
        }

        /// <summary>
        /// Liefert oder setzt die Css-Style
        /// </summary>
        public string Style
        {
            get => GetAttribute("style");
            set => SetAttribute("style", value);
        }

        /// <summary>
        /// Liefert oder setzt die Rolle
        /// </summary>
        public string Role
        {
            get => GetAttribute("role");
            set => SetAttribute("role", value);
        }

        /// <summary>
        /// Liefert oder setzt das HTML5 Data-Attribut
        /// </summary>
        public string DataToggle
        {
            get => GetAttribute("data-toggle");
            set => SetAttribute("data-toggle", value);
        }

        /// <summary>
        /// Liefert oder setzt das HTML5 Data-Attribut
        /// </summary>
        public string DataProvide
        {
            get => GetAttribute("data-provide");
            set => SetAttribute("data-provide", value);
        }

        /// <summary>
        /// Liefert oder setzt die OnClick-Attribut
        /// </summary>
        public string OnClick
        {
            get => GetAttribute("onclick");
            set => SetAttribute("onclick", value);
        }

        /// <summary>
        /// Bestimmt, ob das Element inline ist
        /// </summary>
        public bool Inline { get; set; }

        /// <summary>
        /// Bestimmt ob das Element einen End-Tag benötigt
        /// z.B.: true = <div></div> false = <br>
        /// </summary>
        public bool CloseTag { get; protected set; }

        /// <summary>
        /// Konstruktr
        /// </summary>
        /// <param name="name">Der Name des Elements</param>
        public HtmlElement(string name, bool closeTag = true)
        {
            ElementName = name;
            Attributes = new List<IHtmlAttribute>();
            Elements = new List<IHtmlNode>();
            CloseTag = closeTag;
        }

        /// <summary>
        /// Konstruktr
        /// </summary>
        /// <param name="name">Der Name des Elements</param>
        public HtmlElement(string name, bool closeTag, params IHtml[] nodes)
            : this(name, closeTag)
        {
            foreach (var v in nodes)
            {
                if (v is HtmlAttribute)
                {
                    Attributes.Add(v as HtmlAttribute);
                }
                else if (v is HtmlElement)
                {
                    Elements.Add(v as HtmlElement);
                }
                else if (v is HtmlText)
                {
                    Elements.Add(v as HtmlText);
                }
            }
        }

        /// <summary>
        /// Liefert den Wert eines Attributs
        /// </summary>
        /// <param name="name">Der Attributname</param>
        /// <returns>Der Wert des Attributes</returns>
        protected string GetAttribute(string name)
        {
            var a = Attributes.Where(x => x.Name == name).FirstOrDefault();

            if (a != null)
            {
                return a is HtmlAttribute ? (a as HtmlAttribute).Value : string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Prüft ob ein Attribut gesetzt ist
        /// </summary>
        /// <param name="name">Der Attributname</param>
        /// <returns>true wenn Attribut vorhanden, false sonst</returns>
        protected bool HasAttribute(string name)
        {
            var a = Attributes.Where(x => x.Name == name).FirstOrDefault();

            return (a != null);
        }

        /// <summary>
        /// Setzt den Wert eines Attributs
        /// </summary>
        /// <param name="name">Der Attributname</param>
        /// <param name="value">Der Wert des Attributes</param>
        protected void SetAttribute(string name, string value)
        {
            var a = Attributes.Where(x => x.Name == name).FirstOrDefault();

            if (a != null)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Attributes.Remove(a);
                }
                else if (a is HtmlAttribute)
                {
                    (a as HtmlAttribute).Value = value;
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Attributes.Add(new HtmlAttribute(name, value));
                }
            }
        }

        /// <summary>
        /// Setzt den Wert eines Attributs
        /// </summary>
        /// <param name="name">Der Attributname</param>
        protected void SetAttribute(string name)
        {
            var a = Attributes.Where(x => x.Name == name).FirstOrDefault();

            if (a == null)
            {
                Attributes.Add(new HtmlAttributeNoneValue(name));
            }
        }

        /// <summary>
        /// Entfernt ein Attribut
        /// </summary>
        /// <param name="name">Der Attributname</param>
        protected void RemoveAttribute(string name)
        {
            var a = Attributes.Where(x => x.Name == name).FirstOrDefault();

            if (a != null)
            {
                Attributes.Remove(a);
            }
        }

        /// <summary>
        /// Liefert ein Element anhand seines Namens
        /// </summary>
        /// <param name="name">Der Elementname</param>
        /// <returns>Das Element</returns>
        protected HtmlElement GetElement(string name)
        {
            var a = Elements.Where(x => x is HtmlElement && (x as HtmlElement).ElementName == name).FirstOrDefault();

            return a as HtmlElement;
        }

        /// <summary>
        /// Setzt ein Element anhand seines Namens
        /// </summary>
        /// <param name="element">Das Element</param>
        protected void SetElement(HtmlElement element)
        {
            if (element != null)
            {
                var a = Elements.Where(x => x is HtmlElement && (x as HtmlElement).ElementName == element.ElementName);

                foreach (var v in a)
                {
                    Elements.Remove(v);
                }

                Elements.Add(element);
            }
        }

        /// <summary>
        /// Liefert den Text
        /// </summary>
        /// <returns>The text.</returns>
        protected string GetText()
        {
            var a = Elements.Where(x => x is HtmlText).Select(x => (x as HtmlText).Value);

            return string.Join(" ", a);
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public virtual void ToString(StringBuilder builder, int deep)
        {
            ToPreString(builder, deep);

            var closeTag = false;
            var nl = true;

            if (Elements.Count == 1 && Elements.First() is HtmlText)
            {
                closeTag = true;
                nl = false;

                Elements.First().ToString(builder, 0);
            }
            else if (Elements.Count > 0)
            {
                closeTag = true;
                var count = builder.Length;

                foreach (var v in Elements.Where(x => x != null))
                {
                    v.ToString(builder, deep + 1);
                }

                if (count == builder.Length)
                {
                    nl = false;
                }
            }
            else if (Elements.Count == 0)
            {
                nl = false;
            }

            if (closeTag || CloseTag)
            {
                ToPostString(builder, deep, nl);
            }
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        protected virtual void ToPreString(StringBuilder builder, int deep)
        {
            if (!Inline)
            {
                builder.AppendLine();
                builder.Append(string.Empty.PadRight(deep));
            }

            builder.Append("<");
            builder.Append(ElementName);
            foreach (var v in Attributes)
            {
                builder.Append(" ");
                v.ToString(builder, 0);
            }
            builder.Append(">");
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        /// <param name="nl">Abschlustag auf neuer Zeile beginnen</param>
        protected virtual void ToPostString(StringBuilder builder, int deep, bool nl = true)
        {
            if (!Inline && nl)
            {
                builder.AppendLine();
                builder.Append(string.Empty.PadRight(deep));
            }

            builder.Append("</");
            builder.Append(ElementName);
            builder.Append(">");
        }

        /// <summary>
        /// Setzt den Wert eines Attributs
        /// </summary>
        /// <param name="name">Der Attributname</param>
        /// <param name="value">Der Wert des Attributes</param>
        public void AddUserAttribute(string name, string value)
        {
            SetAttribute(name, value);
        }

        /// <summary>
        /// Liefert den Wert eines Attributs
        /// </summary>
        /// <param name="name">Der Attributname</param>
        /// <returns>Der Wert des Attributes</returns>
        public string GetUserAttribute(string name)
        {
            return GetAttribute(name);
        }

        /// <summary>
        /// Prüft ob ein Attribut gesetzt ist
        /// </summary>
        /// <param name="name">Der Attributname</param>
        /// <returns>true wenn Attribut vorhanden, false sonst</returns>
        public bool HasUserAttribute(string name)
        {
            return HasAttribute(name);
        }

        /// <summary>
        /// Entfernt ein Attribut
        /// </summary>
        /// <param name="name">Der Attributname</param>
        protected void RemoveUserAttribute(string name)
        {
            RemoveAttribute(name);
        }

        /// <summary>
        /// In String konvertieren
        /// </summary>
        /// <returns>Das Objekt als String</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            ToString(builder, 0);

            return builder.ToString();
        }
    }
}
