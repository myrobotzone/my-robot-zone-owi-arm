using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyRobotZone;
using Moq.AutoMock;
using my_robot_zone_robot_server;
using Moq;
using System.IO;

namespace MyRobotZoneTest
{
    [TestClass]
    public class IWebSocketServerConfigurationFactoryTest
    {
        [TestMethod]
        public void FactoryCreatesDefaultConfigurationAndGenerateCertificate()
        {
            var mocker = new AutoMocker();

            string path = "filePath";

            mocker.GetMock<IFileSystem>().Setup(mock => mock.GetTempPath()).Returns(path);
            mocker.GetMock<ISSLCertificateGenerator>().Setup(mock => mock.Generate(It.IsAny<string>())).Returns(new MemoryStream());

            var factory = mocker.CreateInstance<WebSocketServerConfigurationFactory>();
            var config = factory.CreateConfiguration();

            Assert.IsInstanceOfType(config, typeof(WebSocketServerConfiguration));
            Assert.AreEqual(path, config.FilePath);

            mocker.GetMock<ISSLCertificateGenerator>().Verify(mock => mock.Generate(config.Password));
        }

        [TestMethod]
        public void SSLFileExists_SSLFileIsNoGenerated()
        {
            var mocker = new AutoMocker();
            mocker.GetMock<IFileSystem>().Setup(mock => mock.FileExists(It.IsAny<string>())).Returns(true);

            var factory = mocker.CreateInstance<WebSocketServerConfigurationFactory>();
            factory.CreateConfiguration();

            mocker.GetMock<ISSLCertificateGenerator>().Verify(mock => mock.Generate(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void SSLFileIsNew_SSLFileIsGeneratedAndSaved()
        {
            var mocker = new AutoMocker();

            var stream = new MemoryStream();

            mocker.GetMock<IFileSystem>().Setup(mock => mock.FileExists(It.IsAny<string>())).Returns(false);
            mocker.GetMock<ISSLCertificateGenerator>().Setup(mock => mock.Generate(It.IsAny<string>())).Returns(stream);

            var factory = mocker.CreateInstance<WebSocketServerConfigurationFactory>();
            var config = factory.CreateConfiguration();

            mocker.GetMock<ISSLCertificateGenerator>().Verify(mock => mock.Generate(config.Password), Times.Once);
            mocker.GetMock<IFileSystem>().Verify(mock => mock.WriteAllBytes(config.FilePath, It.IsAny<byte[]>()), Times.Once);
        }
    }
}
