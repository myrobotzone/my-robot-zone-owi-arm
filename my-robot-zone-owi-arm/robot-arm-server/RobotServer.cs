using SuperSocket.WebSocket;

namespace robot_arm_server
{
    public class RobotServer : IRobotServer
    {
        readonly WebSocketServer appServer = new WebSocketServer();
        readonly IMessageHandler messageHander;
        readonly ILogger logger;

        public RobotServer(IMessageHandler messageHandler, ILogger logger)
        {
            this.messageHander = messageHandler;
            this.logger = logger;
        }

        public bool Start()
        {
            if (!StartSocketServer())
            {
                return false;
            }

            if (!this.messageHander.Start())
            {
                this.appServer.Stop();
                return false;
            }

            return true;
        }

        public void Stop()
        {
            this.appServer.Stop();
            this.appServer.NewSessionConnected -= this.appServer_NewSessionConnected;
            this.appServer.NewMessageReceived -= this.appServer_NewRequestReceived;
            this.messageHander.Stop();
        }

        private bool StartSocketServer()
        {
            const int port = 5000;
            if (!this.appServer.Setup(port))
            {
                this.logger.Log("Failed to setup socket server to listen on port {0}", port);
                return false;
            }
            if (!this.appServer.Start())
            {
                this.logger.Log("Failed to start socket server");
                return false;
            }

            this.appServer.NewSessionConnected += this.appServer_NewSessionConnected;
            this.appServer.NewMessageReceived += this.appServer_NewRequestReceived;

            return true;
        }

        void appServer_NewSessionConnected(WebSocketSession session)
        {
            this.logger.Log("Session connected: {0}", session.RemoteEndPoint.Address);
        }

        void appServer_NewRequestReceived(WebSocketSession session, string message)
        {
            this.messageHander.HandleMessage(message);
        }
    }
}
