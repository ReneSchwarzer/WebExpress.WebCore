using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebExpress.Core.WebComponent;
using WebExpress.Core.WebHtml;
using WebExpress.Core.WebSession;
using WebExpress.Core.WebUri;

namespace WebExpress.Core.WebMessage
{
    /// <summary>
    /// See RFC 2616, The Request class encapsulates and extends the 
    /// original request of the HttpListener call.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// The context of the web server.
        /// </summary>
        public IHttpServerContext ServerContext { get; protected set; }

        /// <summary>
        /// Returns the request method (e.g. POST).
        /// </summary>
        public RequestMethod Method { get; private set; }

        /// <summary>
        /// Returns the uri.
        /// </summary>
        public UriResource Uri { get; internal set; }

        /// <summary>
        /// Returns the parameters.
        /// </summary>
        private ParameterDictionary Param { get; } = new ParameterDictionary();

        /// <summary>
        /// Returns the session.
        /// </summary>
        public Session Session { get; private set; }

        /// <summary>
        /// Returns the http version.
        /// </summary>
        public string Protocoll { get; private set; }

        /// <summary>
        /// Returns the options from the header.
        /// </summary>
        public RequestHeaderFields Header { get; private set; }

        /// <summary>
        /// Returns the ip address and port number of the server to which the request is made.
        /// </summary>
        public EndPoint LocalEndPoint { get; private set; }

        /// <summary>
        /// Returns the ip address and port number of the client from which the request originated.
        /// </summary>
        public EndPoint RemoteEndPoint { get; private set; }

        /// <summary>
        /// Returns a boolean value that indicates whether the client sending this request is authenticated.
        /// </summary>
        //public bool IsAuthenticated { get; private set; }  //=> RawRequuest.IsAuthenticated;

        /// <summary>
        /// Returns a boolean value that indicates whether the request was sent from the local computer.
        /// </summary>
        //public bool IsLocal { get; private set; }  //=> RawRequuest.IsLocal;

        /// <summary>
        /// Returns a boolean value that indicates whether the tcp connection used to send the request uses the secure sockets layer (ssl) protocol.
        /// </summary>
        public bool IsSecureConnection { get; private set; }

        /// <summary>
        /// Returns a boolean value indicating whether the tcp connection was a web socket request.
        /// </summary>
        //public bool IsWebSocketRequest { get; private set; }  // => RawRequuest.IsWebSocketRequest;

        /// <summary>
        /// Returns a boolean value that indicates whether the client is requesting a persistent connection.
        /// </summary>
        //public bool KeepAlive { get; private set; }  //=> RawRequuest.KeepAlive;

        /// <summary>
        /// Returns the shema. This can be http or https.
        /// </summary>
        public UriScheme Scheme { get; private set; }

        /// <summary>
        /// Returns the request identifier of the incoming http request.
        /// </summary>
        public string RequestTraceIdentifier { get; private set; }

