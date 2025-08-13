using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.DatabaseContext.Entities
{
    public class Student
    {
        public int StudentID { get; set; }
        
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        public string FullName { get; set; } = null!;
        
        [Required(ErrorMessage = "Student code is required")]
        [StringLength(20, ErrorMessage = "Student code cannot exceed 20 characters")]
        public string StudentCode { get; set; } = null!;
        
        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }
        
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = null!;
        
        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; } = null!;
        
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters")]
        public string PhoneNumber { get; set; } = null!;
        
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "Class is required")]
        [StringLength(20, ErrorMessage = "Class cannot exceed 20 characters")]
        public string Class { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
