using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyRobotZone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRobotZoneTest
{
    [TestClass]
    public class WebSocketServerConfigurationTest
    {
        [TestMethod]
        public void ConfigurationHasExpectedValues()
        {
            var password = "password";
            var filePath = "filePath";

            IWebSocketServerConfiguration config = new WebSocketServerConfiguration(password, filePath);
            Assert.AreEqual(5000, config.Port);
            Assert.AreEqual(WebSocketSecurity.tls, config.Security);
            Assert.AreEqual(password, config.Password);
            Assert.AreEqual(filePath, config.FilePath);
        }
    }
}
