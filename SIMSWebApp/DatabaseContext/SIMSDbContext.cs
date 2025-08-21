using Microsoft.EntityFrameworkCore;
using SIMSWebApp.DatabaseContext.Entities;

namespace SIMSWebApp.DatabaseContext
{
	public class SIMSDbContext : DbContext
	{
		public SIMSDbContext(DbContextOptions<SIMSDbContext> options) : base(options)
		{
		}

		// DbSets
		public DbSet<User> Users { get; set; }
		public DbSet<Teacher> Teachers { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<CourseAssignment> CourseAssignments { get; set; }
		public DbSet<Enrollment> Enrollments { get; set; }
		public DbSet<Grade> Grades { get; set; }
		public DbSet<ClassRoom> ClassRooms { get; set; }
		public DbSet<ClassCourse> ClassCourses { get; set; }
		public DbSet<Attendance> Attendances { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// User Configuration
			modelBuilder.Entity<User>(entity =>
			{
				entity.HasKey(e => e.UserID);
				entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
				entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
				entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
				entity.Property(e => e.PhoneNumber).HasMaxLength(15);
				entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
				// Unique constraints
				entity.HasIndex(e => e.Username).IsUnique();
				entity.HasIndex(e => e.Email).IsUnique();
				// Defaults
				entity.Property(e => e.IsActive).HasDefaultValue(true);
				entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
				entity.Property(e => e.Role).HasDefaultValue("Student");
			});

			// Teacher Configuration
			modelBuilder.Entity<Teacher>(entity =>
			{
				entity.HasKey(e => e.TeacherID);
				entity.Property(e => e.TeacherCode).IsRequired().HasMaxLength(20);
				entity.Property(e => e.Department).IsRequired().HasMaxLength(100);
				entity.Property(e => e.Specialization).HasMaxLength(100);
				entity.Property(e => e.AcademicDegree).HasMaxLength(50);
				entity.Property(e => e.HireDate).IsRequired();
				entity.HasIndex(e => e.TeacherCode).IsUnique();
				entity.HasIndex(e => e.UserID).IsUnique();
				entity.Property(e => e.IsActive).HasDefaultValue(true);
				entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
				entity.HasOne(e => e.User)
					.WithOne(e => e.Teacher)
					.HasForeignKey<Teacher>(e => e.UserID)
					.OnDelete(DeleteBehavior.Cascade);
			});

			// Student Configuration
			modelBuilder.Entity<Student>(entity =>
			{
				entity.HasKey(e => e.StudentID);
				entity.Property(e => e.StudentCode).IsRequired().HasMaxLength(20);
				entity.Property(e => e.Class).IsRequired().HasMaxLength(20);
				entity.Property(e => e.Major).IsRequired().HasMaxLength(100);
				entity.Property(e => e.EnrollmentDate).IsRequired();
				entity.HasIndex(e => e.StudentCode).IsUnique();
				entity.HasIndex(e => e.UserID).IsUnique();
				entity.Property(e => e.IsActive).HasDefaultValue(true);
				entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
				entity.HasOne(e => e.User)
					.WithOne(e => e.Student)
					.HasForeignKey<Student>(e => e.UserID)
					.OnDelete(DeleteBehavior.Cascade);
			});

			// Course Configuration
			modelBuilder.Entity<Course>(entity =>
			{
				entity.HasKey(e => e.CourseID);
				entity.Property(e => e.CourseCode).IsRequired().HasMaxLength(20);
				entity.Property(e => e.CourseName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.Credits).IsRequired();
				entity.Property(e => e.Duration).IsRequired().HasMaxLength(50);
				entity.Property(e => e.Description).HasMaxLength(500);
				entity.Property(e => e.Department).IsRequired().HasMaxLength(100);
				entity.Property(e => e.Semester).IsRequired().HasMaxLength(20);
				entity.Property(e => e.AcademicYear).IsRequired().HasMaxLength(10);
				entity.Property(e => e.MaxStudents).HasDefaultValue(50);
				entity.Property(e => e.Fee).HasDefaultValue(0);
				entity.HasIndex(e => e.CourseCode).IsUnique();
				entity.Property(e => e.IsActive).HasDefaultValue(true);
				entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
			});

			// CourseAssignment Configuration
			modelBuilder.Entity<CourseAssignment>(entity =>
			{
				entity.HasKey(e => e.AssignmentID);
				entity.Property(e => e.Notes).HasMaxLength(500);
				entity.Property(e => e.AssignmentDate).HasDefaultValueSql("GETDATE()");
				entity.Property(e => e.IsActive).HasDefaultValue(true);
				entity.HasIndex(e => new { e.CourseID, e.TeacherID }).IsUnique();
				entity.HasOne(e => e.Course)
					.WithMany(e => e.CourseAssignments)
					.HasForeignKey(e => e.CourseID)
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.Teacher)
					.WithMany(e => e.CourseAssignments)
					.HasForeignKey(e => e.TeacherID)
					.OnDelete(DeleteBehavior.Cascade);
			});

			// Enrollment Configuration
			modelBuilder.Entity<Enrollment>(entity =>
			{
				entity.HasKey(e => e.EnrollmentID);
				entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
				entity.Property(e => e.Notes).HasMaxLength(500);
				entity.Property(e => e.EnrollmentDate).HasDefaultValueSql("GETDATE()");
				entity.Property(e => e.IsActive).HasDefaultValue(true);
				entity.HasIndex(e => new { e.StudentID, e.CourseID }).IsUnique();
				entity.HasOne(e => e.Student)
					.WithMany(e => e.Enrollments)
					.HasForeignKey(e => e.StudentID)
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.Course)
					.WithMany(e => e.Enrollments)
					.HasForeignKey(e => e.CourseID)
					.OnDelete(DeleteBehavior.Cascade);
			});

