using BaSocket;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace MyRobotZone.UWP
{
    class SHA1 : ISHA1
    {
        public string ComputeHash(string text)
        {
            // https://blogs.msdn.microsoft.com/ramg/2013/07/10/how-to-generate-hash-in-windows-store-apps/
            string base64Encoded = string.Empty;
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(text, BinaryStringEncoding.Utf8);
            HashAlgorithmProvider provider = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
            IBuffer hash = provider.HashData(buffer);
            base64Encoded = CryptographicBuffer.EncodeToBase64String(hash);
            return base64Encoded;
        }
    }
}
