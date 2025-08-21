using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.DatabaseContext.Entities
{
    public class Teacher
    {
        public int TeacherID { get; set; }
        
        [Required(ErrorMessage = "Teacher code is required")]
        [StringLength(20, ErrorMessage = "Teacher code cannot exceed 20 characters")]
        public string TeacherCode { get; set; } = null!;
        
        [Required(ErrorMessage = "User is required")]
        public int UserID { get; set; }
        
        [Required(ErrorMessage = "Department is required")]
        [StringLength(100, ErrorMessage = "Department cannot exceed 100 characters")]
        public string Department { get; set; } = null!;
        
        [StringLength(100, ErrorMessage = "Specialization cannot exceed 100 characters")]
        public string? Specialization { get; set; }
        
        [StringLength(50, ErrorMessage = "Academic degree cannot exceed 50 characters")]
        public string? AcademicDegree { get; set; }
        
        [Required(ErrorMessage = "Hire date is required")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();
        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
