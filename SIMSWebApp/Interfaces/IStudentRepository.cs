using SIMSWebApp.DatabaseContext.Entities;

namespace SIMSWebApp.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student?> GetStudentByCodeAsync(string studentCode);
        Task<Student> CreateStudentAsync(Student student);
        Task<Student> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
        Task<bool> StudentExistsAsync(int id);
        Task<bool> StudentCodeExistsAsync(string studentCode);
    }
}
