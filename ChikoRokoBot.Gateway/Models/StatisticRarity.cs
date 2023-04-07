using System;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Gateway.Models
{
    public record StatisticRarity(
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("count")] int? Count
    );
}

