using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.DatabaseContext.Entities
{
	public class ClassCourse
	{
		public int ClassCourseID { get; set; }
		[Required]
		public int ClassRoomID { get; set; }
		[Required]
		public int CourseID { get; set; }
		public DateTime AssignedAt { get; set; } = DateTime.Now;
		public bool IsActive { get; set; } = true;

		public virtual ClassRoom ClassRoom { get; set; } = null!;
		public virtual Course Course { get; set; } = null!;
	}
}
