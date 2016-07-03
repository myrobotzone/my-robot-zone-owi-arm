using System.Threading.Tasks;
namespace my_robot_zone_robot_server.MessageHandlers
{
    public class DefaultMessageHandler : IMessageHandler
    {
        readonly ILogger logger;

        public DefaultMessageHandler(ILogger logger)
        {
            this.logger = logger;
        }

        public Task<bool> StartAsync()
        {
            this.logger.Log("Default message handler started");
            return Task.FromResult(true);
        }

        public Task HandleMessageAsync(string message)
        {
            this.logger.Log("Received message {0}", message);
            return Task.FromResult(true);
        }

        public Task StopAsync()
        {
            this.logger.Log("Default message handler stopped");
            return Task.FromResult(true);

        }
    }
}
