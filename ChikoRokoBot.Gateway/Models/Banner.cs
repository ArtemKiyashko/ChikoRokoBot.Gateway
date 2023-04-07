using System;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Gateway.Models
{
    public record Banner(
        [property: JsonPropertyName("src")] string Src,
        [property: JsonPropertyName("mobileSrc")] string MobileSrc,
        [property: JsonPropertyName("href")] string Href,
        [property: JsonPropertyName("background")] string Background,
        [property: JsonPropertyName("target")] string Target,
        [property: JsonPropertyName("rel")] string Rel,
        [property: JsonPropertyName("region")] string Region
    );
}

