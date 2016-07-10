using System;
using System.Threading.Tasks;

namespace BaSocket
{
    public interface ITcpListener
    {
        event Action<INetworkStream> ClientConnected;

        Task StartListening();

        Task StopAsync();
    }
}
