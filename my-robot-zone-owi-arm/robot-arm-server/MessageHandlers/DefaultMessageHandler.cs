namespace robot_arm_server.MessageHandlers
{
    public class DefaultMessageHandler : IMessageHandler
    {
        readonly ILogger logger;

        public DefaultMessageHandler(ILogger logger)
        {
            this.logger = logger;
        }

        public bool Start()
        {
            this.logger.Log("Default message handler started");
            return true;
        }

        public void HandleMessage(string message)
        {
            this.logger.Log("Received message {0}", message);
        }

        public void Stop()
        {
            this.logger.Log("Default message handler stopped");
        }
    }
}
