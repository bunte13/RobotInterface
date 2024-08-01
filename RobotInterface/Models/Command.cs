namespace RobotInterface.Models
{
    public class Command
    {
        public int Id { get; set; }
        public string? CommandName { get; set; }
        public ICollection<FunctionCommand>? FunctionCommands { get; set;}
    }
}
