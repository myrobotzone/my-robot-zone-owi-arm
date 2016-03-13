namespace robot_arm_server
{
    public interface ILogger
    {
        void Log(string format, params object[] args);
    }
}
