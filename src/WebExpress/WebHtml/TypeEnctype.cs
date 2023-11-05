namespace WebExpress.WebHtml
{
    /// <summary>
    /// Legt fest, wie die Daten encoded werden, wenn sie zum Server übertragen werden.
    /// </summary>
    public enum TypeEnctype
    {
        /// <summary>
        /// Alle Zeichen werden encoded (Spaces wird zu "+" koncertiert und spezalzeichen in der Hexrepräsentation) 
        /// </summary>
        UrLEncoded,
        /// <summary>
        /// Keine Zeichen werden encodes. Wird verwendet, wenn Dateien übertragen werden
        /// </summary>
        None,
        /// <summary>
        /// Nur Space-Zeichen werden encodiert.
        /// </summary>
        Text,
        /// <summary>
        /// Nicht zuordbar
        /// </summary>
        Default
    }

    public static class TypeEnctypeExtensions
    {
        /// <summary>
        /// Umwandlung von String in TypeEnctype
        /// </summary>
        /// <param name="enctype">Die Kodierung</param>
        /// <returns>Die umgewandelte Kodierung</returns>
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
        /// <param name="enctype">Die Kodierung</param>
        /// <returns>Die umgewandelte Kodierung</returns>
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
