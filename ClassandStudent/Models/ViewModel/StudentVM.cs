using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClassandStudent.Models.ViewModel
{
    public class StudentVM
    {
        public List<StudentDto> Students { get; set; }
        public Student student { get; set; }
        public List<SelectListItem> ClassLevelBranchForDropDown { get; set; }
        public List<SelectListItem> ClasBranchForDropDown { get; set; }

    }
}
