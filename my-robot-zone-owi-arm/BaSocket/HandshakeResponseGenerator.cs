using System;
using System.Text.RegularExpressions;

namespace BaSocket
{
    internal class HandshakeResponseGenerator : IHandshakeResponseGenerator
    {
        private static string ResponseFormat = "HTTP/1.1 101 Switching Protocols{0}Upgrade: websocket{0}Connection: Upgrade{0}Sec-WebSocket-Accept: {1}{0}{0}";

        ISHA1 sha1;

        public HandshakeResponseGenerator(ISHA1 sha1)
        {
            this.sha1 = sha1;
        }

        public string Generate(string request)
        {
            var rawKey = GetHashedKey(request);
            return string.Format(ResponseFormat, Environment.NewLine, rawKey);
        }

        private string GetHashedKey(string response)
        {
            //var parts = response.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            //var rawKey = parts[4].Split(new[] { ": " }, StringSplitOptions.None).Last();
            var rawKey = new Regex("Sec-WebSocket-Key: (.*)").Match(response).Groups[1].Value.Trim();
            var hashedKey = this.sha1.ComputeHash(rawKey + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11");
            return hashedKey;
        }
    }
}
