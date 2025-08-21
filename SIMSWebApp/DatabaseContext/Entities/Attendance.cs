using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.DatabaseContext.Entities
{
	public class Attendance
	{
		public int AttendanceID { get; set; }
		[Required]
		public int StudentID { get; set; }
		[Required]
		public int CourseID { get; set; }
		[Required]
		public int TeacherID { get; set; }
		[Required]
		[DataType(DataType.Date)]
		public DateTime AttendanceDate { get; set; } = DateTime.Today;
		[Required]
		[StringLength(10)]
		public string Status { get; set; } = "Present"; // Present, Absent, Late, Excused
		[StringLength(200)]
		public string? Note { get; set; }
		public bool IsActive { get; set; } = true;

		public virtual Student Student { get; set; } = null!;
		public virtual Course Course { get; set; } = null!;
		public virtual Teacher Teacher { get; set; } = null!;
	}
}
