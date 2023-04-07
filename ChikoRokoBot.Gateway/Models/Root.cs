using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Gateway.Models
{
    public record Root(
        [property: JsonPropertyName("props")] Props Props,
        [property: JsonPropertyName("page")] string Page,
        [property: JsonPropertyName("query")] Query Query,
        [property: JsonPropertyName("buildId")] string BuildId,
        [property: JsonPropertyName("isFallback")] bool IsFallback,
        [property: JsonPropertyName("gssp")] bool Gssp,
        [property: JsonPropertyName("locale")] string Locale,
        [property: JsonPropertyName("locales")] IReadOnlyList<string> Locales,
        [property: JsonPropertyName("defaultLocale")] string DefaultLocale,
        [property: JsonPropertyName("scriptLoader")] IReadOnlyList<object> ScriptLoader
    );
}

