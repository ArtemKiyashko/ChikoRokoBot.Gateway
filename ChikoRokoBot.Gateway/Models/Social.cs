using System;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Gateway.Models
{
    public record Social(
        [property: JsonPropertyName("translate_key")] string TranslateKey,
        [property: JsonPropertyName("href")] string Href,
        [property: JsonPropertyName("icon")] string Icon
    );
}

