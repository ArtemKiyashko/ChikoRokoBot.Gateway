using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Gateway.Models
{
    public record ArtistStatistic(
        [property: JsonPropertyName("kind")] string Kind,
        [property: JsonPropertyName("toys")] int? Toys,
        [property: JsonPropertyName("likes")] int? Likes,
        [property: JsonPropertyName("rarity")] IReadOnlyList<StatisticRarity> Rarity,
        [property: JsonPropertyName("version")] string Version
    );
}

