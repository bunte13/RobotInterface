namespace RobotInterface.Services
{
    public interface ISshService
    {
        void SetHost(string host);
        string ExecuteCommand(string combinedCommand, int timeoutSeconds = 300);
        string GetHost();
        string TestConnection();  
    }
}
