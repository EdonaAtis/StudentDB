using Student.DataModels.CustomModels;
using Student.DataModels.Models;
using System.Threading.Tasks;

namespace Student.Logic.Services.Interfaces
{
    public interface IAdminService
    {
        Task<ResponseModel> AdminLogin(LoginModel loginModel);
        Task<ResponseModel> Register(RegisterModel registerModel);
        Task<ServiceResponse> CreateCourse(Course course);

        Task<ServiceResponse<List<Course>>> GetCourses();
        Task<ServiceResponse<List<Course>>> SearchCourses(string searchText);
        Task<ServiceResponse> DeleteCourse(int courseId);
        Task<ResponseModel> AssignRoleToUser(string userId, string role);

    }
}
