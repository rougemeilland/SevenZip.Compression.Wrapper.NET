using System.Text.Json.Serialization;

namespace SevenZip.Compression.Models
{
    [JsonSerializable(typeof(SettingsModel))]
    internal partial class SettingsModelSourceGenerationContext
        : JsonSerializerContext
    {
    }
}
