using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMSWebApp.DatabaseContext;
using SIMSWebApp.Models;
using System.Security.Claims;

namespace SIMSWebApp.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly SIMSDbContext _context;

        public TeacherController(SIMSDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var teacherId = GetCurrentTeacherId();
            if (teacherId == 0) return RedirectToAction("Login", "Auth");

            var teacher = await _context.Teachers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TeacherID == teacherId);

            var dashboard = new DashboardViewModel
            {
                UserRole = "Teacher",
                UserName = teacher?.User.FullName ?? "Teacher",
                UserEmail = teacher?.User.Email ?? "",
                Department = teacher?.Department ?? "",
                Specialization = teacher?.Specialization ?? "",
                AssignedCourses = await _context.CourseAssignments
                    .CountAsync(ca => ca.TeacherID == teacherId && ca.IsActive),
                TotalStudentsInCourses = await _context.CourseAssignments
                    .Where(ca => ca.TeacherID == teacherId && ca.IsActive)
                    .SelectMany(ca => ca.Course.Enrollments)
                    .Where(e => e.Status == "Active")
                    .Select(e => e.StudentID)
                    .Distinct()
                    .CountAsync()
            };

            return View(dashboard);
        }

        // Courses assigned to this teacher
        public async Task<IActionResult> MyCourses()
        {
            var teacherId = GetCurrentTeacherId();
            if (teacherId == 0) return RedirectToAction("Login", "Auth");

            var courses = await _context.CourseAssignments
                .Include(ca => ca.Course)
                .Where(ca => ca.TeacherID == teacherId && ca.IsActive)
                .Select(ca => ca.Course)
                .OrderBy(c => c.CourseCode)
                .ToListAsync();

            return View(courses);
        }

        // Students in My Courses
        public async Task<IActionResult> MyStudents(int courseId)
        {
            var teacherId = GetCurrentTeacherId();
            if (teacherId == 0) return RedirectToAction("Login", "Auth");

            var students = await _context.Enrollments
                .Include(e => e.Student)
                .ThenInclude(s => s.User)
                .Where(e => e.CourseID == courseId && 
                           e.Status == "Active" &&
                           e.Course.CourseAssignments.Any(ca => ca.TeacherID == teacherId))
                .Select(e => e.Student)
                .OrderBy(s => s.StudentCode)
                .ToListAsync();

            ViewBag.CourseId = courseId;
            return View(students);
        }
        
        // Xem danh sách lớp học được phân công
        public async Task<IActionResult> MyClassStudents()
        {
            var teacherId = GetCurrentTeacherId();
            if (teacherId == 0) return RedirectToAction("Login", "Auth");
            
            // Lấy danh sách lớp học được phân công cho giáo viên này
            var classes = await _context.ClassRooms
                .Where(c => c.TeacherID == teacherId && c.IsActive)
                .OrderBy(c => c.ClassCode)
                .ToListAsync();
                
            return View(classes);
        }

        // Xem danh sách học sinh trong một lớp cụ thể
        public async Task<IActionResult> ClassStudents(int classId)
        {
            var teacherId = GetCurrentTeacherId();
            if (teacherId == 0) return RedirectToAction("Login", "Auth");
            
            // Kiểm tra xem lớp có được phân công cho giáo viên này không
            var classRoom = await _context.ClassRooms
                .FirstOrDefaultAsync(c => c.ClassRoomID == classId && c.TeacherID == teacherId && c.IsActive);
                
            if (classRoom == null)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập lớp này.";
                return RedirectToAction(nameof(MyClassStudents));
            }
            
            // Lấy danh sách học sinh có Class trùng với ClassCode của lớp
            var students = await _context.Students
                .Include(s => s.User)
                .Where(s => s.Class == classRoom.ClassCode && s.IsActive)
                .OrderBy(s => s.StudentCode)
                .ToListAsync();
                
            ViewBag.ClassRoom = classRoom;
            return View(students);
        }
        
        // Xem và chấm điểm cho học sinh trong lớp
        public async Task<IActionResult> ClassStudentGrades(int classId, int studentId)
        {
            var teacherId = GetCurrentTeacherId();
            if (teacherId == 0) return RedirectToAction("Login", "Auth");
            
            // Kiểm tra xem lớp có được phân công cho giáo viên này không
            var classRoom = await _context.ClassRooms
                .FirstOrDefaultAsync(c => c.ClassRoomID == classId && c.TeacherID == teacherId && c.IsActive);
                
            if (classRoom == null)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập lớp này.";
                return RedirectToAction(nameof(MyClassStudents));
            }
            
            // Kiểm tra học sinh có thuộc lớp này không
            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentID == studentId && s.Class == classRoom.ClassCode && s.IsActive);
                
            if (student == null)
            {
                TempData["ErrorMessage"] = "Học sinh không thuộc lớp này.";
                return RedirectToAction(nameof(ClassStudents), new { classId });
            }
            
            // Lấy danh sách điểm của học sinh do giáo viên này chấm
            var grades = await _context.Grades
                .Where(g => g.StudentID == studentId && 
                           g.TeacherID == teacherId &&
                           g.IsActive)
                .OrderBy(g => g.GradeType)
                .ThenBy(g => g.GradingDate)
                .ToListAsync();
            
            ViewBag.ClassId = classId;
            ViewBag.StudentId = studentId;
            ViewBag.Student = student;
            ViewBag.ClassRoom = classRoom;
            return View(grades);
        }
        
        // Thêm điểm cho học sinh trong lớp
        [HttpGet]
        public async Task<IActionResult> AddClassGrade(int classId, int studentId)
        {
            var teacherId = GetCurrentTeacherId();
            if (teacherId == 0) return RedirectToAction("Login", "Auth");
            
            // Kiểm tra xem lớp có được phân công cho giáo viên này không
            var classRoom = await _context.ClassRooms
                .FirstOrDefaultAsync(c => c.ClassRoomID == classId && c.TeacherID == teacherId && c.IsActive);
                
            if (classRoom == null)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập lớp này.";
                return RedirectToAction(nameof(MyClassStudents));
            }
            
            // Kiểm tra học sinh có thuộc lớp này không
            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentID == studentId && s.Class == classRoom.ClassCode && s.IsActive);
                
            if (student == null)
            {
                TempData["ErrorMessage"] = "Học sinh không thuộc lớp này.";
                return RedirectToAction(nameof(ClassStudents), new { classId });
            }
            
            // Lấy danh sách khóa học mà học sinh đã đăng ký
            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentID == studentId && e.Status == "Active")
                .ToListAsync();
                
            var courseList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                enrollments.Select(e => e.Course).OrderBy(c => c.CourseCode),
                "CourseID", 
                "CourseCode",
                null);
            
            var model = new Models.GradeViewModel
            {
                StudentId = studentId,
                TeacherId = teacherId,
                GradingDate = DateTime.Now,
                Weight = 1.0m
            };
            
            ViewBag.ClassId = classId;
            ViewBag.StudentId = studentId;
            ViewBag.Student = student;
            ViewBag.ClassRoom = classRoom;
            ViewBag.Courses = courseList;
            return View(model);
        }
        
        // Thêm điểm cho học sinh trong lớp
        [HttpPost]
        public async Task<IActionResult> AddClassGrade(Models.GradeViewModel model, int classId)
        {
            var teacherId = GetCurrentTeacherId();
            if (teacherId == 0) return RedirectToAction("Login", "Auth");
            
            // Kiểm tra xem lớp có được phân công cho giáo viên này không
            var classRoom = await _context.ClassRooms
                .FirstOrDefaultAsync(c => c.ClassRoomID == classId && c.TeacherID == teacherId && c.IsActive);
                
            if (classRoom == null)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập lớp này.";
                return RedirectToAction(nameof(MyClassStudents));
            }
            
            // Kiểm tra học sinh có thuộc lớp này không
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentID == model.StudentId && s.Class == classRoom.ClassCode && s.IsActive);
                
            if (student == null)
            {
                TempData["ErrorMessage"] = "Học sinh không thuộc lớp này.";
                return RedirectToAction(nameof(ClassStudents), new { classId });
            }
            
            // Kiểm tra học sinh có đăng ký khóa học này không
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.CourseID == model.CourseId && e.StudentID == model.StudentId && e.Status == "Active");
                
            if (enrollment == null)
            {
                TempData["ErrorMessage"] = "Học sinh chưa đăng ký khóa học này.";
                return RedirectToAction(nameof(ClassStudentGrades), new { classId, studentId = model.StudentId });
            }
            
            var grade = new DatabaseContext.Entities.Grade
            {
                StudentID = model.StudentId,
                CourseID = model.CourseId,
                TeacherID = model.TeacherId,
                EnrollmentID = enrollment.EnrollmentID,
                GradeType = model.GradeType,
                Score = model.Score,
                Weight = model.Weight,
                Comments = model.Comments,
                GradingDate = model.GradingDate,
                IsActive = true
            };
            
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Đã thêm điểm thành công.";
            return RedirectToAction(nameof(ClassStudentGrades), new { classId, studentId = model.StudentId });
        }

        // Course grade management actions have been removed as requested

        // Xem danh sách các lớp trong một khóa học
        public async Task<IActionResult> CourseClasses(int courseId)
        {
            var teacherId = GetCurrentTeacherId();
            if (teacherId == 0) return RedirectToAction("Login", "Auth");
            
            // Kiểm tra xem khóa học có được phân công cho giáo viên này không
            var courseAssignment = await _context.CourseAssignments
                .FirstOrDefaultAsync(ca => ca.CourseID == courseId && ca.TeacherID == teacherId && ca.IsActive);
                
            if (courseAssignment == null)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập khóa học này.";
                return RedirectToAction(nameof(MyCourses));
            }
            
            // Lấy thông tin khóa học
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseID == courseId && c.IsActive);
                
            if (course == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy khóa học.";
                return RedirectToAction(nameof(MyCourses));
            }
            
            // Lấy danh sách các lớp học có khóa học này
            var classes = await _context.ClassCourses
                .Include(cc => cc.ClassRoom)
                .ThenInclude(cr => cr.Teacher)
                .ThenInclude(t => t.User)
                .Where(cc => cc.CourseID == courseId && cc.IsActive)
                .Select(cc => cc.ClassRoom)
                .OrderBy(cr => cr.ClassCode)
                .ToListAsync();
            
            ViewBag.Course = course;
            return View(classes);
        }

        // My Profile
        public async Task<IActionResult> Profile()
        {
            var teacherId = GetCurrentTeacherId();
            if (teacherId == 0) return RedirectToAction("Login", "Auth");

            var teacher = await _context.Teachers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TeacherID == teacherId);

            if (teacher == null) return NotFound();

            return View(teacher);
        }

        private int GetCurrentTeacherId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userId, out int userID))
            {
                var teacher = _context.Teachers.FirstOrDefault(t => t.UserID == userID);
                return teacher?.TeacherID ?? 0;
            }
            return 0;
        }
    }
}
