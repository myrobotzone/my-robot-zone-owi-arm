using System.Threading.Tasks;

namespace my_robot_zone_robot_server.MessageHandlers
{
    public class DefaultMessageHandler : IMessageHandler
    {
        private readonly ILogger _logger;

        public DefaultMessageHandler(ILogger logger)
        {
            _logger = logger;
        }

        public Task<bool> StartAsync()
        {
            _logger.Log("Default message handler started");
            return Task.FromResult(true);
        }

        public Task HandleMessageAsync(MRZMessage message)
        {
            _logger.Log($"Received FeatureId = '{message.FeatureId}', Payload = '{message.Payload}'");
            return Task.FromResult(true);
        }

        public Task StopAsync()
        {
            _logger.Log("Default message handler stopped");
            return Task.FromResult(true);
        }
    }
}