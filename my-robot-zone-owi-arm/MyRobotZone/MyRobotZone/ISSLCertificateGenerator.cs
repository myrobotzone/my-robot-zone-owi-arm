using System;
using System.IO;

namespace my_robot_zone_robot_server
{
    public interface ISSLCertificateGenerator
    {
        MemoryStream Generate(string password);
    }
}
