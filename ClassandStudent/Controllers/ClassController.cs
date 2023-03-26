using ClassandStudent.Models;
using ClassandStudent.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.CodeAnalysis;

namespace ClassandStudent.Controllers
{
    public class ClassController : Controller
    {
        StudentClassContext db;
        ClassVM classVM = new ClassVM();

        public ClassController(StudentClassContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetClass(string id)
        {
            classVM.BranchClassesForDropDown = FillClassBranch();
            if (!string.IsNullOrEmpty(id))
            {
                classVM.ClassList = db.classesEntity.Where(x=>x.Branch==id).Select(x => new ClassDto
                {
                    Id = x.ClassId,
                    Level = x.Level,
                    Branch = x.Branch,
                    TotalStudentCount = x.Students.Count,
                }).ToList();
                return View(classVM);
            }
            classVM.ClassList = db.classesEntity.Select(x => new ClassDto
            {
                Id = x.ClassId,
                Level = x.Level,
                Branch = x.Branch,
                TotalStudentCount = x.Students.Count,
            }).ToList();
            return View(classVM);
        }
        public IActionResult Detail(int id)
        {
            ClassDto classDto = db.classesEntity.Select(x => new ClassDto()
            {
                Id = x.ClassId,
                Level = x.Level,
                Branch = x.Branch,
                TotalStudentCount = x.Students.Count,
            }).FirstOrDefault(x=>x.Id==id);
            return View(classDto);
        }
        public IActionResult Update(int id)
        {
            classVM.LevelClassesForDropDown = FillClassLevel();
            classVM.BranchClassesForDropDown = FillClassBranch();
            classVM.classes = db.classesEntity.Find(id);
            return View(classVM);
        }
        private List<SelectListItem> FillClassLevel()
        {
            List<SelectListItem> classLevelList = db.classesEntity.Select(x => new SelectListItem()
            {
                Text = x.Level.ToString(),
                Value = x.ClassId.ToString()
            }).ToList();
            List<SelectListItem> uniqListLevel = classLevelList.Distinct(new SelectListItemComparer()).ToList();
            return uniqListLevel;
        }
   
        private List<SelectListItem> FillClassBranch()
        {
            List<SelectListItem> classBranchList = db.classesEntity.Select(x => new SelectListItem()
            {
                Text = x.Branch.ToString(),
                Value = x.ClassId.ToString()
            }).ToList();
            List<SelectListItem> uniqListBranch = classBranchList.Distinct(new SelectListItemComparer()).ToList();
            return uniqListBranch;
        }
        public IActionResult Delete(int id)
        {
            db.classesEntity.Remove(db.classesEntity.Find(id));
            db.SaveChanges();
            return RedirectToAction("GetClass");
        }
        [HttpPost]
        public IActionResult Update(int id, ClassVM classVM)
        {
            Class _classes = db.classesEntity.Find(id);
            _classes.Level = classVM.classes.Level;
            _classes.Branch = classVM.classes.Branch;
            db.SaveChanges();
            return RedirectToAction("GetClass");
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
        [HttpPost]
        public IActionResult Create(ClassVM classVM)
        {
            if (db.classesEntity.Any(x => x.Level == classVM.classes.Level && x.Branch == classVM.classes.Branch))
            {
                return RedirectToAction("GetClass");
            }
            db.classesEntity.Add(classVM.classes);
            db.SaveChanges();
            return RedirectToAction("GetClass");

        }
        public IActionResult Create()
        {
            classVM.BranchClassesForDropDown = FillClassBranch();
            classVM.LevelClassesForDropDown = FillClassLevel();
            return View(classVM);
        }
    }
}
