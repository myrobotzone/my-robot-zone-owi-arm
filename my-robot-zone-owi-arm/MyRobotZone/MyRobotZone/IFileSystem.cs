namespace my_robot_zone_robot_server
{
    public interface IFileSystem
    {
        bool FileExists(string file);

        void WriteAllBytes(string path, byte[] bytes);

        string GetTempPath();
    }
}
