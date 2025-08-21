using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.Models
{
	public class CreateClassViewModel
	{
		[Required]
		[StringLength(20)]
		[Display(Name = "Class code")]
		public string ClassCode { get; set; } = null!;

		[Required]
		[StringLength(100)]
		[Display(Name = "Class name")]
		public string ClassName { get; set; } = null!;

		[Required]
		[StringLength(100)]
		[Display(Name = "Department")]
		public string Department { get; set; } = null!;

		[Required]
		[StringLength(10)]
		[Display(Name = "Academic year")]
		public string AcademicYear { get; set; } = null!;

		[Display(Name = "Courses")]
		public List<int> CourseIDs { get; set; } = new();

		[Display(Name = "Main Teacher")]
		public int? TeacherID { get; set; }
	}
}