        /// <summary>
        /// Returns the culture.
        /// </summary>
        public CultureInfo Culture
        {
            get
            {
                try
                {
                    // see RFC 5646 
                    var languages = Header?.AcceptLanguage.FirstOrDefault();
                    var language = languages?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).FirstOrDefault();

                    return new CultureInfo(language);
                }
                catch
                {
                    return ServerContext.Culture ?? CultureInfo.CurrentCulture;
                }
            }
        }

        /// <summary>
        /// Returns the content.
        /// </summary>
        public byte[] Content { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contextFeatures">Initial set of features.</param>
        /// <param name="serverContext">The context of the web server.</param>
        /// <param name="header">The header.</param>
        internal Request(IFeatureCollection contextFeatures, IHttpServerContext serverContext, RequestHeaderFields header)
        {
            var connectionFeature = contextFeatures.Get<IHttpConnectionFeature>();
            var requestFeature = contextFeatures.Get<IHttpRequestFeature>();
            var requestIdentifierFeature = contextFeatures.Get<IHttpRequestIdentifierFeature>();
            //var sessionFeature = contextFeatures.Get<ISessionFeature>();

            ServerContext = serverContext;
            RequestTraceIdentifier = requestIdentifierFeature.TraceIdentifier;
            Protocoll = requestFeature.Protocol;

            Scheme = requestFeature.Scheme.ToLower() switch
            {
                "http" => UriScheme.Http,
                "https" => UriScheme.Https,
                "ftp" => UriScheme.FTP,
                "file" => UriScheme.File,
                "mailto" => UriScheme.Mailto,
                "ldap" => UriScheme.Ldap,
                _ => UriScheme.Http

            };
            Method = requestFeature.Method.ToUpper() switch
            {
                "GET" => RequestMethod.GET,
                "POST" => RequestMethod.POST,
                "PUT" => RequestMethod.PUT,
                "DELETE" => RequestMethod.DELETE,
                "HEAD" => RequestMethod.HEAD,
                "PATCH" => RequestMethod.PATCH,
                _ => RequestMethod.GET
            };

            Header = header;

            LocalEndPoint = new IPEndPoint(connectionFeature.LocalIpAddress, connectionFeature.LocalPort);
            RemoteEndPoint = new IPEndPoint(connectionFeature.RemoteIpAddress, connectionFeature.RemotePort);

            Uri = new UriResource
            (
                Scheme,
                new UriAuthority()
                {
                    Host = Header.Host,
                    Port = connectionFeature.LocalPort
                },
                requestFeature.RawTarget
            );

            Content = GetContent(requestFeature.Body, Header.ContentLength);

            ParseQueryParams(requestFeature.QueryString);
            ParseRequestParams();
            ParseSessionParams();
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
        /// Returns the parameters from the reuest query (for example, http://www.example.com?key=value).
        /// </summary>
        /// <param name="query">The query.</param>
        private void ParseQueryParams(string query)
        {
            query = query.TrimStart('?');

            Parallel.ForEach(query.Split('&'), (param) =>
            {
                if (!string.IsNullOrWhiteSpace(param))
                {
                    var split = param.Split('=');

                    if (split.Length == 1)
                    {
                        AddParameter(new Parameter(split[0], null, ParameterScope.Parameter));
                    }
                    else if (split.Length == 2)
                    {
                        AddParameter(new Parameter(split[0], split[1], ParameterScope.Parameter));
                    }
                    else if (split.Length > 2)
                    {
                        AddParameter(new Parameter(split[0], string.Join("=", split.Skip(1)), ParameterScope.Parameter));
                    }
                }
            });
        }

        /// <summary>
        /// Parse the request parameters.
        /// </summary>
        private void ParseRequestParams()
        {
            if (string.IsNullOrWhiteSpace(Header.ContentType))
            {
                return;
            }

            var contentType = Header.ContentType?.Split(';');
            //var contentStr = Encoding.UTF8.GetString(Content);

            switch (TypeEnctypeExtensions.Convert(contentType.FirstOrDefault()))
            {
                case TypeEnctype.None:
                    {
                        var boundary = Header.ContentType;
                        var boundaryValue = "--" + boundary?.Split('=').Skip(1)?.FirstOrDefault();
                        var offset = 0;
                        int pos = 0;
                        var dispositions = new List<Tuple<int, int>>(); // Item1=Position, Item2=Länge

                        // determine dispositions
                        for (var i = 0; i < Content.Length; i++)
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
                            else if (last != null)
                            {
                                last.Value += "\r\n" + v;

                            }
                        }

                        if (last != null)
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

        /// <summary>
        /// Parse the session parameters.
        /// </summary>
        private void ParseSessionParams()
        {
            Session = ComponentManager.SessionManager.GetSession(this);

            var property = Session.GetProperty<SessionPropertyParameter>();
            if (property != null && property.Params != null)
            {
                foreach (var param in property.Params)
                {
                    AddParameter(new Parameter(param.Key?.ToLower(), param.Value.Value, ParameterScope.Session));
                }
            }
        }

        /// <summary>
        /// Adds several parameters.
        /// </summary>
        /// <param name="param">The parameters.</param>
        public void AddParameter(IEnumerable<Parameter> param)
        {
            foreach (var p in param)
            {
                AddParameter(p);
            }
        }

        /// <summary>
        /// Adds one parameter.
        /// </summary>
        /// <param name="param">The parameter.</param>
        public void AddParameter(Parameter param)
        {
            if (!Param.ContainsKey(param.Key.ToLower()))
            {
                Param.Add(param.Key.ToLower(), param);
            }
            else
            {
                Param[param.Key.ToLower()] = param;
            }
        }

        /// <summary>
        /// Returns a parameter by name.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>The value.</returns>
        public Parameter GetParameter(string name)
        {
            if (!string.IsNullOrWhiteSpace(name) && HasParameter(name))
            {
                return Param[name.ToLower()];
            }

            return null;
        }

        /// <summary>
        /// Returns a parameter by name.
        /// </summary>
        /// <typeparam name="T">The parameter.</typeparam>
        /// <returns>The value.</returns>
        public Parameter GetParameter<T>() where T : Parameter
        {
            var name = Parameter.GetKey<T>();

            if (!string.IsNullOrWhiteSpace(name) && HasParameter(name))
            {
                return Param[name.ToLower()];
            }

            return null;
        }

        /// <summary>
        /// Checks whether a parameter exists.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>True if parameters are present, false otherwise.</returns>
        public bool HasParameter(string name)
        {
            if (name == null)
            {
                return false;
            }

            return Param.ContainsKey(name.ToLower());
        }
    }
}
