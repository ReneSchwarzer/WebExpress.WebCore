using System;
using System.Collections.Generic;
using System.Linq;

namespace WebExpress.WebCore
{
    /// <summary>
    /// Parse the handoff arguments.
    /// </summary>
    public class ArgumentParser
    {
        /// <summary>
        /// The singelton.
        /// </summary>
        private static ArgumentParser m_this = null;

        /// <summary>
        /// Enumeration of all registered commands.
        /// </summary>
        private List<ArgumentParserCommand> Commands { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ArgumentParser()
        {
            Commands = new List<ArgumentParserCommand>();
        }

        /// <summary>
        /// Registers a command.
        /// </summary>
        /// <param name="command">The command to register.</param>
        public void Register(ArgumentParserCommand command)
        {
            Commands.Add(command);
        }

        /// <summary>
        /// Prepare program arguments.
        /// A program argument consists of a command and a value. The value can 
        /// contain the empty string, for example, -help. Commands beginning 
        /// with -- are considered comments and are not considered further.
        /// </summary>
        /// <param name="args">The program arguments.</param>
        /// <returns>A list of prepared program arguments.</returns>
        public ArguemtParserResult Parse(string[] args)
        {
            var argsDict = new ArguemtParserResult();

            var key = "";
            var value = "";

            foreach (var s in args)
            {
                if (s.StartsWith("--") == true)
                {
                }
                else if (s.StartsWith("-") == true)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        var command = (from x in Commands
                                       where x.FullName.Equals(key[1..], StringComparison.OrdinalIgnoreCase) ||
                                             x.ShortName.Equals(key[1..], StringComparison.OrdinalIgnoreCase)
                                       select x).FirstOrDefault();

                        if (command != null)
                        {
                            argsDict.Add(command.FullName.ToLower(), value.Trim());
                        }

                        value = "";
                    }
                    key = s;
                }
                else
                {
                    value += " " + s;
                }
            }

            if (!string.IsNullOrEmpty(key))
            {
                var command = (from x in Commands
                               where x.FullName.Equals(key[1..], StringComparison.OrdinalIgnoreCase) ||
                                     x.ShortName.Equals(key[1..], StringComparison.OrdinalIgnoreCase)
                               select x).FirstOrDefault();

                if (command != null)
                {
                    argsDict.Add(command.FullName.ToLower(), value.Trim());
                }
            }

            return argsDict;
        }

        /// <summary>
        /// Returns the recognized arguments.
        /// </summary>
        /// <param name="args">The program arguments.</param>
        /// <returns>the recognized argument.</returns>
        public string GetValidArguments(string[] args)
        {
            var argumentDict = Parse(args);

            var v = from x in argumentDict
                    select "-" + x.Key + (string.IsNullOrWhiteSpace(x.Value) ? "" : " " + x.Value);

            return string.Join(' ', v);
        }

        /// <summary>
        /// Returns the current ArgumentParser object.
        /// </summary>
        public static ArgumentParser Current
        {
            get
            {
                if (m_this == null)
                {
                    m_this = new ArgumentParser();
                }

                return m_this;
            }
        }
    }
}
