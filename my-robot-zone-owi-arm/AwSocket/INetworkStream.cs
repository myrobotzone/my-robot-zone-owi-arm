using System.Threading.Tasks;

namespace AwSocket
{
    public interface INetworkStream
    {
        Task<string> ReadAsync();

        Task WriteAsync(string message);
    }
}
