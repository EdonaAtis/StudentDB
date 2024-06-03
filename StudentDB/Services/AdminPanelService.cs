using Student.DataModels.CustomModels;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace StudentDB.Services
{
    public class AdminPanelService : IAdminPanelService
    {
        private readonly HttpClient httpClient;

        public AdminPanelService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ResponseModel> AdminLogin(LoginModel loginModel)
        {
            try
            {
                if (loginModel == null)
                {
                    throw new ArgumentNullException(nameof(loginModel), "LoginModel cannot be null.");
                }

                var response = await httpClient.PostAsJsonAsync("api/admin/AdminLogin", loginModel);
                response.EnsureSuccessStatusCode(); // This will throw if the response is not successful

                var responseModel = await response.Content.ReadFromJsonAsync<ResponseModel>();
                if (responseModel == null)
                {
                    throw new NullReferenceException("ResponseModel is null.");
                }

                return responseModel;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                return new ResponseModel { Status = false, Message = "An error occurred while processing your request." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponseModel { Status = false, Message = "An unexpected error occurred." };
            }
        }

        public async Task<ResponseModel> AdminRegister(RegisterModel registerModel)
        {
            try
            {
                Console.WriteLine($"Sending registration request for: {registerModel.EmailId}");
                var response = await httpClient.PostAsJsonAsync("api/admin/Register", registerModel);
                response.EnsureSuccessStatusCode();

                var responseModel = await response.Content.ReadFromJsonAsync<ResponseModel>();
                if (responseModel == null)
                {
                    throw new NullReferenceException("ResponseModel is null.");
                }

                return responseModel;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                return new ResponseModel { Status = false, Message = "An error occurred while processing your request." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponseModel { Status = false, Message = "An unexpected error occurred." };
            }
        }

    }
}
