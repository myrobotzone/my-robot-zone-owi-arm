using System.IO;

namespace robot_arm_server
{
    class SystemInterface : ISystemInterface
    {
        public bool FileExists(string file)
        {
            return File.Exists(file);
        }


        public string GetTempPath()
        {
            return Path.GetTempPath();
        }
    }
}
