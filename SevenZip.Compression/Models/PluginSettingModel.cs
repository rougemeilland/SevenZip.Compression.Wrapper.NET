using System;

namespace SevenZip.Compression.Models
{
    class PluginSettingModel
    {
        public PluginSettingModel()
        {
            PluginDirs = Array.Empty<string>();
            SevenZipLibraryFilePaths = Array.Empty<string>();
        }

        /// <summary>
        /// A list of directory pathnames to search for plugin assemblies.
        /// Plugin assemblies are searched from the top of this list.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the directory path name starts with "./" or "../", it is considered to be a relative path from the directory where the assembly "SevenZip.Compression" is located.
        /// If other than the above, the directory path name is regarded as an absolute path name.
        /// </para>
        /// <para>
        /// The directory path name can contain the following macros:
        /// <list type="bullet">
        /// <item><term>${framework}:</term><description>The version of the framework that is running. (Example: "net50", "net60", etc.)</description></item>
        /// <item><term>${os}</term><description>The type of operating system you are running. (Example: "win", "linux", "macOs", etc.)</description></item>
        /// <item><term>${architecture}:</term><description>The architecture of the running process. (Example: "x86", "x64", "arm64", etc.)</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public string[] PluginDirs { get; set; }

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
        /// <item><term>${Framework}:</term><description>The version of the framework that is running. (Example: "net50", "net60", etc.)</description></item>
        /// <item><term>${os}</term><description>The type of operating system you are running. (Example: "win", "linux", "macOs", etc.)</description></item>
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
        public string[] SevenZipLibraryFilePaths { get; set; }
    }
}
