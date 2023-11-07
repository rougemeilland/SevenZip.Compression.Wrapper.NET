using System;

namespace SevenZip.Compression.Models
{
    class SettingsModel
    {
        public SettingsModel()
        {
            Plugins = Array.Empty<PluginKeyValueModel>();
        }

        public PluginKeyValueModel[] Plugins { get; set; }
    }
}
