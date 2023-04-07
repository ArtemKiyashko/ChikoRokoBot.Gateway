using System;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Gateway.Models
{
    public record Accept(
        [property: JsonPropertyName("webp")] bool Webp
    );
}

