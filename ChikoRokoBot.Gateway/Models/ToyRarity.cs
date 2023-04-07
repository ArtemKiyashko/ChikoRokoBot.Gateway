using System;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Gateway.Models
{
    public record ToyRarity(
        [property: JsonPropertyName("id")] int? Id,
        [property: JsonPropertyName("label")] string Label,
        [property: JsonPropertyName("range")] string Range,
        [property: JsonPropertyName("amount")] int? Amount
    );
}

