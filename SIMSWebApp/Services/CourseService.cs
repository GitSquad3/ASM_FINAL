using SIMSWebApp.DatabaseContext.Entities;
using SIMSWebApp.Interfaces;
using SIMSWebApp.Models;

namespace SIMSWebApp.Services
{
    public class CourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _courseRepository.GetAllCoursesAsync();
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await _courseRepository.GetCourseByIdAsync(id);
        }

        public async Task<Course?> GetCourseByCodeAsync(string courseCode)
        {
            return await _courseRepository.GetCourseByCodeAsync(courseCode);
        }

        public async Task<Course> CreateCourseAsync(CourseViewModel viewModel)
        {
            // Check if course code already exists
            if (await _courseRepository.CourseCodeExistsAsync(viewModel.CourseCode))
            {
                throw new InvalidOperationException("Course code already exists");
            }

            var course = new Course
            {
                CourseName = viewModel.CourseName,
                CourseCode = viewModel.CourseCode,
                Credits = viewModel.Credits,
                Duration = viewModel.Duration,
                Description = viewModel.Description,
                Department = viewModel.Department,
                Instructor = viewModel.Instructor,
                Semester = viewModel.Semester,
                AcademicYear = viewModel.AcademicYear,
                MaxStudents = viewModel.MaxStudents,
                Fee = viewModel.Fee
            };

            return await _courseRepository.CreateCourseAsync(course);
        }

        public async Task<Course> UpdateCourseAsync(CourseViewModel viewModel)
        {
            var existingCourse = await _courseRepository.GetCourseByIdAsync(viewModel.CourseID);
            if (existingCourse == null)
            {
                throw new InvalidOperationException("Course not found");
            }

            // Check if course code conflicts with another course
            if (existingCourse.CourseCode != viewModel.CourseCode)
            {
                if (await _courseRepository.CourseCodeExistsAsync(viewModel.CourseCode))
                {
                    throw new InvalidOperationException("Course code already exists");
                }
            }

            var course = new Course
            {
                CourseID = viewModel.CourseID,
                CourseName = viewModel.CourseName,
                CourseCode = viewModel.CourseCode,
                Credits = viewModel.Credits,
                Duration = viewModel.Duration,
                Description = viewModel.Description,
                Department = viewModel.Department,
                Instructor = viewModel.Instructor,
                Semester = viewModel.Semester,
                AcademicYear = viewModel.AcademicYear,
                MaxStudents = viewModel.MaxStudents,
                Fee = viewModel.Fee
            };

            return await _courseRepository.UpdateCourseAsync(course);
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            return await _courseRepository.DeleteCourseAsync(id);
        }

        public async Task<IEnumerable<Course>> GetCoursesByDepartmentAsync(string department)
        {
            return await _courseRepository.GetCoursesByDepartmentAsync(department);
        }

        public async Task<IEnumerable<Course>> GetCoursesBySemesterAsync(string semester, string academicYear)
        {
            return await _courseRepository.GetCoursesBySemesterAsync(semester, academicYear);
        }

        public CourseViewModel MapToViewModel(Course course)
        {
            return new CourseViewModel
            {
                CourseID = course.CourseID,
                CourseName = course.CourseName,
                CourseCode = course.CourseCode,
                Credits = course.Credits,
                Duration = course.Duration,
                Description = course.Description,
                Department = course.Department,
                Instructor = course.Instructor,
                Semester = course.Semester,
                AcademicYear = course.AcademicYear,
                MaxStudents = course.MaxStudents,
                Fee = course.Fee,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt,
                IsActive = course.IsActive
            };
        }
    }
}
