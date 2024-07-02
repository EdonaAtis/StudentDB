using Student.DataModels.CustomModels;
using StudentDB.Services.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.SessionStorage;

namespace StudentDB.Services
{
    public class AdminPanelService : IAdminPanelService
    {
        private readonly HttpClient _httpClient;
        private readonly ISessionStorageService _sessionStorage;

        public AdminPanelService(HttpClient httpClient, ISessionStorageService sessionStorage)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _sessionStorage = sessionStorage ?? throw new ArgumentNullException(nameof(sessionStorage));
        }

        public async Task<ResponseModel> AdminLogin(LoginModel loginModel)
        {
            try
            {
                if (loginModel == null)
                {
                    throw new ArgumentNullException(nameof(loginModel), "LoginModel cannot be null.");
                }

                var response = await _httpClient.PostAsJsonAsync("api/admin/AdminLogin", loginModel);
                response.EnsureSuccessStatusCode(); // This will throw if the response is not successful

                var responseModel = await response.Content.ReadFromJsonAsync<ResponseModel>();
                if (responseModel == null)
                {
                    throw new NullReferenceException("ResponseModel is null.");
                }

                if (responseModel.Status)
                {
                    // Store the admin details in session storage
                    await _sessionStorage.SetItemAsync("adminName", responseModel.AdminName);
                    await _sessionStorage.SetItemAsync("adminEmail", responseModel.AdminEmail);
                    await _sessionStorage.SetItemAsync("adminRoles", responseModel.AdminRoles);
                }

                return responseModel;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
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
                var response = await _httpClient.PostAsJsonAsync("api/admin/Register", registerModel);
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
