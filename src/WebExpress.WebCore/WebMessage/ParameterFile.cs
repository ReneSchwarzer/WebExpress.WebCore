namespace WebExpress.WebCore.WebMessage
{
    public class ParameterFile : Parameter
    {
        /// <summary>
        /// Returns the content type.
        /// </summary>
        public string ContentType { get; internal set; }

        /// <summary>
        /// Returns the data.
        /// </summary>
        public byte[] Data { get; internal set; }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ParameterFile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="scope">The scope of the parameter.</param>
        public ParameterFile(string key, string value, ParameterScope scope)
            : base(key, value, scope)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="scope">The scope of the parameter.</param>
        public ParameterFile(string key, int value, ParameterScope scope)
            : base(key, value, scope)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="scope">The scope of the parameter.</param>
        public ParameterFile(string key, char value, ParameterScope scope)
            : base(key, value, scope)
        {
        }
    }
}
