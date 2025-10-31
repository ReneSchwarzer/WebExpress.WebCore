namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents supported content types for file handling and MIME mapping.
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// Unknown or unsupported content type.
        /// </summary>
        Unknown,

        /// <summary>
        /// Portable Document Format (.pdf).
        /// </summary>
        Pdf,

        /// <summary>
        /// Plain text file (.txt).
        /// </summary>
        Txt,

        /// <summary>
        /// Cascading Style Sheets (.css).
        /// </summary>
        Css,

        /// <summary>
        /// JavaScript file (.js).
        /// </summary>
        Js,

        /// <summary>
        /// XML file (.xml).
        /// </summary>
        Xml,

        /// <summary>
        /// HTML file (.htm).
        /// </summary>
        Htm,

        /// <summary>
        /// HTML file (.html).
        /// </summary>
        Html,

        /// <summary>
        /// ZIP archive (.zip).
        /// </summary>
        Zip,

        /// <summary>
        /// Microsoft Word document (.doc).
        /// </summary>
        Doc,

        /// <summary>
        /// Microsoft Word Open XML document (.docx).
        /// </summary>
        Docx,

        /// <summary>
        /// Microsoft Excel spreadsheet (.xls).
        /// </summary>
        Xls,

        /// <summary>
        /// Microsoft Excel Open XML spreadsheet (.xlx).
        /// </summary>
        Xlx,

        /// <summary>
        /// Microsoft PowerPoint presentation (.ppt).
        /// </summary>
        Ppt,

        /// <summary>
        /// Graphics Interchange Format image (.gif).
        /// </summary>
        Gif,

        /// <summary>
        /// Portable Network Graphics image (.png).
        /// </summary>
        Png,

        /// <summary>
        /// Scalable Vector Graphics image (.svg).
        /// </summary>
        Svg,

        /// <summary>
        /// JPEG image (.jpeg).
        /// </summary>
        Jpeg,

        /// <summary>
        /// JPEG image (.jpg).
        /// </summary>
        Jpg,

        /// <summary>
        /// Icon file (.ico).
        /// </summary>
        Ico,

        /// <summary>
        /// WebP image format.
        /// </summary>
        WebP,

        /// <summary>
        /// MPEG audio file (.mp3).
        /// </summary>
        Mp3,

        /// <summary>
        /// MPEG-4 video file (.mp4).
        /// </summary>
        Mp4
    }

    /// <summary>
    /// Provides extension methods for converting file extensions and MIME types to <see cref="ContentType"/> values.
    /// </summary>
    /// <remarks>
    /// This class includes utility methods for mapping common file formats and media types to their corresponding <c>ContentType</c> enumeration.
    /// It supports both extension-based (e.g. ".png") and MIME-based (e.g. "image/png") conversions.
    /// </remarks>

    public static class ContentTypeExtensions
    {
        /// <summary>
        /// Converts a file extension (e.g. ".png") to a ContentType enum.
        /// </summary>
        public static ContentType ToContentType(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                return ContentType.Unknown;
            }

            extension = extension.Trim().ToLowerInvariant();
            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            return extension switch
            {
                ".pdf" => ContentType.Pdf,
                ".txt" => ContentType.Txt,
                ".css" => ContentType.Css,
                ".js" => ContentType.Js,
                ".xml" => ContentType.Xml,
                ".html" => ContentType.Html,
                ".htm" => ContentType.Htm,
                ".zip" => ContentType.Zip,
                ".doc" => ContentType.Doc,
                ".docx" => ContentType.Docx,
                ".xls" => ContentType.Xls,
                ".xlx" => ContentType.Xlx,
                ".ppt" => ContentType.Ppt,
                ".gif" => ContentType.Gif,
                ".png" => ContentType.Png,
                ".svg" => ContentType.Svg,
                ".jpeg" => ContentType.Jpeg,
                ".jpg" => ContentType.Jpg,
                ".ico" => ContentType.Ico,
                ".webp" => ContentType.WebP,
                ".mp3" => ContentType.Mp3,
                ".mp4" => ContentType.Mp4,
                _ => ContentType.Unknown,
            };
        }

        /// <summary>
        /// Converts a MIME type string (e.g. "application/pdf") to a ContentType enum.
        /// </summary>
        public static ContentType ToContentTypeFromMime(string mimeType)
        {
            if (string.IsNullOrWhiteSpace(mimeType))
            {
                return ContentType.Unknown;
            }

            mimeType = mimeType.Trim().ToLowerInvariant();

            return mimeType switch
            {
                "application/pdf" => ContentType.Pdf,
                "text/plain" => ContentType.Txt,
                "text/css" => ContentType.Css,
                "application/javascript" => ContentType.Js,
                "application/xml" => ContentType.Xml,
                "text/xml" => ContentType.Xml,
                "text/html" => ContentType.Html,
                "application/zip" => ContentType.Zip,
                "application/msword" => ContentType.Doc,
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => ContentType.Docx,
                "application/vnd.ms-excel" => ContentType.Xls,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => ContentType.Xlx,
                "application/vnd.ms-powerpoint" => ContentType.Ppt,
                "image/gif" => ContentType.Gif,
                "image/png" => ContentType.Png,
                "image/svg+xml" => ContentType.Svg,
                "image/jpeg" => ContentType.Jpeg,
                "image/jpg" => ContentType.Jpg,
                "image/x-icon" => ContentType.Ico,
                "image/webp" => ContentType.WebP,
                "audio/mpeg" => ContentType.Mp3,
                "video/mp4" => ContentType.Mp4,
                _ => ContentType.Unknown,
            };
        }

        /// <summary>
        /// Returns the MIME type string associated with the specified <see cref="ContentType"/> value.
        /// </summary>
        /// <param name="extension">The <see cref="ContentType"/> value to convert.</param>
        /// <returns>
        /// A MIME type string such as <c>"image/png"</c> or <c>"application/pdf"</c>. 
        /// If the content type is <see cref="ContentType.Unknown"/>, an empty string is returned.
        /// </returns>
        public static string GetMimeType(this ContentType extension)
        {
            return extension switch
            {
                ContentType.Pdf => "application/pdf",
                ContentType.Txt => "text/plain",
                ContentType.Css => "text/css",
                ContentType.Js => "application/javascript",
                ContentType.Xml => "text/xml",
                ContentType.Html => "text/html",
                ContentType.Htm => "text/html",
                ContentType.Zip => "application/zip",
                ContentType.Doc => "application/msword",
                ContentType.Docx => "application/msword",
                ContentType.Xls => "application/vnd.ms-excel",
                ContentType.Xlx => "application/vnd.ms-excel",
                ContentType.Ppt => "application/vnd.ms-powerpoint",
                ContentType.Gif => "image/gif",
                ContentType.Png => "image/png",
                ContentType.Svg => "image/svg+xml",
                ContentType.Jpeg => "image/jpeg",
                ContentType.Jpg => "image/jpeg",
                ContentType.Ico => "image/x-icon",
                ContentType.WebP => "image/webp",
                ContentType.Mp3 => "audio/mpeg",
                ContentType.Mp4 => "video/mp4",
                _ => "application/octet-stream",
            };
        }

        /// <summary>
        /// Returns a file search pattern (e.g. "*.png") associated with the specified <see cref="ContentType"/> value.
        /// </summary>
        /// <param name="extension">The <see cref="ContentType"/> value to convert.</param>
        /// <returns>
        /// A file pattern string such as <c>"*.pdf"</c> or <c>"*.jpg"</c>. 
        /// If the content type is <see cref="ContentType.Unknown"/>, an empty string is returned.
        /// </returns>

        public static string GetFilePattern(this ContentType extension)
        {
            return extension switch
            {
                ContentType.Pdf => "*.pdf",
                ContentType.Txt => "*.txt",
                ContentType.Css => "*.css",
                ContentType.Js => "*.js",
                ContentType.Xml => "*.xml",
                ContentType.Html => "*.html",
                ContentType.Htm => "*.htm",
                ContentType.Zip => "*.zip",
                ContentType.Doc => "*.doc",
                ContentType.Docx => "*.docx",
                ContentType.Xls => "*.xls",
                ContentType.Xlx => "*.xlx",
                ContentType.Ppt => "*.ppt",
                ContentType.Gif => "*.gif",
                ContentType.Png => "*.png",
                ContentType.Svg => "*.svg",
                ContentType.Jpeg => "*.jpeg",
                ContentType.Jpg => "*.jpg",
                ContentType.Ico => "*.ico",
                ContentType.WebP => "*.webp",
                ContentType.Mp3 => "*.mp3",
                ContentType.Mp4 => "*.mp4",
                _ => "*.*",
            };
        }
    }
}
