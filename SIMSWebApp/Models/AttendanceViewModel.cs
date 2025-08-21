using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.Models
{
	public class AttendanceViewModel
	{
		[Required]
		public int CourseID { get; set; }
		[Required]
		[DataType(DataType.Date)]
		public DateTime AttendanceDate { get; set; } = DateTime.Today;
		public List<StudentAttendanceItem> Students { get; set; } = new();
	}

	public class StudentAttendanceItem
	{
		public int StudentID { get; set; }
		public string StudentCode { get; set; } = null!;
		public string FullName { get; set; } = null!;
		public string Status { get; set; } = "Present"; // Present, Absent, Late, Excused
		public string? Note { get; set; }
	}
}
