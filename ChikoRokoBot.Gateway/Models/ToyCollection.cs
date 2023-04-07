using System;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Gateway.Models
{
    public record ToyCollection(
        [property: JsonPropertyName("id")] int? Id,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("imageid")] string Imageid,
        [property: JsonPropertyName("toysPublished")] int? ToysPublished,
        [property: JsonPropertyName("toysTotal")] int? ToysTotal,
        [property: JsonPropertyName("slug")] string Slug,
        [property: JsonPropertyName("popularity")] int? Popularity,
        [property: JsonPropertyName("created")] DateTime Created
    );
}

