namespace RobotInterface.Models
{
    public class Function
    {
        public int Id { get; set; }
        public string? FunctionName { get; set; }
        public int CategoryId { get; set; }
        public ICollection<FunctionCommand>? FunctionCommands { get; set; }
        public ICollection<FunctionLibrary>? FunctionLibraries { get; set; }
        public Category? Category { get; set; }

    }
}
