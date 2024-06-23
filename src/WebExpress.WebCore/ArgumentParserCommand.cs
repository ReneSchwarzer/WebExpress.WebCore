namespace WebExpress.WebCore
{
    /// <summary>
    /// A command which is controlled by the program arguments.
    /// </summary>
    public class ArgumentParserCommand
    {
        /// <summary>
        /// The full name of the command.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The short name of the command.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// The description of the command.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The description of the parameter.
        /// </summary>
        public string ParameterDescription { get; set; }

        /// <summary>
        /// Returns or sets whether the command is mandatory.
        /// </summary>
        public bool Mandatory { get; set; } = false;
    }
}
