using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMSWebApp.DatabaseContext;
using SIMSWebApp.Models;
using System.Security.Claims;

namespace SIMSWebApp.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly SIMSDbContext _context;

        public StudentController(SIMSDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var studentId = GetCurrentStudentId();
            if (studentId == 0) return RedirectToAction("Login", "Auth");

            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentID == studentId);

            var dashboard = new DashboardViewModel
            {
                UserRole = "Student",
                UserName = student?.User.FullName ?? "Student",
                UserEmail = student?.User.Email ?? "",
                Class = student?.Class ?? "",
                Major = student?.Major ?? "",
                EnrollmentDate = student?.EnrollmentDate ?? DateTime.Now,
                EnrolledCourses = await _context.Enrollments
                    .CountAsync(e => e.StudentID == studentId && e.Status == "Active"),
                AverageGrade = (await _context.Grades
                    .Where(g => g.StudentID == studentId && g.IsActive)
                    .Select(g => (decimal?)g.Score)
                    .AverageAsync()) ?? 0m
            };

            return View(dashboard);
        }

        // My Profile
        public async Task<IActionResult> Profile()
        {
            var studentId = GetCurrentStudentId();
            if (studentId == 0) return RedirectToAction("Login", "Auth");

            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentID == studentId);

            if (student == null) return NotFound();

            return View(student);
        }

        // My Enrolled Courses
        public async Task<IActionResult> MyCourses()
        {
            var studentId = GetCurrentStudentId();
            if (studentId == 0) return RedirectToAction("Login", "Auth");

            var courses = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentID == studentId && e.Status == "Active")
                .Select(e => e.Course)
                .OrderBy(c => c.CourseCode)
                .ToListAsync();

            return View(courses);
        }

        // Course Details
        public async Task<IActionResult> CourseDetails(int courseId)
        {
            var studentId = GetCurrentStudentId();
            if (studentId == 0) return RedirectToAction("Login", "Auth");

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Course.CourseAssignments)
                .ThenInclude(ca => ca.Teacher)
                .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(e => e.StudentID == studentId && e.CourseID == courseId);

            if (enrollment == null) return NotFound();

            return View(enrollment);
        }

        // My Grades
        public async Task<IActionResult> MyGrades()
        {
            var studentId = GetCurrentStudentId();
            if (studentId == 0) return RedirectToAction("Login", "Auth");

            var grades = await _context.Grades
                .Include(g => g.Course)
                .Include(g => g.Teacher)
                .ThenInclude(t => t.User)
                .Where(g => g.StudentID == studentId && g.IsActive)
                .OrderBy(g => g.Course.CourseCode)
                .ThenBy(g => g.GradeType)
                .ToListAsync();

            return View(grades);
        }

        // Course Grades
        public async Task<IActionResult> CourseGrades(int courseId)
        {
            var studentId = GetCurrentStudentId();
            if (studentId == 0) return RedirectToAction("Login", "Auth");

            // Verify that the student is enrolled in this course
            var enrollment = await _context.Enrollments
                .AnyAsync(e => e.StudentID == studentId && e.CourseID == courseId);

            if (!enrollment) return RedirectToAction("MyCourses");

            var grades = await _context.Grades
                .Include(g => g.Course)
                .Include(g => g.Teacher)
                .ThenInclude(t => t.User)
                .Where(g => g.StudentID == studentId && g.CourseID == courseId && g.IsActive)
                .OrderByDescending(g => g.GradingDate)
                .ToListAsync();

            return View(grades);
        }

        // Academic Transcript
        public async Task<IActionResult> Transcript()
        {
            var studentId = GetCurrentStudentId();
            if (studentId == 0) return RedirectToAction("Login", "Auth");

            var transcript = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Grades)
                .ThenInclude(g => g.Teacher)
                .ThenInclude(t => t.User)
                .Where(e => e.StudentID == studentId)
                .OrderBy(e => e.Course.CourseCode)
                .ToListAsync();

            return View(transcript);
        }

        private int GetCurrentStudentId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userId, out int userID))
            {
                var student = _context.Students.FirstOrDefault(s => s.UserID == userID);
                return student?.StudentID ?? 0;
            }
            return 0;
        }
    }
}
