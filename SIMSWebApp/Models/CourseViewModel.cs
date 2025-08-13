using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.Models
{
    public class CourseViewModel
    {
        public int CourseID { get; set; }
        
        [Required(ErrorMessage = "Course name is required")]
        [Display(Name = "Course Name")]
        [StringLength(100, ErrorMessage = "Course name cannot exceed 100 characters")]
        public string CourseName { get; set; } = null!;
        
        [Required(ErrorMessage = "Course code is required")]
        [Display(Name = "Course Code")]
        [StringLength(20, ErrorMessage = "Course code cannot exceed 20 characters")]
        public string CourseCode { get; set; } = null!;
        
        [Required(ErrorMessage = "Credits are required")]
        [Display(Name = "Credits")]
        [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10")]
        public int Credits { get; set; } = 3;
        
        [Required(ErrorMessage = "Duration is required")]
        [Display(Name = "Duration")]
        [StringLength(50, ErrorMessage = "Duration cannot exceed 50 characters")]
        public string Duration { get; set; } = null!;
        
        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = null!;
        
        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        [StringLength(100, ErrorMessage = "Department cannot exceed 100 characters")]
        public string Department { get; set; } = null!;
        
        [Required(ErrorMessage = "Instructor is required")]
        [Display(Name = "Instructor")]
        [StringLength(100, ErrorMessage = "Instructor name cannot exceed 100 characters")]
        public string Instructor { get; set; } = null!;
        
        [Required(ErrorMessage = "Semester is required")]
        [Display(Name = "Semester")]
        [StringLength(20, ErrorMessage = "Semester cannot exceed 20 characters")]
        public string Semester { get; set; } = null!;
        
        [Required(ErrorMessage = "Academic year is required")]
        [Display(Name = "Academic Year")]
        [StringLength(10, ErrorMessage = "Academic year cannot exceed 10 characters")]
        public string AcademicYear { get; set; } = null!;
        
        [Display(Name = "Maximum Students")]
        [Range(0, 100, ErrorMessage = "Maximum students must be between 0 and 100")]
        public int? MaxStudents { get; set; }
        
        [Display(Name = "Course Fee")]
        [Range(0, 10000000, ErrorMessage = "Fee must be between 0 and 10,000,000")]
        public decimal? Fee { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
