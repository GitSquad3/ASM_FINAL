using Microsoft.EntityFrameworkCore;
using SIMSWebApp.DatabaseContext;
using SIMSWebApp.DatabaseContext.Entities;
using SIMSWebApp.Interfaces;

namespace SIMSWebApp.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SIMSDbContext _context;

        public StudentRepository(SIMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Where(s => s.IsActive)
                .OrderBy(s => s.FullName)
                .ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.StudentID == id && s.IsActive);
        }

        public async Task<Student?> GetStudentByCodeAsync(string studentCode)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.StudentCode == studentCode && s.IsActive);
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            student.CreatedAt = DateTime.Now;
            student.IsActive = true;
            
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            var existingStudent = await _context.Students.FindAsync(student.StudentID);
            if (existingStudent == null)
                throw new InvalidOperationException("Student not found");

            existingStudent.FullName = student.FullName;
            existingStudent.StudentCode = student.StudentCode;
            existingStudent.DateOfBirth = student.DateOfBirth;
            existingStudent.Gender = student.Gender;
            existingStudent.Address = student.Address;
            existingStudent.PhoneNumber = student.PhoneNumber;
            existingStudent.Email = student.Email;
            existingStudent.Class = student.Class;
            existingStudent.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingStudent;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return false;

            student.IsActive = false;
            student.UpdatedAt = DateTime.Now;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> StudentExistsAsync(int id)
        {
            return await _context.Students
                .AnyAsync(s => s.StudentID == id && s.IsActive);
        }

        public async Task<bool> StudentCodeExistsAsync(string studentCode)
        {
            return await _context.Students
                .AnyAsync(s => s.StudentCode == studentCode && s.IsActive);
        }
    }
}
