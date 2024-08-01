using System.Threading.Tasks;

namespace RobotInterface.Services
{
    public interface IRobotService
    {
        string ExecuteCommand(int functionId);
    }
}
