using BaSocket;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyRobotZone.WindowsDesktop
{
    internal class NetworkStreamWrapper : INetworkStream
    {
        TcpClient client;

        public NetworkStreamWrapper(TcpClient client)
        {
            this.client = client;
        }

        public async Task<string> ReadAsync()
        {
            var stream = this.client.GetStream();
            while (true)
            {
                if (stream.DataAvailable)
                {
                    var numBytes = this.client.Available;
                    var data = new byte[numBytes];
                    await stream.ReadAsync(data, 0, numBytes);
                    string message = Encoding.UTF8.GetString(data);
                    return message;
                }
            }
        }

        public Task WriteAsync(string message)
        {
            var stream = client.GetStream();
            var buffer = Encoding.UTF8.GetBytes(message);
            return stream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}
