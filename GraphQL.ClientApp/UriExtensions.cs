using System;

namespace GraphQL.ClientApp
{
    public static class UriExtensions
    {
        public static Uri ToWebSocketAddress(this Uri uri)
        {
            var newProtocol = uri.Scheme.ToLowerInvariant() switch
            {
                "http" => "ws",
                "https" => "wss",
                _ => throw new ArgumentOutOfRangeException(nameof(uri), $"Invalid protocol name {uri.Scheme}")
            };

            var uriString = uri.ToString();
            return new Uri($"{newProtocol}{uriString.Substring(uri.Scheme.Length)}");
        }
    }
}