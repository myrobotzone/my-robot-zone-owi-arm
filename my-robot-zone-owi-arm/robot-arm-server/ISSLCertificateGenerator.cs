using System;
namespace robot_arm_server
{
    interface ISSLCertificateGenerator
    {
        void Generate(string path, string password);
    }
}
