using System;

namespace SevenZip.Compression.Models
{
    class PluginModel
    {
        public PluginModel()
        {
            Os = "";
            Bits = 0;
            SubDir = null;
            FileNamePattern = "";
        }

        public string Os { get; set; }
        public Int32 Bits { get; set; }
        public string? SubDir { get; set; }
        public string FileNamePattern { get; set; }
    }
}
