using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.DatabaseContext.Entities
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        
        [Required(ErrorMessage = "Student is required")]
        public int StudentID { get; set; }
        
        [Required(ErrorMessage = "Course is required")]
        public int CourseID { get; set; }
        
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
        
        [Required(ErrorMessage = "Status is required")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = "Active"; // Active, Completed, Dropped
        
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }
        
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Student Student { get; set; } = null!;
        public virtual Course Course { get; set; } = null!;
        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
