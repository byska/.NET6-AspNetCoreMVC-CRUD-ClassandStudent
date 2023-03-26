using System.ComponentModel.DataAnnotations;

namespace ClassandStudent.Models
{
    public class Class
    {
        [Key]
        public int ClassId { get; set; }
        public int Level { get; set; }
        public string Branch { get; set; }
        public List<Student> Students { get; set; }
        public Class()
        {
            Students= new List<Student>();
        }
    }
}
