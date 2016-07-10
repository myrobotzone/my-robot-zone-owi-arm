using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaSocket
{
    public class WebSocketServer
    {
        ITcpListener listener;
        IHandshakeResponseGenerator handshakeResponseGenerator;
        CancellationToken token;

        public WebSocketServer(ITcpListener listener, ISHA1 sha1)
        {
            this.listener = listener;
            this.handshakeResponseGenerator = new HandshakeResponseGenerator(sha1);
        }

        public void StartAsync(CancellationToken token)
        {
            this.token = token;
            listener.ClientConnected += Server_ClientConnected;
            listener.StartListening();
        }

        public Task StopAsync()
        {
            this.listener.ClientConnected -= Server_ClientConnected;
            return this.listener.StopAsync();
        }

        private async void Server_ClientConnected(INetworkStream stream)
        {
            string data = string.Empty;
            await Task.Factory.StartNew(async () =>
            {
                //enter to an infinite cycle to be able to handle every change in stream
                while (this.token.IsCancellationRequested == false)
                {
                    data += await stream.ReadAsync();
                    if (data.Contains("GET"))
                    {
                        var response = this.handshakeResponseGenerator.Generate(data);
                        await stream.WriteAsync(response);
                        var testMessage = Encoding.UTF8.GetBytes("hello");
                        var frame = this.GetPackageData(1, testMessage, 0, testMessage.Length);
                        await stream.WriteAsync(Encoding.UTF8.GetString(frame, 0, frame.Length));
                    }
                }
            });
        }

        private byte[] GetPackageData(int opCode, byte[] data, int offset, int length)
        {
            byte[] fragment;

            if (length < 126)
            {
                fragment = new byte[2 + length];
                fragment[1] = (byte)length;
            }
            else if (length < 65536)
            {
                fragment = new byte[4 + length];
                fragment[1] = (byte)126;
                fragment[2] = (byte)(length / 256);
                fragment[3] = (byte)(length % 256);
            }
            else
            {
                fragment = new byte[10 + length];
                fragment[1] = (byte)127;

                int left = length;
                int unit = 256;

                for (int i = 9; i > 1; i--)
                {
                    fragment[i] = (byte)(left % unit);
                    left = left / unit;

                    if (left == 0)
                        break;
                }
            }

            fragment[0] = (byte)(opCode | 0x80);

            if (length > 0)
            {
                Buffer.BlockCopy(data, offset, fragment, fragment.Length - length, length);
            }

            return fragment;
        }
    }
}
