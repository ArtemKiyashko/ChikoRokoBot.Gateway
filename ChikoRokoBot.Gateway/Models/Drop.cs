using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Azure;
using Azure.Data.Tables;

namespace ChikoRokoBot.Gateway.Models
{
    public record Drop(
        [property: JsonPropertyName("id")] int? Id,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("created")] DateTime Created,
        [property: JsonPropertyName("finish")] DateTime Finish,
        [property: JsonPropertyName("start")] DateTime Start,
        [property: JsonPropertyName("toyid")] int? Toyid,
        [property: JsonPropertyName("mechanic")] string Mechanic,
        [property: JsonPropertyName("code")] IReadOnlyList<string> Code,
        [property: JsonPropertyName("blindBoxId")] int? BlindBoxId,
        [property: JsonPropertyName("toy")] Toy Toy
    );
}

