namespace SIMSWebApp.Models
{
    public class DashboardViewModel
    {
        public string UserRole { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        
        // Admin Dashboard Properties
        public int TotalUsers { get; set; }
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalCourses { get; set; }
        public int TotalAdmins { get; set; }
        public int TotalClasses { get; set; }
        public int TotalEnrollments { get; set; }
        
        // Teacher Dashboard Properties
        public int AssignedCourses { get; set; }
        public int TotalStudentsInCourses { get; set; }
        public string Department { get; set; } = null!;
        public string Specialization { get; set; } = null!;
        
        // Student Dashboard Properties
        public int EnrolledCourses { get; set; }
        public decimal AverageGrade { get; set; }
        public string Class { get; set; } = null!;
        public string Major { get; set; } = null!;
        public DateTime EnrollmentDate { get; set; }
    }
}
