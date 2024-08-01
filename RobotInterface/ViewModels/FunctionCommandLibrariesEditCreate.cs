using Microsoft.AspNetCore.Mvc.Rendering;
using RobotInterface.Models;


namespace RobotInterface.ViewModels
{
    public class FunctionCommandLibrariesEditCreate
    {
        public Function? Function { get; set; }
        public IEnumerable<int>? SelectedCommands { get; set; }
        public IEnumerable<SelectListItem>? CommandsList { get; set; }
        public IEnumerable<int>? SelectedLibraries { get; set; }
        public IEnumerable<SelectListItem>? LibrariesList { get; set; }
        public SelectList? CategoryList { get; set; }
        public int SelectedCategoryId { get; set; }
    }
}
