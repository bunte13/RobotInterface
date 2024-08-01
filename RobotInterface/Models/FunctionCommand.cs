namespace RobotInterface.Models
{
    public class FunctionCommand
    {
        public int Id { get; set; }
        public int FunctionId { get; set; }
        public int CommandId { get; set; }
        public Function? Function { get; set; }
        public Command? Command { get; set; }
    }
}
