using BaSocket;
using System;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace MyRobotZone.UWP
{
    class TcpSocketListener : ITcpListener
    {
        StreamSocketListener streamSocketListener;

        public event Action<INetworkStream> ClientConnected;

        public async Task StartListening()
        {
            this.streamSocketListener = new StreamSocketListener();
            this.streamSocketListener.ConnectionReceived += Listener_ConnectionReceived;
            await streamSocketListener.BindEndpointAsync(new Windows.Networking.HostName("localhost"), "5002");
            return;
        }

        public Task StopAsync()
        {
            this.streamSocketListener.ConnectionReceived -= Listener_ConnectionReceived;
            this.streamSocketListener.Dispose();
            return Task.CompletedTask;
        }

        private void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            var argsCopy = args;
            var stream = new NetworkStream(argsCopy);
            this.ClientConnected?.Invoke(stream);
        }
    }
}
