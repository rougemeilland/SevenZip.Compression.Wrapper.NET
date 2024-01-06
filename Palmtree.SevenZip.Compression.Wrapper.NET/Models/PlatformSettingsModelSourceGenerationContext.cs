using System.Text.Json.Serialization;

namespace SevenZip.Compression.Models
{
    [JsonSerializable(typeof(PlatformSettingsModel))]
    internal partial class PlatformSettingsModelSourceGenerationContext
        : JsonSerializerContext
    {
    }
}
