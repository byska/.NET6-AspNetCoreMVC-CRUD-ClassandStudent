using Microsoft.EntityFrameworkCore;

namespace ClassandStudent.Models
{
    public class StudentClassContext:DbContext
    {
       public DbSet<Student> studentsEntity { get; set; }
       public DbSet<Class> classesEntity { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source= DESKTOP-Q56AEMU\\MSSQLKD14;Initial Catalog=ClassandStudents;user id=sa;Password=Beste1998.");
          
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasOne(x=>x.StudentClass).WithMany(x=>x.Students).HasForeignKey(x=>x.ClassId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
