using Microsoft.EntityFrameworkCore;
using SIMSWebApp.DatabaseContext;
using SIMSWebApp.DatabaseContext.Entities;
using SIMSWebApp.Interfaces;

namespace SIMSWebApp.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly SIMSDbContext _context;

        public CourseRepository(SIMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses
                .Where(c => c.IsActive)
                .OrderBy(c => c.CourseName)
                .ToListAsync();
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseID == id && c.IsActive);
        }

        public async Task<Course?> GetCourseByCodeAsync(string courseCode)
        {
            return await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseCode == courseCode && c.IsActive);
        }

        public async Task<Course> CreateCourseAsync(Course course)
        {
            course.CreatedAt = DateTime.Now;
            course.IsActive = true;
            
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course> UpdateCourseAsync(Course course)
        {
            var existingCourse = await _context.Courses.FindAsync(course.CourseID);
            if (existingCourse == null)
                throw new InvalidOperationException("Course not found");

            existingCourse.CourseName = course.CourseName;
            existingCourse.CourseCode = course.CourseCode;
            existingCourse.Credits = course.Credits;
            existingCourse.Duration = course.Duration;
            existingCourse.Description = course.Description;
            existingCourse.Department = course.Department;
            existingCourse.Instructor = course.Instructor;
            existingCourse.Semester = course.Semester;
            existingCourse.AcademicYear = course.AcademicYear;
            existingCourse.MaxStudents = course.MaxStudents;
            existingCourse.Fee = course.Fee;
            existingCourse.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingCourse;
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return false;

            course.IsActive = false;
            course.UpdatedAt = DateTime.Now;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CourseExistsAsync(int id)
        {
            return await _context.Courses
                .AnyAsync(c => c.CourseID == id && c.IsActive);
        }

        public async Task<bool> CourseCodeExistsAsync(string courseCode)
        {
            return await _context.Courses
                .AnyAsync(c => c.CourseCode == courseCode && c.IsActive);
        }

        public async Task<IEnumerable<Course>> GetCoursesByDepartmentAsync(string department)
        {
            return await _context.Courses
                .Where(c => c.Department == department && c.IsActive)
                .OrderBy(c => c.CourseName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesBySemesterAsync(string semester, string academicYear)
        {
            return await _context.Courses
                .Where(c => c.Semester == semester && c.AcademicYear == academicYear && c.IsActive)
                .OrderBy(c => c.CourseName)
                .ToListAsync();
        }
    }
}
