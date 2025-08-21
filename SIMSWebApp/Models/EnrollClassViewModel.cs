using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.Models
{
	public class EnrollClassViewModel
	{
		[Required]
		[Display(Name = "Course")]
		public int CourseID { get; set; }

		[Required]
		[Display(Name = "Class")]
		public int ClassRoomID { get; set; }
	}
}
