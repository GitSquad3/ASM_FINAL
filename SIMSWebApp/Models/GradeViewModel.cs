using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.Models
{
    public class GradeViewModel
    {
        public int GradeID { get; set; }
        
        [Required(ErrorMessage = "Student is required")]
        public int StudentId { get; set; }
        
        [Required(ErrorMessage = "Course is required")]
        public int CourseId { get; set; }
        
        [Required(ErrorMessage = "Teacher is required")]
        public int TeacherId { get; set; }
        
        public int? EnrollmentId { get; set; }
        
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
        
        [Required(ErrorMessage = "Grading date is required")]
        [DataType(DataType.Date)]
        public DateTime GradingDate { get; set; } = DateTime.Now;
    }
}