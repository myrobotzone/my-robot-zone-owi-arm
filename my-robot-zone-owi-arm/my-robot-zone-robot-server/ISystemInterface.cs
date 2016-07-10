namespace my_robot_zone_robot_server
{
    internal interface ISystemUtils
    {
        bool FileExists(string file);

        string GetTempPath();
    }
}
