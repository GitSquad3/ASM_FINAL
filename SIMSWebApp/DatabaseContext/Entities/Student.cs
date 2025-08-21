using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.DatabaseContext.Entities
{
    public class Student
    {
        public int StudentID { get; set; }
        
        [Required(ErrorMessage = "Student code is required")]
        [StringLength(20, ErrorMessage = "Student code cannot exceed 20 characters")]
        public string StudentCode { get; set; } = null!;
        
        [Required(ErrorMessage = "User is required")]
        public int UserID { get; set; }
        
        [Required(ErrorMessage = "Class is required")]
        [StringLength(20, ErrorMessage = "Class cannot exceed 20 characters")]
        public string Class { get; set; } = null!;
        
        [Required(ErrorMessage = "Major is required")]
        [StringLength(100, ErrorMessage = "Major cannot exceed 100 characters")]
        public string Major { get; set; } = null!;
        
        [Required(ErrorMessage = "Enrollment date is required")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }
        
        public int? TeacherID { get; set; }
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Teacher? Teacher { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
