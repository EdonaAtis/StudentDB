using Student.DataModels.CustomModels;

namespace StudentDB.Services.Interfaces
{
    public interface IAdminPanelService
    {
        Task<ResponseModel> AdminLogin(LoginModel loginModel);
        Task<ResponseModel> AdminRegister(RegisterModel registerModel);
    }
}
