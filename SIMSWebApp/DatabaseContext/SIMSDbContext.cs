using Microsoft.EntityFrameworkCore;
using SIMSWebApp.DatabaseContext.Entities;

namespace SIMSWebApp.DatabaseContext
{
    public class SIMSDbContext : DbContext
    {
        public SIMSDbContext(DbContextOptions<SIMSDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // dinh nghia Entity User la bang du lieu Users trong database
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey("UserID");
            modelBuilder.Entity<User>().HasIndex("Username").IsUnique();
            modelBuilder.Entity<User>().Property(u => u.Role).HasDefaultValue("Admin");
            
            // dinh nghia Entity Student la bang du lieu Students trong database
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Student>().HasKey("StudentID");
            modelBuilder.Entity<Student>().HasIndex("StudentCode").IsUnique();
            modelBuilder.Entity<Student>().Property(s => s.CreatedAt).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Student>().Property(s => s.IsActive).HasDefaultValue(true);
            
            // dinh nghia Entity Course la bang du lieu Courses trong database
            modelBuilder.Entity<Course>().ToTable("Courses");
            modelBuilder.Entity<Course>().HasKey("CourseID");
            modelBuilder.Entity<Course>().HasIndex("CourseCode").IsUnique();
            modelBuilder.Entity<Course>().Property(c => c.Credits).HasDefaultValue(3);
            modelBuilder.Entity<Course>().Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Course>().Property(c => c.IsActive).HasDefaultValue(true);
        }
    }
}
