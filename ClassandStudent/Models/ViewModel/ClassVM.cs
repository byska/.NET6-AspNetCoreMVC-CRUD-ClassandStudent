using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClassandStudent.Models.ViewModel
{
    public class ClassVM
    {
        public List<ClassDto> ClassList { get; set; }
        public Class classes { get; set; }
        public List<SelectListItem> LevelClassesForDropDown { get; set; }
        public List<SelectListItem> BranchClassesForDropDown { get; set; }

    }
}
