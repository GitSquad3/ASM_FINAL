using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.DatabaseContext.Entities
{
    public class CourseAssignment
    {
        public int AssignmentID { get; set; }
        
        [Required(ErrorMessage = "Course is required")]
        public int CourseID { get; set; }
        
        [Required(ErrorMessage = "Teacher is required")]
        public int TeacherID { get; set; }
        
        public DateTime AssignmentDate { get; set; } = DateTime.Now;
        
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }
        
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Course Course { get; set; } = null!;
        public virtual Teacher Teacher { get; set; } = null!;
    }
}
