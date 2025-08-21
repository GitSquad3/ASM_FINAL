using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.DatabaseContext.Entities
{
    public class Grade
    {
        public int GradeID { get; set; }
        
        [Required(ErrorMessage = "Student is required")]
        public int StudentID { get; set; }
        
        [Required(ErrorMessage = "Course is required")]
        public int CourseID { get; set; }
        
        [Required(ErrorMessage = "Teacher is required")]
        public int TeacherID { get; set; }
        
        [Required(ErrorMessage = "Enrollment is required")]
        public int EnrollmentID { get; set; }
        
        [Required(ErrorMessage = "Grade type is required")]
        [StringLength(20, ErrorMessage = "Grade type cannot exceed 20 characters")]
        public string GradeType { get; set; } = null!; // Assignment, Midterm, Final, Participation
        
        [Required(ErrorMessage = "Score is required")]
        [Range(0, 100, ErrorMessage = "Score must be between 0 and 100")]
        public decimal Score { get; set; }
        
        [Required(ErrorMessage = "Weight is required")]
        [Range(0, 1, ErrorMessage = "Weight must be between 0 and 1")]
        public decimal Weight { get; set; } = 1.00m;
        
        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters")]
        public string? Comments { get; set; }
        
        public DateTime GradingDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Student Student { get; set; } = null!;
        public virtual Course Course { get; set; } = null!;
        public virtual Teacher Teacher { get; set; } = null!;
        public virtual Enrollment Enrollment { get; set; } = null!;
    }
}
