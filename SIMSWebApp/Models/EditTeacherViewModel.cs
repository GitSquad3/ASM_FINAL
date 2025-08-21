using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.Models
{
    public class EditTeacherViewModel
    {
        public int TeacherID { get; set; }
        
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        public string Username { get; set; } = null!;
        
        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password (leave empty to keep current)")]
        public string? Password { get; set; }
        
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = null!;
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; } = null!;
        
        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = null!;
        
        [Required(ErrorMessage = "Teacher code is required")]
        [StringLength(20, ErrorMessage = "Teacher code cannot exceed 20 characters")]
        [Display(Name = "Teacher Code")]
        public string TeacherCode { get; set; } = null!;
        
        [Required(ErrorMessage = "Department is required")]
        [StringLength(100, ErrorMessage = "Department cannot exceed 100 characters")]
        public string Department { get; set; } = null!;
        
        [Required(ErrorMessage = "Specialization is required")]
        [StringLength(100, ErrorMessage = "Specialization cannot exceed 100 characters")]
        public string Specialization { get; set; } = null!;
        
        [Required(ErrorMessage = "Academic degree is required")]
        [StringLength(50, ErrorMessage = "Academic degree cannot exceed 50 characters")]
        [Display(Name = "Academic Degree")]
        public string AcademicDegree { get; set; } = null!;
        
        [Required(ErrorMessage = "Hire date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }
    }
}