using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Gateway.Models
{
    public record Artist(
        [property: JsonPropertyName("id")] int? Id,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("avatarid")] string Avatarid,
        [property: JsonPropertyName("signatureid")] string Signatureid,
        [property: JsonPropertyName("statistic")] ArtistStatistic Statistic,
        [property: JsonPropertyName("created")] DateTime Created,
        [property: JsonPropertyName("popularity")] int? Popularity,
        [property: JsonPropertyName("socials")] ArtistSocial Socials,
        [property: JsonPropertyName("toys")] IReadOnlyList<Toy> Toys
    );
}

