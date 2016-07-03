using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using my_robot_zone_robot_server;
using MyRobotZone;
using System;

namespace MyRobotZoneTest
{
    [TestClass]
    public class MyRobotZoneServerTest
    {
        [TestMethod]
        public void StartMyRobotZoneServer_StartsWebsocketServerWithConfigAndStartsMessageHandler()
        {
            var mocker = new AutoMocker();

            var config = mocker.GetMock<IWebSocketServerConfiguration>().Object;
            var configFactory = mocker.GetMock<IWebSocketServerConfigurationFactory>();
            configFactory.Setup(mock => mock.CreateConfiguration()).Returns(config);

            var mrzServer = mocker.CreateInstance<MyRobotZoneServer>();

            mrzServer.StartAsync();

            mocker.GetMock<IMessageHandler>().Verify(mock => mock.StartAsync(), Times.Once);
            mocker.GetMock<IWebSocketServer>().Verify(mock => mock.Start(config), Times.Once);
        }

        [TestMethod]
        public void MyRobotZoneServerIsStartedAndMessageIsRecieved_HandlerIsCalled()
        {
            var mocker = new AutoMocker();

            var message = "message";
            var mrzServer = mocker.CreateInstance<MyRobotZoneServer>();
            mrzServer.StartAsync();

            mocker.GetMock<IWebSocketServer>().Raise(mock => mock.MessageRecieved += null, message);

            mocker.GetMock<IMessageHandler>().Verify(mock => mock.HandleMessageAsync(message));
        }

        [TestMethod]
        public void MyRobotZoneServerIsStartedThenStopedAndMessageIsRecieved_HandlerIsNotCalled()
        {
            var mocker = new AutoMocker();

            var message = "message";
            var mrzServer = mocker.CreateInstance<MyRobotZoneServer>();
            mrzServer.StartAsync();
            mrzServer.StopAsync();

            mocker.GetMock<IWebSocketServer>().Raise(mock => mock.MessageRecieved += null, message);

            mocker.GetMock<IMessageHandler>().Verify(mock => mock.HandleMessageAsync(message), Times.Never);
        }

        [TestMethod]
        public void StopMyRobotZoneServer_StopsMessageHandlerAndWebSocketServer()
        {
            var mocker = new AutoMocker();

            var mrzServer = mocker.CreateInstance<MyRobotZoneServer>();
            mrzServer.StopAsync();

            mocker.GetMock<IMessageHandler>().Verify(mock => mock.StopAsync(), Times.Once);
            mocker.GetMock<IWebSocketServer>().Verify(mock => mock.Stop(), Times.Once);
        }
    }
}
