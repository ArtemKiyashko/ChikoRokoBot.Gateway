using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Gateway.Models
{
    public record Toy(
        [property: JsonPropertyName("id")] int? Id,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("countrycode")] string Countrycode,
        [property: JsonPropertyName("publishdate")] DateTime Publishdate,
        [property: JsonPropertyName("supplied")] int? Supplied,
        [property: JsonPropertyName("reserved")] int? Reserved,
        [property: JsonPropertyName("imageid")] string Imageid,
        [property: JsonPropertyName("raritytype")] RarityTypes RarityType,
        [property: JsonPropertyName("authorid")] int? Authorid,
        [property: JsonPropertyName("likescount")] int? Likescount,
        [property: JsonPropertyName("owned")] int? Owned,
        [property: JsonPropertyName("available")] int? Available,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("slug")] string Slug,
        [property: JsonPropertyName("tags")] IReadOnlyList<string> Tags,
        [property: JsonPropertyName("internalSearchTags")] IReadOnlyList<string> InternalSearchTags,
        [property: JsonPropertyName("backgroundColor")] string BackgroundColor,
        [property: JsonPropertyName("fontColor")] string FontColor,
        [property: JsonPropertyName("visible")] bool Visible,
        [property: JsonPropertyName("model")] ToyModel Model,
        [property: JsonPropertyName("artist")] Artist Artist,
        [property: JsonPropertyName("collections")] IReadOnlyList<ToyCollection> Collections,
        [property: JsonPropertyName("rarity")] ToyRarity Rarity,
        [property: JsonPropertyName("stores")] IReadOnlyList<Store> Stores
    );
}

