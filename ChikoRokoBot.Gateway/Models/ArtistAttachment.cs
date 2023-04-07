using System;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Gateway.Models
{
    public record ArtistAttachment(
        [property: JsonPropertyName("id")] int? Id,
        [property: JsonPropertyName("artistId")] int? ArtistId,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("attachmentId")] string AttachmentId
    );
}

