using SIMSWebApp.DatabaseContext.Entities;

namespace SIMSWebApp.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course?> GetCourseByIdAsync(int id);
        Task<Course?> GetCourseByCodeAsync(string courseCode);
        Task<Course> CreateCourseAsync(Course course);
        Task<Course> UpdateCourseAsync(Course course);
        Task<bool> DeleteCourseAsync(int id);
        Task<bool> CourseExistsAsync(int id);
        Task<bool> CourseCodeExistsAsync(string courseCode);
        Task<IEnumerable<Course>> GetCoursesByDepartmentAsync(string department);
        Task<IEnumerable<Course>> GetCoursesBySemesterAsync(string semester, string academicYear);
    }
}
