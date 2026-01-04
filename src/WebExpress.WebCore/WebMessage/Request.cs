using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebExpress.WebCore.WebHtml;
using WebExpress.WebCore.WebParameter;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// See RFC 2616, The Request class encapsulates and extends the 
    /// original request of the HttpListener call.
    /// </summary>
    public class Request : RequestBase
    {
        /// <summary>
        /// Returns the content.
        /// </summary>
        public byte[] Content { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="contextFeatures">Initial set of features.</param>
        /// <param name="header">The header.</param>
        /// <param name="httpServerContext">The context of the web server.</param>
        internal Request(IFeatureCollection contextFeatures, RequestHeaderFields header, IHttpServerContext httpServerContext)
            : base(contextFeatures, header, httpServerContext)
        {
            var requestFeature = contextFeatures.Get<IHttpRequestFeature>();

            Content = GetContent(requestFeature.Body, Header.ContentLength);
        }

        /// <summary>
        /// Returns the content.
        /// </summary>
        /// <param name="body">The content of a request.</param>
        /// <param name="contentLength">The number of bytes sent in the body or zero.</param>
        /// <returns>Der Content als Byte-Array</returns>
        internal static byte[] GetContent(Stream body, long? contentLength)
        {
            if (!contentLength.HasValue || contentLength.Value == 0)
            {
                return null;
            }

            using var ms = new MemoryStream();
            body.CopyTo(ms);

            return ms.ToArray();
        }

        /// <summary>
        /// Parse the request parameters.
        /// </summary>
        protected override void ParseRequestParams()
        {
            if (string.IsNullOrWhiteSpace(Header.ContentType))
            {
                return;
            }

            var contentType = Header.ContentType?.Split(';');

            switch (TypeEnctypeExtensions.Convert(contentType.FirstOrDefault()))
            {
                case TypeEnctype.None:
                    {
                        var boundary = Header.ContentType;
                        var boundaryValue = "--" + boundary?.Split('=').Skip(1)?.FirstOrDefault();
                        var offset = 0;
                        int pos = 0;
                        var dispositions = new List<Tuple<int, int>>(); // Item1=position, Item2=size

                        // determine dispositions
                        for (var i = 0; i < Content?.Length; i++)
                        {
                            if (Content[i] == '\r')
                            {
                                var c = Encoding.UTF8.GetString(Content, offset, boundaryValue.Length).Trim();
                                if (c.StartsWith(boundaryValue))
                                {
                                    if (i - boundaryValue.Length - pos > 0)
                                    {
                                        dispositions.Add(new Tuple<int, int>(pos, i - boundaryValue.Length - pos));
                                    }

                                    pos = i + 2;

                                    if (c.EndsWith("--"))
                                    {
                                        break;
                                    }
                                }
                            }
                            else if (Content[i] == '\n')
                            {
                                offset = i + 1;
                            }
                            else if (i == Content.Length - 1)
                            {
                                // at the end
                                var c = Encoding.UTF8.GetString(Content, offset, boundaryValue.Length).Trim();
                                if (c.StartsWith(boundaryValue))
                                {
                                    dispositions.Add(new Tuple<int, int>(pos, i - boundaryValue.Length - pos));
                                }
                            }
                        }

                        foreach (var item in dispositions)
                        {
                            var disposition = string.Empty;
                            var name = string.Empty;
                            var filename = string.Empty;
                            var contenttype = string.Empty;
                            offset = 0;

                            var str = Encoding.UTF8.GetString(Content, item.Item1, item.Item2 > 256 ? 256 : item.Item2);
                            var match = Regex.Match(str, @"^Content-Disposition: (.*)$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
                            if (match.Groups[1].Success)
                            {
                                offset += match.Length + 1; // + Zeilenende
                                var dispositionParam = match.Groups[1].ToString().Split(';');
                                disposition = dispositionParam.FirstOrDefault();
                                foreach (var v in dispositionParam.Skip(1))
                                {
                                    match = Regex.Match(v.Trim(), @"^name=""(.*)""$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                                    if (match.Groups[1].Success)
                                    {
                                        name = match.Groups[1].ToString();
                                    }

                                    match = Regex.Match(v.Trim(), @"^filename=""(.*)""$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                                    if (match.Groups[1].Success)
                                    {
                                        filename = match.Groups[1].ToString();
                                    }
                                }
                            }

                            match = Regex.Match(str, @"^Content-Type: (.*)$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
                            if (match.Groups[1].Success)
                            {
                                offset += match.Length + 1; // + End of line
                                contenttype = match.Groups[1].ToString().Trim();
                            }

                            if (string.IsNullOrWhiteSpace(filename))
                            {
                                offset += 2; // + blank line
                                if (item.Item2 - offset - 1 >= 0)
                                {
                                    var value = Encoding.UTF8.GetString(Content, item.Item1 + offset, item.Item2 - offset - 2);

                                    var param = new Parameter(name, value.TrimEnd(), ParameterScope.Parameter);
                                    AddParameter(param);
                                }
                                else
                                {
                                    var param = new Parameter(name, string.Empty, ParameterScope.Parameter);
                                    AddParameter(param);
                                }
                            }
                            else
                            {
                                offset += 2; // + blank line
                                if (item.Item2 - offset - 1 >= 0)
                                {
                                    var bytes = new byte[item.Item2 - offset - 2];
                                    Buffer.BlockCopy(Content, item.Item1 + offset, bytes, 0, item.Item2 - offset - 2);

                                    var param = new ParameterFile(name, filename, ParameterScope.Parameter) { ContentType = contenttype, Data = bytes };
                                    AddParameter(param);
                                }
                                else
                                {
                                    var param = new Parameter(name, filename, ParameterScope.Parameter);
                                    AddParameter(param);
                                }
                            }
                        }

                        break;
                    }
                case TypeEnctype.Text:
                    {
                        var lines = new List<string>();
                        var offset = 0;

                        for (var i = 0; i < Content.Length; i++)
                        {
                            if (Content[i] == '\r')
                            {
                                lines.Add(Encoding.UTF8.GetString(Content, offset, i - offset));
                            }
                            else if (Content[i] == '\n')
                            {
                                offset = i + 1;
                            }
                        }

                        // if not all bytes have been read yet
                        if (offset < Content.Length)
                        {
                            lines.Add(Encoding.UTF8.GetString(Content, offset, Content.Length - offset));
                        }

                        var last = default(Parameter);

                        foreach (var v in lines)
                        {
                            var match = Regex.Match(v, @"([\w-]*)=(.*)", RegexOptions.Compiled);
                            if (match.Groups[1].Success && match.Groups[2].Success)
                            {
                                last = new Parameter(match.Groups[1].ToString().Trim(), match.Groups[2].ToString().Trim(), ParameterScope.Parameter);
                                AddParameter(last);
                            }
                            else if (last is not null)
                            {
                                last.Value += "\r\n" + v;

                            }
                        }

                        if (last is not null)
                        {
                            last.Value = last.Value.TrimEnd();
                        }

                        break;
                    }
                case TypeEnctype.UrLEncoded:
                    {
                        var str = Encoding.UTF8.GetString(Content, 0, Content.Length);
                        var param = str.Replace('+', ' ');

                        foreach (var v in param.Split('&'))
                        {
                            var s = v.Split('=');
                            AddParameter(new Parameter
                            (
                                s[0],
                                s.Length > 1 ? s[1]?.TrimEnd() : string.Empty,
                                ParameterScope.Parameter
                            ));
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
