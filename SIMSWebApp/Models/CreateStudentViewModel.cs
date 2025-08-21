using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.Models
{
	public class CreateStudentViewModel
	{
		// Account info
		[Required]
		[StringLength(50)]
		[Display(Name = "Username")]
		public string Username { get; set; } = null!;

		[Required]
		[StringLength(255)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; } = null!;

		[Required]
		[StringLength(100)]
		[Display(Name = "Full name")]
		public string FullName { get; set; } = null!;

		[Required]
		[StringLength(100)]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; } = null!;

		[StringLength(15)]
		[Display(Name = "Phone number")]
		public string? PhoneNumber { get; set; }

		// Student info
		[Required]
		[StringLength(20)]
		[Display(Name = "Student code")]
		public string StudentCode { get; set; } = null!;

		[Required]
		[StringLength(20)]
		[Display(Name = "Class")]
		public string Class { get; set; } = null!;

		[Required]
		[StringLength(100)]
		[Display(Name = "Major")]
		public string Major { get; set; } = null!;

		[DataType(DataType.Date)]
		[Display(Name = "Enrollment date")]
		public DateTime EnrollmentDate { get; set; } = DateTime.Today;

		[Display(Name = "Teacher")]
		public int? TeacherID { get; set; }
	}
}
