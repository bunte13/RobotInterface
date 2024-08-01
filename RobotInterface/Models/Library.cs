namespace RobotInterface.Models
{
    public class Library
    {
        public int Id { get; set; }
        public string? LibraryName { get; set; }
        public string? Description { get; set; }
        public ICollection<FunctionLibrary>? FunctionLibraries { get; set;}
    }
}
