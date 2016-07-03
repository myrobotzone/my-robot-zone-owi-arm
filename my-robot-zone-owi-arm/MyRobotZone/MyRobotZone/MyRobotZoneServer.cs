using my_robot_zone_robot_server;
using System;

namespace MyRobotZone
{
    public class MyRobotZoneServer
    {
        private readonly IWebSocketServer webSocketServer;
        private readonly IWebSocketServerConfigurationFactory webSocketConfigurationFactory;
        private readonly IMessageHandler messageHandler;

        public MyRobotZoneServer(IWebSocketServer webSocketServer, IWebSocketServerConfigurationFactory webSocketConfigurationFactory, IMessageHandler messageHandler)
        {
            this.webSocketServer = webSocketServer;
            this.webSocketConfigurationFactory = webSocketConfigurationFactory;
            this.messageHandler = messageHandler;
        }

        public async void StartAsync()
        {
            await this.messageHandler.StartAsync();
            var config = this.webSocketConfigurationFactory.CreateConfiguration();
            this.webSocketServer.Start(config);
            this.webSocketServer.MessageRecieved += WebSocketServer_MessageRecieved;
        }

        private void WebSocketServer_MessageRecieved(string message)
        {
            this.messageHandler.HandleMessageAsync(message);
        }

        public async void StopAsync()
        {
            await this.messageHandler.StopAsync();
            this.webSocketServer.MessageRecieved -= WebSocketServer_MessageRecieved;
            this.webSocketServer.Stop();
        }
    }
}
