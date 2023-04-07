using System;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Gateway.Models
{
    public record Props(
        [property: JsonPropertyName("pageProps")] PageProps PageProps,
        [property: JsonPropertyName("__N_SSP")] bool NSSP
    );
}

