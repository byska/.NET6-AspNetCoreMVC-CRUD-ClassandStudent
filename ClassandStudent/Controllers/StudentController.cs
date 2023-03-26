using ClassandStudent.Models;
using ClassandStudent.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static ClassandStudent.Controllers.ClassController;

namespace ClassandStudent.Controllers
{
    public class StudentController : Controller
    {
        StudentClassContext db;
        StudentVM studentVM = new StudentVM();
        ClassVM classVM = new ClassVM();
        public StudentController(StudentClassContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetStudent(string id)
        {
            studentVM.ClasBranchForDropDown = FillClassBranch();
            if (!string.IsNullOrEmpty(id))
            {
                studentVM.Students = db.studentsEntity.Where(x=>x.StudentClass.Branch==id).Select(x => new StudentDto()
                {
                    id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Address = x.Address,
                    Level = x.StudentClass.Level,
                    Branch = x.StudentClass.Branch,
                }).ToList();
                return View(studentVM);
            }
            else
            {
                studentVM.Students = db.studentsEntity.Select(x => new StudentDto()
                {
                    id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Address = x.Address,
                    Level = x.StudentClass.Level,
                    Branch = x.StudentClass.Branch,
                }).ToList();
                return View(studentVM);
            }
        }
        public IActionResult Detail(int id)
        {
            studentVM.student = db.studentsEntity.Include(x=>x.StudentClass).FirstOrDefault(x=>x.Id==id);
            return View(studentVM);
        }
        [HttpPost]
        public IActionResult Create(StudentVM studentVm)
        {
            if (db.studentsEntity.Any(x => x.FirstName == studentVm.student.FirstName && x.LastName == studentVm.student.LastName))
            {
                TempData["result"] = "This student is already registered.";
                return RedirectToAction("GetStudent");
            }
            else
            {
                db.studentsEntity.Add(studentVm.student);
                db.SaveChanges();
                return RedirectToAction("GetStudent");
            }
        }
        public IActionResult Create()
        {
            studentVM.ClassLevelBranchForDropDown = FillClassLevelBranch();
            return View(studentVM);
        }
        public IActionResult Delete(int id)
        {
            db.studentsEntity.Remove(db.studentsEntity.Find(id));
            db.SaveChanges();
            return RedirectToAction("GetStudent");
        }
        public IActionResult Update(int id)
        {
            studentVM.ClassLevelBranchForDropDown = FillClassLevelBranch();
            studentVM.student = db.studentsEntity.FirstOrDefault(x => x.Id == id);
            return View(studentVM);
        }
        [HttpPost]
        public IActionResult Update(StudentVM studentVM, int id)
        {
            Student student = db.studentsEntity.Find(id);
            student.FirstName = studentVM.student.FirstName;
            student.LastName = studentVM.student.LastName;
            student.Address = studentVM.student.Address;
            student.StudentClass.Level = studentVM.student.StudentClass.Level;
            student.StudentClass.Branch = studentVM.student.StudentClass.Branch;
            db.SaveChanges();
            return RedirectToAction("GetStudent");
        }

        private List<SelectListItem> FillClassLevelBranch()
        {
            List<SelectListItem> classLevelList = db.classesEntity.Select(x => new SelectListItem()
            {
                Text = x.Level.ToString()+"-"+x.Branch.ToString(),
                Value = x.ClassId.ToString()
            }).ToList();
            return classLevelList;
        }

        private List<SelectListItem> FillClassBranch()
        {
            List<SelectListItem> classBranchList = db.classesEntity.Select(x => new SelectListItem()
            {
                Text = x.Branch,
                Value = x.ClassId.ToString()
            }).OrderBy(x=>x.Text).ToList();
            List<SelectListItem> uniqListBranch = classBranchList.Distinct(new SelectListItemComparer()).ToList();
            return uniqListBranch;
        }
        public class SelectListItemComparer : IEqualityComparer<SelectListItem>
        {
            public bool Equals(SelectListItem x, SelectListItem y)
            {
                return x.Text == y.Text;
            }

            public int GetHashCode(SelectListItem obj)
            {
                return obj.Text.GetHashCode();
            }
        }
    }
}
