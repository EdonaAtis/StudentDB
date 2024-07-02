using Student.DataModels.CustomModels;
using Student.DataModels.Models;
using System.Threading.Tasks;

namespace Student.Logic.Services.Interfaces
{
    public interface IAdminService
    {
        Task<ResponseModel> AdminLogin(LoginModel loginModel);
        Task<ResponseModel> Register(RegisterModel registerModel);
        Task<ResponseModel> CreateCourse(Course course);
        Task<ResponseModel> UpdateCourse(Course course);
        Task<ResponseModel> DeleteCourse(int courseId);
    }
}
