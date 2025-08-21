using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMSWebApp.DatabaseContext;
using SIMSWebApp.DatabaseContext.Entities;
using SIMSWebApp.Models;
using System.Security.Claims;

namespace SIMSWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly SIMSDbContext _context;

        public AdminController(SIMSDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = new DashboardViewModel
            {
                UserRole = "Admin",
                UserName = User.FindFirst("FullName")?.Value ?? "Admin",
                UserEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "",
                TotalUsers = await _context.Users.CountAsync(u => u.IsActive),
                TotalStudents = await _context.Students.CountAsync(s => s.IsActive),
                TotalTeachers = await _context.Teachers.CountAsync(t => t.IsActive),
                TotalCourses = await _context.Courses.CountAsync(c => c.IsActive),
                TotalClasses = await _context.ClassRooms.CountAsync(c => c.IsActive),
                TotalEnrollments = await _context.Enrollments.CountAsync(e => e.IsActive),
                TotalAdmins = await _context.Users.CountAsync(u => u.Role == "Admin" && u.IsActive)
            };

            return View(dashboard);
        }

        // Users management
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.OrderBy(u => u.Username).ToListAsync();
            return View(users);
        }
        
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View(new CreateUserViewModel());
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            
            // Check for duplicates
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            { 
                ModelState.AddModelError("Username", "Username already exists."); 
                return View(model);
            } 
            
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            { 
                ModelState.AddModelError("Email", "Email already exists."); 
                return View(model);
            }

            // Create user account
            var user = new User { 
                Username = model.Username, 
                PasswordHash = model.Password, 
                FullName = model.FullName, 
                Email = model.Email, 
                PhoneNumber = model.PhoneNumber, 
                Role = model.Role, 
                IsActive = true 
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "User created successfully.";
            return RedirectToAction(nameof(Users));
        }
        
        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            
            var model = new EditUserViewModel
            {
                UserID = user.UserID,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsActive = user.IsActive
            };
            
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            
            var user = await _context.Users.FindAsync(model.UserID);
            if (user == null) return NotFound();
            
            // Check for duplicates (excluding current user)
            if (await _context.Users.AnyAsync(u => u.Username == model.Username && u.UserID != model.UserID))
            { 
                ModelState.AddModelError("Username", "Username already exists."); 
                return View(model);
            } 
            
            if (await _context.Users.AnyAsync(u => u.Email == model.Email && u.UserID != model.UserID))
            { 
                ModelState.AddModelError("Email", "Email already exists."); 
                return View(model);
            }
            
            // Update user account
            user.Username = model.Username;
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Role = model.Role;
            user.IsActive = model.IsActive;
            user.UpdatedAt = DateTime.Now;
            
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                user.PasswordHash = model.Password; // In a real app, hash the password
            }
            
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "User updated successfully.";
            return RedirectToAction(nameof(Users));
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            
            // Check if this is the last admin user
            if (user.Role == "Admin" && await _context.Users.CountAsync(u => u.Role == "Admin" && u.IsActive) <= 1)
            {
                TempData["ErrorMessage"] = "Cannot delete the last admin user.";
                return RedirectToAction(nameof(Users));
            }
            
            // Instead of hard delete, set IsActive to false
            user.IsActive = false;
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "User deleted successfully.";
            return RedirectToAction(nameof(Users));
        }

        // Students management
        public async Task<IActionResult> Students()
        {
            var students = await _context.Students.Include(s => s.User).Where(s => s.IsActive).OrderBy(s => s.StudentCode).ToListAsync();
            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> CreateStudent()
        {
            ViewBag.Teachers = await _context.Teachers
                .Include(t => t.User)
                .Where(t => t.IsActive)
                .OrderBy(t => t.TeacherCode)
                .ToListAsync();
                
            ViewBag.Classes = await _context.ClassRooms
                .Where(c => c.IsActive)
                .OrderBy(c => c.ClassCode)
                .ToListAsync();
                
            return View(new CreateStudentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(CreateStudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Teachers = await _context.Teachers
                    .Include(t => t.User)
                    .Where(t => t.IsActive)
                    .OrderBy(t => t.TeacherCode)
                    .ToListAsync();
                    
                ViewBag.Classes = await _context.ClassRooms
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.ClassCode)
                    .ToListAsync();
                    
                return View(model);
            }
            
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            { 
                ModelState.AddModelError("Username", "Username already exists.");
                ViewBag.Teachers = await _context.Teachers
                    .Include(t => t.User)
                    .Where(t => t.IsActive)
                    .OrderBy(t => t.TeacherCode)
                    .ToListAsync();
                    
                ViewBag.Classes = await _context.ClassRooms
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.ClassCode)
                    .ToListAsync();
                    
                return View(model);
            } 
            
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            { 
                ModelState.AddModelError("Email", "Email already exists.");
                ViewBag.Teachers = await _context.Teachers
                    .Include(t => t.User)
                    .Where(t => t.IsActive)
                    .OrderBy(t => t.TeacherCode)
                    .ToListAsync();
                    
                ViewBag.Classes = await _context.ClassRooms
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.ClassCode)
                    .ToListAsync();
                    
                return View(model);
            } 
            
            if (await _context.Students.AnyAsync(s => s.StudentCode == model.StudentCode))
            { 
                ModelState.AddModelError("StudentCode", "Student code already exists.");
                ViewBag.Teachers = await _context.Teachers
                    .Include(t => t.User)
                    .Where(t => t.IsActive)
                    .OrderBy(t => t.TeacherCode)
                    .ToListAsync();
                    
                ViewBag.Classes = await _context.ClassRooms
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.ClassCode)
                    .ToListAsync();
                    
                return View(model);
            } 

            var user = new DatabaseContext.Entities.User { Username = model.Username, PasswordHash = model.Password, FullName = model.FullName, Email = model.Email, PhoneNumber = model.PhoneNumber, Role = "Student", IsActive = true };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var student = new DatabaseContext.Entities.Student { 
                StudentCode = model.StudentCode, 
                UserID = user.UserID, 
                Class = model.Class, 
                Major = model.Major, 
                EnrollmentDate = model.EnrollmentDate, 
                TeacherID = model.TeacherID,
                IsActive = true 
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Students));
        }

        // Courses list and actions
        public async Task<IActionResult> Courses()
        {
            var courses = await _context.Courses.Where(c => c.IsActive).OrderBy(c => c.CourseCode).ToListAsync();
            return View(courses);
        }

        // Create course
        [HttpGet]
        public IActionResult CreateCourse() => View();

        [HttpPost]
        public async Task<IActionResult> CreateCourse(DatabaseContext.Entities.Course model)
        {
            if (!ModelState.IsValid) return View(model);
            _context.Courses.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Courses));
        }

        // Enroll whole class into a course
        [HttpGet]
        public async Task<IActionResult> EnrollClass()
        {
            ViewBag.Courses = await _context.Courses.Where(c => c.IsActive).OrderBy(c => c.CourseCode).ToListAsync();
            ViewBag.ClassRooms = await _context.ClassRooms.Where(c => c.IsActive).OrderBy(c => c.ClassCode).ToListAsync();
            return View(new EnrollClassViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> EnrollClass(EnrollClassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Courses = await _context.Courses.Where(c => c.IsActive).OrderBy(c => c.CourseCode).ToListAsync();
                ViewBag.ClassRooms = await _context.ClassRooms.Where(c => c.IsActive).OrderBy(c => c.ClassCode).ToListAsync();
                return View(model);
            }

            var cls = await _context.ClassRooms.FirstOrDefaultAsync(c => c.ClassRoomID == model.ClassRoomID);
            if (cls == null)
            {
                ModelState.AddModelError("ClassRoomID", "Class not found.");
                ViewBag.Courses = await _context.Courses.Where(c => c.IsActive).OrderBy(c => c.CourseCode).ToListAsync();
                ViewBag.ClassRooms = await _context.ClassRooms.Where(c => c.IsActive).OrderBy(c => c.ClassCode).ToListAsync();
                return View(model);
            }

            // Enroll all students whose Student.Class matches the class code
            var students = await _context.Students.Where(s => s.Class == cls.ClassCode && s.IsActive).ToListAsync();
            foreach (var s in students)
            {
                if (!await _context.Enrollments.AnyAsync(e => e.StudentID == s.StudentID && e.CourseID == model.CourseID))
                {
                    _context.Enrollments.Add(new DatabaseContext.Entities.Enrollment
                    {
                        StudentID = s.StudentID,
                        CourseID = model.CourseID,
                        Status = "Active",
                        EnrollmentDate = DateTime.Now,
                        IsActive = true
                    });
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Courses));
        }

        // Classes management
        public async Task<IActionResult> Classes()
        {
            var classes = await _context.ClassRooms
                .Include(c => c.ClassCourses)
                .ThenInclude(cc => cc.Course)
                .Include(c => c.Teacher)
                .ThenInclude(t => t.User)
                .Where(c => c.IsActive)
                .OrderBy(c => c.ClassCode)
                .ToListAsync();
            return View(classes);
        }

        [HttpGet]
        public async Task<IActionResult> CreateClass()
        {
            ViewBag.Courses = await _context.Courses.Where(c => c.IsActive).OrderBy(c => c.CourseCode).ToListAsync();
            ViewBag.Teachers = await _context.Teachers
                .Include(t => t.User)
                .Where(t => t.IsActive)
                .OrderBy(t => t.TeacherCode)
                .ToListAsync();
            return View(new CreateClassViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateClass(CreateClassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Courses = await _context.Courses.Where(c => c.IsActive).OrderBy(c => c.CourseCode).ToListAsync();
                ViewBag.Teachers = await _context.Teachers
                    .Include(t => t.User)
                    .Where(t => t.IsActive)
                    .OrderBy(t => t.TeacherCode)
                    .ToListAsync();
                return View(model);
            }
            if (await _context.ClassRooms.AnyAsync(c => c.ClassCode == model.ClassCode)) 
            { 
                ModelState.AddModelError("ClassCode", "Class code already exists."); 
                ViewBag.Courses = await _context.Courses.Where(c => c.IsActive).OrderBy(c => c.CourseCode).ToListAsync();
                ViewBag.Teachers = await _context.Teachers
                    .Include(t => t.User)
                    .Where(t => t.IsActive)
                    .OrderBy(t => t.TeacherCode)
                    .ToListAsync();
                return View(model);
            } 

            var cls = new DatabaseContext.Entities.ClassRoom { 
                ClassCode = model.ClassCode, 
                ClassName = model.ClassName, 
                Department = model.Department, 
                AcademicYear = model.AcademicYear, 
                TeacherID = model.TeacherID,
                IsActive = true 
            };
            _context.ClassRooms.Add(cls);
            await _context.SaveChangesAsync();

            foreach (var courseId in model.CourseIDs.Distinct())
            {
                _context.ClassCourses.Add(new DatabaseContext.Entities.ClassCourse { ClassRoomID = cls.ClassRoomID, CourseID = courseId, IsActive = true });
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Classes));
        }

        // Enroll single student into a course
        [HttpGet]
        public async Task<IActionResult> EnrollStudent()
        {
            ViewBag.Courses = await _context.Courses.Where(c => c.IsActive).OrderBy(c => c.CourseCode).ToListAsync();
            ViewBag.Students = await _context.Students.Include(s => s.User).Where(s => s.IsActive).OrderBy(s => s.StudentCode).ToListAsync();
            return View(new EnrollStudentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> EnrollStudent(EnrollStudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Courses = await _context.Courses.Where(c => c.IsActive).OrderBy(c => c.CourseCode).ToListAsync();
                ViewBag.Students = await _context.Students.Include(s => s.User).Where(s => s.IsActive).OrderBy(s => s.StudentCode).ToListAsync();
                return View(model);
            }

            var exists = await _context.Enrollments.AnyAsync(e => e.StudentID == model.StudentID && e.CourseID == model.CourseID);
            if (!exists)
            {
                _context.Enrollments.Add(new DatabaseContext.Entities.Enrollment
                {
                    StudentID = model.StudentID,
                    CourseID = model.CourseID,
                    Status = "Active",
                    EnrollmentDate = DateTime.Now,
                    IsActive = true
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Courses));
        }

        // Course Assignment Management
        public async Task<IActionResult> CourseAssignments()
        {
            var assignments = await _context.CourseAssignments
                .Include(ca => ca.Course)
                .Include(ca => ca.Teacher)
                .ThenInclude(t => t.User)
                .Where(ca => ca.IsActive)
                .OrderBy(ca => ca.Course.CourseCode)
                .ToListAsync();
            return View(assignments);
        }

        // Enrollment Management
        public async Task<IActionResult> Enrollments()
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .ThenInclude(s => s.User)
                .Include(e => e.Course)
                .Where(e => e.IsActive)
                .OrderBy(e => e.Student.StudentCode)
                .ToListAsync();
            return View(enrollments);
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                enrollment.IsActive = false;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Enrollment deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Enrollment not found.";
            }
            return RedirectToAction(nameof(Enrollments));
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateEnrollmentStatus(int id, string status)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                enrollment.Status = status;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Enrollment status updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Enrollment not found.";
            }
            return RedirectToAction(nameof(Enrollments));
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateEnrollmentNotes(int id, string notes)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                enrollment.Notes = notes;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Enrollment notes updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Enrollment not found.";
            }
            return RedirectToAction(nameof(Enrollments));
        }

        // Grade Management
        public async Task<IActionResult> Grades()
        {
            var grades = await _context.Grades
                .Include(g => g.Student)
                .ThenInclude(s => s.User)
                .Include(g => g.Course)
                .Include(g => g.Teacher)
                .ThenInclude(t => t.User)
                .Where(g => g.IsActive)
                .OrderBy(g => g.Student.StudentCode)
                .ThenBy(g => g.Course.CourseCode)
                .ToListAsync();
            return View(grades);
        }
        
        // Teachers management
        public async Task<IActionResult> Teachers()
        {
            var teachers = await _context.Teachers
                .Include(t => t.User)
                .Where(t => t.IsActive)
                .OrderBy(t => t.TeacherCode)
                .ToListAsync();
            return View(teachers);
        }
        
        [HttpGet]
        public IActionResult CreateTeacher()
        {
            return View(new CreateTeacherViewModel());
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateTeacher(CreateTeacherViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            
            // Check for duplicates
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            { 
                ModelState.AddModelError("Username", "Username already exists."); 
                return View(model);
            } 
            
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            { 
                ModelState.AddModelError("Email", "Email already exists."); 
                return View(model);
            } 
            
            if (await _context.Teachers.AnyAsync(t => t.TeacherCode == model.TeacherCode))
            { 
                ModelState.AddModelError("TeacherCode", "Teacher code already exists."); 
                return View(model);
            } 

            // Create user account
            var user = new User { 
                Username = model.Username, 
                PasswordHash = model.Password, 
                FullName = model.FullName, 
                Email = model.Email, 
                PhoneNumber = model.PhoneNumber, 
                Role = "Teacher", 
                IsActive = true 
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Create teacher profile
            var teacher = new Teacher { 
                TeacherCode = model.TeacherCode, 
                UserID = user.UserID, 
                Department = model.Department, 
                Specialization = model.Specialization, 
                AcademicDegree = model.AcademicDegree, 
                HireDate = model.HireDate, 
                IsActive = true 
            };
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Teachers));
        }
        
        [HttpGet]
        public async Task<IActionResult> EditTeacher(int id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TeacherID == id);
                
            if (teacher == null) return NotFound();
            
            var model = new EditTeacherViewModel
            {
                TeacherID = teacher.TeacherID,
                Username = teacher.User.Username,
                FullName = teacher.User.FullName,
                Email = teacher.User.Email,
                PhoneNumber = teacher.User.PhoneNumber ?? "",
                TeacherCode = teacher.TeacherCode,
                Department = teacher.Department,
                Specialization = teacher.Specialization ?? "",
                AcademicDegree = teacher.AcademicDegree ?? "",
                HireDate = teacher.HireDate
            };
            
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditTeacher(EditTeacherViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            
            var teacher = await _context.Teachers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TeacherID == model.TeacherID);
                
            if (teacher == null) return NotFound();

            // Update user account
            teacher.User.Username = model.Username;
            teacher.User.FullName = model.FullName;
            teacher.User.Email = model.Email;
            teacher.User.PhoneNumber = model.PhoneNumber;
            teacher.User.UpdatedAt = DateTime.Now;
            
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                teacher.User.PasswordHash = model.Password; // In a real app, hash the password
            }

            // Update teacher profile
            teacher.TeacherCode = model.TeacherCode;
            teacher.Department = model.Department;
            teacher.Specialization = model.Specialization;
            teacher.AcademicDegree = model.AcademicDegree;
            teacher.HireDate = model.HireDate;
            teacher.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Teachers));
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();
            
            teacher.IsActive = false;
            teacher.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Teachers));
        }
        
        // Assign teacher to course
        [HttpGet]
        public async Task<IActionResult> AssignTeacher()
        {
            ViewBag.Courses = await _context.Courses.Where(c => c.IsActive).OrderBy(c => c.CourseCode).ToListAsync();
                ViewBag.Teachers = await _context.Teachers
                    .Include(t => t.User)
                    .Where(t => t.IsActive)
                    .OrderBy(t => t.TeacherCode)
                    .ToListAsync();
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AssignTeacher(int teacherId, int courseId, string notes)
        {
            if (await _context.CourseAssignments.AnyAsync(ca => ca.TeacherID == teacherId && ca.CourseID == courseId))
            {
                TempData["Error"] = "This teacher is already assigned to this course.";
                return RedirectToAction(nameof(CourseAssignments));
            }
            
            _context.CourseAssignments.Add(new CourseAssignment
            {
                TeacherID = teacherId,
                CourseID = courseId,
                Notes = notes,
                AssignmentDate = DateTime.Now,
                IsActive = true
            });
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CourseAssignments));
        }

        [HttpGet]
        public async Task<IActionResult> EditStudent(int id)
        {
            var student = await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.StudentID == id);
            if (student == null) return NotFound();
            
            ViewBag.Teachers = await _context.Teachers
                .Include(t => t.User)
                .Where(t => t.IsActive)
                .OrderBy(t => t.TeacherCode)
                .ToListAsync();
                
            ViewBag.Classes = await _context.ClassRooms
                .Where(c => c.IsActive)
                .OrderBy(c => c.ClassCode)
                .ToListAsync();
                
            var model = new EditStudentViewModel
            {
                StudentID = student.StudentID,
                UserID = student.UserID,
                Username = student.User.Username,
                FullName = student.User.FullName,
                Email = student.User.Email,
                PhoneNumber = student.User.PhoneNumber,
                StudentCode = student.StudentCode,
                Class = student.Class,
                Major = student.Major,
                EnrollmentDate = student.EnrollmentDate,
                TeacherID = student.TeacherID
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditStudent(EditStudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Teachers = await _context.Teachers
                    .Include(t => t.User)
                    .Where(t => t.IsActive)
                    .OrderBy(t => t.TeacherCode)
                    .ToListAsync();
                    
                ViewBag.Classes = await _context.ClassRooms
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.ClassCode)
                    .ToListAsync();
                    
                return View(model);
            }
            
            var student = await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.StudentID == model.StudentID);
            if (student == null) return NotFound();

            // Update account
            student.User.Username = model.Username;
            student.User.FullName = model.FullName;
            student.User.Email = model.Email;
            student.User.PhoneNumber = model.PhoneNumber;
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                student.User.PasswordHash = model.Password; // demo only
            }

            // Update student
            student.StudentCode = model.StudentCode;
            student.Class = model.Class;
            student.Major = model.Major;
            student.EnrollmentDate = model.EnrollmentDate;
            student.TeacherID = model.TeacherID;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Students));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentID == id);
            if (student == null) return NotFound();

            // Business rule: cannot delete if there are active or incomplete enrollments
            var hasActiveOrIncomplete = await _context.Enrollments
                .Where(e => e.StudentID == id)
                .AnyAsync(e => e.Status == "Active" || e.Status == "Dropped");
            if (hasActiveOrIncomplete)
            {
                TempData["ErrorMessage"] = "Cannot delete student: student has active or incomplete courses.";
                return RedirectToAction(nameof(Students));
            }

            // Safe to delete: remove user and student (cascade may handle)
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Student deleted successfully.";
            return RedirectToAction(nameof(Students));
        }

        // Edit/Delete Class
        [HttpGet]
        public async Task<IActionResult> EditClass(int id)
        {
            var cls = await _context.ClassRooms.Include(c => c.ClassCourses).FirstOrDefaultAsync(c => c.ClassRoomID == id);
            if (cls == null) return NotFound();
            ViewBag.Courses = await _context.Courses.Where(c => c.IsActive).OrderBy(c => c.CourseCode).ToListAsync();
            ViewBag.Teachers = await _context.Teachers
                .Include(t => t.User)
                .Where(t => t.IsActive)
                .OrderBy(t => t.TeacherCode)
                .ToListAsync();
            var model = new EditClassViewModel
            {
                ClassRoomID = cls.ClassRoomID,
                ClassCode = cls.ClassCode,
                ClassName = cls.ClassName,
                Department = cls.Department,
                AcademicYear = cls.AcademicYear,
                TeacherID = cls.TeacherID,
                CourseIDs = cls.ClassCourses.Select(cc => cc.CourseID).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditClass(EditClassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Courses = await _context.Courses.Where(c => c.IsActive).OrderBy(c => c.CourseCode).ToListAsync();
                ViewBag.Teachers = await _context.Teachers
                    .Include(t => t.User)
                    .Where(t => t.IsActive)
                    .OrderBy(t => t.TeacherCode)
                    .ToListAsync();
                return View(model);
            }
            var cls = await _context.ClassRooms.Include(c => c.ClassCourses).FirstOrDefaultAsync(c => c.ClassRoomID == model.ClassRoomID);
            if (cls == null) return NotFound();

            cls.ClassCode = model.ClassCode;
            cls.ClassName = model.ClassName;
            cls.Department = model.Department;
            cls.AcademicYear = model.AcademicYear;
            cls.TeacherID = model.TeacherID;

            // Update selected courses
            var selected = model.CourseIDs.Distinct().ToHashSet();
            var existing = cls.ClassCourses.ToList();
            // remove not selected
            foreach (var cc in existing.Where(cc => !selected.Contains(cc.CourseID)))
                _context.ClassCourses.Remove(cc);
            // add new
            foreach (var id in selected.Where(id => !existing.Any(cc => cc.CourseID == id)))
                _context.ClassCourses.Add(new DatabaseContext.Entities.ClassCourse { ClassRoomID = cls.ClassRoomID, CourseID = id, IsActive = true });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Classes));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var cls = await _context.ClassRooms.FirstOrDefaultAsync(c => c.ClassRoomID == id);
            if (cls == null) return NotFound();
            // Cannot delete class if there are students belonging to that class code
            var hasStudents = await _context.Students.AnyAsync(s => s.Class == cls.ClassCode && s.IsActive);
            if (hasStudents)
            {
                TempData["ErrorMessage"] = "Cannot delete class: there are students in this class.";
                return RedirectToAction(nameof(Classes));
            }
            _context.ClassRooms.Remove(cls);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Class deleted successfully.";
            return RedirectToAction(nameof(Classes));
        }

        // Edit/Delete Course
        [HttpGet]
        public async Task<IActionResult> EditCourse(int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseID == id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(DatabaseContext.Entities.Course model)
        {
            if (!ModelState.IsValid) return View(model);
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseID == model.CourseID);
            if (course == null) return NotFound();
            course.CourseCode = model.CourseCode;
            course.CourseName = model.CourseName;
            course.Credits = model.Credits;
            course.Duration = model.Duration;
            course.Description = model.Description;
            course.Department = model.Department;
            course.Semester = model.Semester;
            course.AcademicYear = model.AcademicYear;
            course.MaxStudents = model.MaxStudents;
            course.Fee = model.Fee;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Courses));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseID == id);
            if (course == null) return NotFound();
            // Cannot delete if any enrollments exist
            var hasEnrollments = await _context.Enrollments.AnyAsync(e => e.CourseID == id);
            if (hasEnrollments)
            {
                TempData["ErrorMessage"] = "Cannot delete course: there are enrollments in this course.";
                return RedirectToAction(nameof(Courses));
            }
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Course deleted successfully.";
            return RedirectToAction(nameof(Courses));
        }
    }
}
