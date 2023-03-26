using System.ComponentModel.DataAnnotations;

namespace ClassandStudent.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Class StudentClass { get; set; }
        [Required]
        public int ClassId { get; set; }

    }
}
