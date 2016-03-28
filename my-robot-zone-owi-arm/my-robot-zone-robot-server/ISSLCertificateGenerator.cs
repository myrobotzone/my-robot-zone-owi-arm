using System;
namespace my_robot_zone_robot_server
{
    interface ISSLCertificateGenerator
    {
        void Generate(string path, string password);
    }
}
