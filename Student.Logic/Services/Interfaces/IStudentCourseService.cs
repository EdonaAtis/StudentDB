using System.Collections.Generic;
using System.Threading.Tasks;
using Student.DataModels.Models;

namespace Student.Services
{
    public interface IStudentCourseService
    {
        Task<List<StudentCourse>> GetAllStudentCoursesAsync();
        Task<StudentCourse> GetStudentCourseAsync(int studentId, int courseId);
        Task UpdateStudentCourseAsync(int studentId, int courseId, StudentCourse studentCourse);
        Task<StudentCourse> CreateStudentCourseAsync(StudentCourse studentCourse);
        Task DeleteStudentCourseAsync(int studentId, int courseId);
        bool StudentCourseExists(int studentId, int courseId);
    }
}
