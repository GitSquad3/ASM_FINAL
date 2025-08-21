using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.Models
{
	public class EnrollStudentViewModel
	{
		[Required]
		[Display(Name = "Course")]
		public int CourseID { get; set; }

		[Required]
		[Display(Name = "Student")]
		public int StudentID { get; set; }
	}
}
