using System;

namespace SevenZip.Compression.Models
{
    class SettingsModel
    {
        public SettingsModel()
        {
            Plugins = Array.Empty<PluginModel>();
        }

        public PluginModel[] Plugins { get; set; }
    }
}
