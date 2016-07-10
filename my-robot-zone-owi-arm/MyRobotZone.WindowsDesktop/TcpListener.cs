using BaSocket;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MyRobotZone.WindowsDesktop
{
    internal class TcpServer : ITcpListener
    {
        public event Action<INetworkStream> ClientConnected;

        public async Task StartListening()
        {
            var tcpListener = new TcpListener(IPAddress.Loopback, 5002);
            tcpListener.Start();
            var client = await tcpListener.AcceptTcpClientAsync();
            this.ClientConnected?.Invoke(new NetworkStreamWrapper(client));
        }

        public Task StopAsync()
        {
            throw new NotImplementedException();
        }
    }
}
