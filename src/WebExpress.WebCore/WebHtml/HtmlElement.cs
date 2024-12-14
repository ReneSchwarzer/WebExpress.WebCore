using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// The basis of all html elements (see RfC 1866).
    /// </summary>
    public class HtmlElement : IHtmlNode
    {
        private readonly List<IHtmlNode> _elements = [];
        private readonly List<IHtmlAttribute> _attributes = [];

        /// <summary>
        /// Returns or sets the name. des Attributes
        /// </summary>
        protected string ElementName { get; set; }

        /// <summary>
        /// Returns or sets the attributes.
        /// </summary>
        protected IEnumerable<IHtmlAttribute> Attributes => _attributes;

        /// <summary>
        /// Returns the elements.
        /// </summary>
        protected IEnumerable<IHtmlNode> Elements => _elements;

        /// <summary>
        /// Returns or sets the id.
        /// </summary>
        public string Id
        {
            get => GetAttribute("id");
            set => SetAttribute("id", value);
        }

        /// <summary>
        /// Returns or sets the css class.
        /// </summary>
        public string Class
        {
            get => GetAttribute("class");
            set => SetAttribute("class", value);
        }

        /// <summary>
        /// Returns or sets the css style.
        /// </summary>
        public string Style
        {
            get => GetAttribute("style");
            set => SetAttribute("style", value);
        }

        /// <summary>
        /// Returns or sets the role.
        /// </summary>
        public string Role
        {
            get => GetAttribute("role");
            set => SetAttribute("role", value);
        }

        /// <summary>
        /// Returns or sets the html5 data attribute.
        /// </summary>
        public string DataToggle
        {
            get => GetAttribute("data-toggle");
            set => SetAttribute("data-toggle", value);
        }

        /// <summary>
        /// Returns or sets the html5 data attribute.
        /// </summary>
        public string DataProvide
        {
            get => GetAttribute("data-provide");
            set => SetAttribute("data-provide", value);
        }

        /// <summary>
        /// Returns or sets the on click attribute.
        /// </summary>
        public string OnClick
        {
            get => GetAttribute("onclick");
            set => SetAttribute("onclick", value);
        }

        /// <summary>
        /// Determines whether the element is inline.
        /// </summary>
        public bool Inline { get; set; }

        /// <summary>
        /// Determines whether the element needs an end tag.
        /// e.g.: true = <div></div> false = <br/>
        /// </summary>
        public bool CloseTag { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        public HtmlElement(string name, bool closeTag = true)
        {
            ElementName = name;
            CloseTag = closeTag;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        public HtmlElement(string name, bool closeTag, params IHtml[] nodes)
            : this(name, closeTag)
        {
            foreach (var v in nodes)
            {
                if (v is HtmlAttribute attr)
                {
                    _attributes.Add(attr);
                }
                else if (v is HtmlElement element)
                {
                    _elements.Add(element);
                }
                else if (v is HtmlText text)
                {
                    _elements.Add(text);
                }
            }
        }

        /// <summary>
        /// Adds one or more elements to the html element.
        /// </summary>
        /// <param name="elements">The elements to add.</param>
        public void Add(params IHtmlNode[] elements)
        {
            _elements.AddRange(elements);
        }

        /// <summary>
        /// Adds one or more elements to the beginning of the html element.
        /// </summary>
        /// <param name="elements">The elements to add.</param>
        public void AddFirst(params IHtmlNode[] elements)
        {
            _elements.InsertRange(0, elements);
        }
        /// <summary>
        /// Adds one or more attributes to the html element.
        /// </summary>
        /// <param name="attributes">The attributes to add.</param>
        public void Add(params IHtmlAttribute[] attributes)
        {
            _attributes.AddRange(attributes);
        }

        /// <summary>
        /// Clear all elements frrom the html element.
        /// </summary>
        public void Clear()
        {
            _elements.Clear();
        }

        /// <summary>
        /// Clear all elements from the html element that match the given predicate.
        /// </summary>
        /// <param name="predicate">The predicate to match elements.</param>
        protected void Clear(Func<IHtmlNode, bool> predicate)
        {
            _elements.RemoveAll(new Predicate<IHtmlNode>(predicate));
        }
        /// <summary>
        /// Returns the value of an attribute.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        /// <returns>The value of the attribute.</returns>
        protected string GetAttribute(string name)
        {
            var a = _attributes.Where(x => x.Name == name).FirstOrDefault();

            if (a != null)
            {
                return a is HtmlAttribute ? (a as HtmlAttribute).Value : string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Checks whether an attribute is set.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        /// <returns>True if attribute exists, false otherwise.</returns>
        protected bool HasAttribute(string name)
        {
            var a = _attributes.Where(x => x.Name == name).FirstOrDefault();

            return (a != null);
        }

        /// <summary>
        /// Sets the value of an attribute.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        /// <param name="value">The value of the attribute.</param>
        protected void SetAttribute(string name, string value)
        {
            var a = _attributes.Where(x => x.Name == name).FirstOrDefault();

            if (a != null)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _attributes.Remove(a);
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
                    _attributes.Add(new HtmlAttribute(name, value));
                }
            }
        }

        /// <summary>
        /// Setzt den Wert eines Attributs
        /// </summary>
        /// <param name="name">The attribute name.</param>
        protected void SetAttribute(string name)
        {
            var a = _attributes.Where(x => x.Name == name).FirstOrDefault();

            if (a == null)
            {
                _attributes.Add(new HtmlAttributeNoneValue(name));
            }
        }

        /// <summary>
        /// Removes an attribute.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        protected void RemoveAttribute(string name)
        {
            var a = _attributes.Where(x => x.Name == name).FirstOrDefault();

            if (a != null)
            {
                _attributes.Remove(a);
            }
        }

        /// <summary>
        /// Returns an element based on its name.
        /// </summary>
        /// <param name="name">The element name.</param>
        /// <returns>The element.</returns>
        protected HtmlElement GetElement(string name)
        {
            var a = _elements.Where(x => x is HtmlElement && (x as HtmlElement).ElementName == name).FirstOrDefault();

            return a as HtmlElement;
        }

        /// <summary>
        /// Sets an element.
        /// </summary>
        /// <param name="element">The element.</param>
        protected void SetElement(HtmlElement element)
        {
            if (element != null)
            {
                var a = _elements.Where(x => x is HtmlElement && (x as HtmlElement).ElementName == element.ElementName);

                foreach (var v in a)
                {
                    _elements.Remove(v);
                }

                _elements.Add(element);
            }
        }

        /// <summary>
        /// Returns the text.
        /// </summary>
        /// <returns>The text.</returns>
        protected string GetText()
        {
            var a = _elements.Where(x => x is HtmlText).Select(x => (x as HtmlText).Value);

            return string.Join(" ", a);
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public virtual void ToString(StringBuilder builder, int deep)
        {
            var closeTag = false;
            var nl = true;

            ToPreString(builder, deep);

            if (_elements.Count == 1 && Elements.First() is HtmlText)
            {
                closeTag = true;
                nl = false;

                _elements.First().ToString(builder, 0);
            }
            else if (_elements.Count > 0)
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
            else if (_elements.Count == 0)
            {
                nl = false;
            }

            if (closeTag || CloseTag)
            {
                ToPostString(builder, deep, nl);
            }
        }

        /// <summary>
        /// Converts the element to a string and appends it to the provided StringBuilder.
        /// </summary>
        /// <param name="builder">The StringBuilder to append the string representation to.</param>
        /// <param name="deep">The depth of the element in the HTML hierarchy, used for indentation.</param>
        protected virtual void ToPreString(StringBuilder builder, int deep)
        {
            if (!Inline)
            {
                builder.AppendLine();
                builder.Append(string.Empty.PadRight(deep));
            }

            builder.Append('<');
            builder.Append(ElementName);
            foreach (var attribute in Attributes)
            {
                builder.Append(' ');
                attribute.ToString(builder, 0);
            }

            builder.Append('>');
        }

        /// <summary>
        /// Converts the element to a string and appends the closing tag to the provided StringBuilder.
        /// </summary>
        /// <param name="builder">The StringBuilder to append the string representation to.</param>
        /// <param name="deep">The depth of the element in the HTML hierarchy, used for indentation.</param>
        /// <param name="nl">Indicates whether the closing tag should start on a new line.</param>
        protected virtual void ToPostString(StringBuilder builder, int deep, bool nl = true)
        {
            if (!Inline && nl)
            {
                builder.AppendLine();
                builder.Append(string.Empty.PadRight(deep));
            }

            builder.Append("</");
            builder.Append(ElementName);
            builder.Append('>');
        }

        /// <summary>
        /// Sets the value of an user-defined attribute.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        /// <param name="value">The value of the attribute.</param>
        public void AddUserAttribute(string name, string value)
        {
            SetAttribute(name, value);
        }

        /// <summary>
        /// Returns the value of an user-defined attribute.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        /// <returns>The value of the attribute.</returns>
        public string GetUserAttribute(string name)
        {
            return GetAttribute(name);
        }

        /// <summary>
        /// Checks if a user-defined attribute is set.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        /// <returns>True wenn Attribut vorhanden, false sonst</returns>
        public bool HasUserAttribute(string name)
        {
            return HasAttribute(name);
        }

        /// <summary>
        /// Removes an user-defined attribute.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        protected void RemoveUserAttribute(string name)
        {
            RemoveAttribute(name);
        }

        /// <summary>
        /// Convert to String.
        /// </summary>
        /// <returns>The object as a string.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            ToString(builder, 0);

            return builder.ToString();
        }
    }
}
