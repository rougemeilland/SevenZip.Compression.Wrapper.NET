using System;

namespace SevenZip.Compression.Models
{
    class PluginKeyValueModel
    {
        public PluginKeyValueModel()
        {
            Platform = "";
            Settings = new PluginSettingModel();
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
        public string Platform { get; set; }
        public PluginSettingModel Settings {get;set;}
    }
}
