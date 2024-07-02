using Student.DataModels.Models;

namespace Student.Logic.Services.Interfaces
{
    public interface ICourseService
    {
        Task<List<Course>> GetAllCoursesAsync();
        Task<Course> GetCourseAsync(int id);
        Task UpdateCourseAsync(int id, Course course);
        Task<Course> CreateCourseAsync(Course course);
        Task DeleteCourseAsync(int id);
        bool CourseExists(int id);
    }
}
