using System;
using System.Text.Json.Serialization;

namespace SevenZip.Compression.Models
{
    internal class PlatformSettingsModel
    {
        public PlatformSettingsModel()
        {
            Platform = "";
            SevenZipLibraryFilePaths = Array.Empty<String>();
        }

        /// <summary>
        /// <para>
        /// A key string to identify the platform.
        /// Its format is "&lt;os&gt;-&lt;architecture&gt;".
        /// </para>
        /// <para>
        /// Example: win-arm64
        /// </para>
        /// </summary>
        [JsonPropertyName("platform")]
        public String Platform { get; set; }

        /// <summary>
        /// A list of 7-zip native library pathnames.
        /// One of the following can be described in the path name.:
        /// <list type="bullet">
        /// <item>
        /// <term>For a character string that does not contain either a slash (/) or a backslash (\).</term>
        /// <description>The pathname is considered the filename of the 7-zip native library and is searched from the operating system's default directory.</description>
        /// </item>
        /// <item>
        /// <term>For character strings that start with "./", ". \", "../", or ".. \".</term>
        /// <description>The pathname is considered the filename of the 7-zip native library and is searched from the operating system's default directory</description>
        /// </item>
        /// <item>
        /// <term>In cases other than the above.</term>
        /// <description>Path names are considered absolute path names.</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// The path name can include the following macros:
        /// <list type="bullet">
        /// <item><term>${os}</term><description>The type of operating system you are running. (Example: "win", "linux", "osx", etc.)</description></item>
        /// <item><term>${architecture}:</term><description>The architecture of the running process. (Example: "x86", "x64", "arm64", etc.)</description></item>
        /// </list>
        /// </item>
        /// <item>
        /// The default directory depends on your operating system. For example, in the case of Windows, the search is performed in the following order.
        /// <list type="number">
        /// <item>The directory where the program resides</item>
        /// <item>system32 directory</item>
        /// <item>system directory</item>
        /// <item>windows directory</item>
        /// <item>Current directory</item>
        /// <item>Directory registered in environment variable</item>
        /// </list>
        /// </item>
        /// </list>
        /// </remarks>
        [JsonPropertyName("7zLibraryFilePaths")]
        public String[] SevenZipLibraryFilePaths { get; set; }
    }
}
