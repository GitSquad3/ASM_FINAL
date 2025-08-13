using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMSWebApp.Models;
using SIMSWebApp.Services;

namespace SIMSWebApp.Controllers
{
    [Authorize(Roles = "Admin,Student")]
    public class CourseController : Controller
    {
        private readonly CourseService _courseService;

        public CourseController(CourseService courseService)
        {
            _courseService = courseService;
        }

        // GET: Course
        public async Task<IActionResult> Index()
        {
            try
            {
                var courses = await _courseService.GetAllCoursesAsync();
                var viewModels = courses.Select(c => _courseService.MapToViewModel(c));
                return View(viewModels);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading course list: {ex.Message}";
                return View(new List<CourseViewModel>());
            }
        }

        // GET: Course/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new CourseViewModel());
        }

        // POST: Course/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseName,CourseCode,Credits,Duration,Description,Department,Instructor,Semester,AcademicYear,MaxStudents,Fee")] CourseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _courseService.CreateCourseAsync(viewModel);
                    TempData["SuccessMessage"] = "Course added successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error adding course: {ex.Message}");
                }
            }
            return View(viewModel);
        }

        // GET: Course/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseService.GetCourseByIdAsync(id.Value);
            if (course == null)
            {
                return NotFound();
            }

            var viewModel = _courseService.MapToViewModel(course);
            return View(viewModel);
        }

        // POST: Course/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,CourseName,CourseCode,Credits,Duration,Description,Department,Instructor,Semester,AcademicYear,MaxStudents,Fee")] CourseViewModel viewModel)
        {
            if (id != viewModel.CourseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _courseService.UpdateCourseAsync(viewModel);
                    TempData["SuccessMessage"] = "Course updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating course: {ex.Message}");
                }
            }
            return View(viewModel);
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseService.GetCourseByIdAsync(id.Value);
            if (course == null)
            {
                return NotFound();
            }

            var viewModel = _courseService.MapToViewModel(course);
            return View(viewModel);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _courseService.DeleteCourseAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Course deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Unable to delete course. Course not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting course: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Course/ByDepartment
        public async Task<IActionResult> ByDepartment(string department)
        {
            try
            {
                var courses = await _courseService.GetCoursesByDepartmentAsync(department);
                var viewModels = courses.Select(c => _courseService.MapToViewModel(c));
                ViewBag.Department = department;
                return View("Index", viewModels);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading courses by department: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Course/BySemester
        public async Task<IActionResult> BySemester(string semester, string academicYear)
        {
            try
            {
                var courses = await _courseService.GetCoursesBySemesterAsync(semester, academicYear);
                var viewModels = courses.Select(c => _courseService.MapToViewModel(c));
                ViewBag.Semester = semester;
                ViewBag.AcademicYear = academicYear;
                return View("Index", viewModels);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading courses by semester: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
