using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebExpress.Core.WebMessage
{
    public class RequestAuthorization
    {
        /// <summary>
        /// Returns or sets the type.e (Basic bei WWW-Authenticate: Basic realm="RealmName")
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Liefert oder setzt den Loginnamen
        /// </summary>
        public string Identification { get; set; }

        /// <summary>
        /// Liefert oder setzt das Passwort
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Parst die Authorization-Anforderung
        /// </summary>
        /// <param name="str">Authorizations-Zeichenkette e.g. Basic d2lraTpwZWRpYQ==</param>
        /// <returns></returns>
        public static RequestAuthorization Parse(string str)
        {
            if (str == null)
            {
                return null;
            }

            var m = Regex.Match(str, "^(.*) (.*)$");
            var type = "Basic";
            var user = "";
            var password = "";

            if (m.Success && m.Groups.Count >= 3)
            {
                type = m.Groups[1].Value;
                var userPw = m.Groups[2].Value;
                userPw = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(userPw));

                var split = userPw.Split(':');

                user = split[0];
                password = split.Count() > 0 ? split[1] : "";
            }

            return new RequestAuthorization()
            {
                Type = type,
                Identification = user,
                Password = password
            };
        }
    }
}
