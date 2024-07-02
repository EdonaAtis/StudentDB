using System.Collections.Generic;
using System.Threading.Tasks;
using Student.DataModels.Models;

namespace Student.Services
{
    public interface IStudentService
    {
        Task<List<StudentInfo>> GetAllStudentsAsync();
        Task<StudentInfo> GetStudentAsync(int id);
        Task<StudentInfo> CreateStudentAsync(StudentInfo student);
        Task UpdateStudentAsync(int id, StudentInfo student);
        Task DeleteStudentAsync(int id);
        bool StudentExists(int id);
    }
}
