using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.Models
{
	public class EditClassViewModel
	{
		[Required]
		public int ClassRoomID { get; set; }
		[Required]
		[StringLength(20)]
		public string ClassCode { get; set; } = null!;
		[Required]
		[StringLength(100)]
		public string ClassName { get; set; } = null!;
		[Required]
		[StringLength(100)]
		public string Department { get; set; } = null!;
		[Required]
		[StringLength(10)]
		public string AcademicYear { get; set; } = null!;
		public List<int> CourseIDs { get; set; } = new();
		
		[Display(Name = "Main Teacher")]
		public int? TeacherID { get; set; }
	}
}
