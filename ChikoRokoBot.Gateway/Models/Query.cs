using System;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Gateway.Models
{
    public record Query(
        [property: JsonPropertyName("slug")] string Slug
    );
}