			// Grade Configuration
			modelBuilder.Entity<Grade>(entity =>
			{
				entity.HasKey(e => e.GradeID);
				entity.Property(e => e.GradeType).IsRequired().HasMaxLength(20);
				entity.Property(e => e.Score).IsRequired();
				entity.Property(e => e.Weight).IsRequired().HasDefaultValue(1.00m);
				entity.Property(e => e.Comments).HasMaxLength(500);
				entity.Property(e => e.GradingDate).HasDefaultValueSql("GETDATE()");
				entity.Property(e => e.IsActive).HasDefaultValue(true);
				entity.HasOne(e => e.Student)
					.WithMany(e => e.Grades)
					.HasForeignKey(e => e.StudentID)
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.Course)
					.WithMany(e => e.Grades)
					.HasForeignKey(e => e.CourseID)
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.Teacher)
					.WithMany(e => e.Grades)
					.HasForeignKey(e => e.TeacherID)
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.Enrollment)
					.WithMany(e => e.Grades)
					.HasForeignKey(e => e.EnrollmentID)
					.OnDelete(DeleteBehavior.Cascade);
			});

			// ClassRoom Configuration
			modelBuilder.Entity<ClassRoom>(entity =>
			{
				entity.HasKey(e => e.ClassRoomID);
				entity.Property(e => e.ClassCode).IsRequired().HasMaxLength(20);
				entity.Property(e => e.ClassName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.Department).IsRequired().HasMaxLength(100);
				entity.Property(e => e.AcademicYear).IsRequired().HasMaxLength(10);
				entity.HasIndex(e => e.ClassCode).IsUnique();
				entity.Property(e => e.IsActive).HasDefaultValue(true);
				entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
				entity.HasOne(e => e.Teacher)
					.WithMany()
					.HasForeignKey(e => e.TeacherID)
					.OnDelete(DeleteBehavior.SetNull);
			});

			// ClassCourse Configuration
			modelBuilder.Entity<ClassCourse>(entity =>
			{
				entity.HasKey(e => e.ClassCourseID);
				entity.HasIndex(e => new { e.ClassRoomID, e.CourseID }).IsUnique();
				entity.Property(e => e.IsActive).HasDefaultValue(true);
				entity.Property(e => e.AssignedAt).HasDefaultValueSql("GETDATE()");
				entity.HasOne(e => e.ClassRoom)
					.WithMany(c => c.ClassCourses)
					.HasForeignKey(e => e.ClassRoomID)
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.Course)
					.WithMany()
					.HasForeignKey(e => e.CourseID)
					.OnDelete(DeleteBehavior.Cascade);
			});

			// Attendance Configuration
			modelBuilder.Entity<Attendance>(entity =>
			{
				entity.HasKey(e => e.AttendanceID);
				entity.Property(e => e.Status).IsRequired().HasMaxLength(10);
				entity.Property(e => e.Note).HasMaxLength(200);
				entity.Property(e => e.AttendanceDate).HasDefaultValueSql("GETDATE()");
				entity.Property(e => e.IsActive).HasDefaultValue(true);
				entity.HasIndex(e => new { e.StudentID, e.CourseID, e.AttendanceDate }).IsUnique();
				entity.HasOne(e => e.Student).WithMany().HasForeignKey(e => e.StudentID).OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.Course).WithMany().HasForeignKey(e => e.CourseID).OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.Teacher).WithMany().HasForeignKey(e => e.TeacherID).OnDelete(DeleteBehavior.Cascade);
			});
		}
	}
}
