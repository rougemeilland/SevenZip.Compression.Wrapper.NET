using System;
using System.Text.Json.Serialization;

namespace SevenZip.Compression.Models
{
    internal class SettingsModel
    {
        public SettingsModel()
        {
            Entries = Array.Empty<PlatformSettingsModel>();
        }

        [JsonPropertyName("entries")]
        public PlatformSettingsModel[] Entries { get; set; }
    }
}
