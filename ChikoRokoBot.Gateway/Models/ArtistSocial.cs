using System;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Gateway.Models
{
    public record ArtistSocial(
        [property: JsonPropertyName("vk")] string Vk,
        [property: JsonPropertyName("YouTube")] string YouTube,
        [property: JsonPropertyName("discord")] string Discord,
        [property: JsonPropertyName("spotify")] string Spotify,
        [property: JsonPropertyName("twitter")] string Twitter,
        [property: JsonPropertyName("facebook")] string Facebook,
        [property: JsonPropertyName("instagram")] string Instagram,
        [property: JsonPropertyName("Website")] string Website,
        [property: JsonPropertyName("Telegram")] string Telegram,
        [property: JsonPropertyName("Behance")] string Behance
    );
}

