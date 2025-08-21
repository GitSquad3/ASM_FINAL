using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.DatabaseContext.Entities
{
	public class ClassRoom
	{
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

		public int? TeacherID { get; set; }

		public bool IsActive { get; set; } = true;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime? UpdatedAt { get; set; }

		// Navigation
		public virtual ICollection<ClassCourse> ClassCourses { get; set; } = new List<ClassCourse>();
		public virtual Teacher? Teacher { get; set; }
	}
}
