namespace RobotInterface.Models
{
    public class FunctionLibrary
    {
        public int Id { get; set; }
        public int FunctionId { get; set; }
        public int LibraryId { get; set; }
        public Function? Function { get; set; }
        public Library? Library { get; set; }
    }
}
