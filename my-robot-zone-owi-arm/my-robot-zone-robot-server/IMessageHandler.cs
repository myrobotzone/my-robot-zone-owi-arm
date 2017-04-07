using System.Threading.Tasks;

namespace my_robot_zone_robot_server
{
    public interface IMessageHandler
    {
        Task<bool> StartAsync();

        Task HandleMessageAsync(MRZMessage message);

        Task StopAsync();
    }
}
