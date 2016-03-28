using System.IO;

namespace my_robot_zone_robot_server
{
    class SystemUtils : ISystemUtils
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
