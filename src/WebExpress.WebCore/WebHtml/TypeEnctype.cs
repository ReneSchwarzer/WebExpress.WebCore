namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Specifies how the data is encoded when it is transferred to the server.
    /// </summary>
    public enum TypeEnctype
    {
        /// <summary>
        /// All characters are encoded (spaces are conferred to "+" and special characters in the hex representation).
        /// </summary>
        UrLEncoded,
        /// <summary>
        /// No characters will be encodes. Used when transferring files.
        /// </summary>
        None,
        /// <summary>
        /// Only space characters are encoded.
        /// </summary>
        Text,
        /// <summary>
        /// Not assignable.
        /// </summary>
        Default
    }

    public static class TypeEnctypeExtensions
    {
        /// <summary>
        /// Conversion of string to TypeEnctype.
        /// </summary>
        /// <param name="enctype">The coding.</param>
        /// <returns>The converted encoding.</returns>
        public static TypeEnctype Convert(string enctype)
        {
            return (enctype?.ToLower()) switch
            {
                "multipart/form-data" => TypeEnctype.None,
                "text/plain" => TypeEnctype.Text,
                "application/x-www-form-urlencoded" => TypeEnctype.UrLEncoded,
                _ => TypeEnctype.Default,
            };
        }


        /// <summary>
        /// Conversion to string.repräsentation
        /// </summary>
        /// <param name="enctype">The coding.</param>
        /// <returns>The converted encoding.</returns>
        public static string Convert(this TypeEnctype enctype)
        {
            return enctype switch
            {
                TypeEnctype.None => "multipart/form-data",
                TypeEnctype.Text => "text/plain",
                _ => "application/x-www-form-urlencoded",
            };
        }
    }
}
