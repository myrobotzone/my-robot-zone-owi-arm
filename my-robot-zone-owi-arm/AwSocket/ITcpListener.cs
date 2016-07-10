using System;
using System.Threading.Tasks;

namespace AwSocket
{
    public interface ITcpListener
    {
        event Action<INetworkStream> ClientConnected;

        Task StartListening();

        Task StopAsync();
    }
}
