using System.Threading.Tasks;

namespace BaSocket
{
    public interface INetworkStream
    {
        Task<string> ReadAsync();

        Task WriteAsync(string message);
    }
}
