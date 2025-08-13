using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.Models
{
    public class StudentViewModel
    {
        public int StudentID { get; set; }
        
        [Required(ErrorMessage = "Full name is required")]
        [Display(Name = "Full Name")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        public string FullName { get; set; } = null!;
        
        [Required(ErrorMessage = "Student code is required")]
        [Display(Name = "Student Code")]
        [StringLength(20, ErrorMessage = "Student code cannot exceed 20 characters")]
        public string StudentCode { get; set; } = null!;
        
        [Required(ErrorMessage = "Date of birth is required")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; } = DateTime.Now.AddYears(-18);
        
        [Required(ErrorMessage = "Gender is required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; } = null!;
        
        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; } = null!;
        
        [Required(ErrorMessage = "Phone number is required")]
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters")]
        public string PhoneNumber { get; set; } = null!;
        
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "Class is required")]
        [Display(Name = "Class")]
        [StringLength(20, ErrorMessage = "Class cannot exceed 20 characters")]
        public string Class { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
