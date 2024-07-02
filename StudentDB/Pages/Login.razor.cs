using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Student.DataModels.CustomModels;
using StudentDB.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace StudentDB.Pages
{
    public class LoginBase : ComponentBase
    {
        [Inject] protected HttpClient HttpClient { get; set; }
        [Inject] protected INotify Notify { get; set; }
        [Inject] protected Blazored.SessionStorage.ISessionStorageService SessionStorage { get; set; }
        [Inject] protected IAdminPanelService AdminPanelService { get; set; }
        [Inject] protected NavigationManager NavManager { get; set; }

        protected LoginModel LoginModel
        { 
            get; set;
        } = new LoginModel();
        protected string AlertMessage { get; set; }

        protected async Task Login_Click()
        {
            try
            {
                if (string.IsNullOrEmpty(LoginModel.EmailId) || string.IsNullOrEmpty(LoginModel.Password))
                {
                    AlertMessage = "Please provide both email and password.";
                    return;
                }

                var response = await AdminPanelService.AdminLogin(LoginModel);

                if (response.Status)
                {
                    if (!string.IsNullOrEmpty(response.Message))
                    {
                        var userResponse = response.Message.Split("|");
                        if (userResponse.Length == 4 && userResponse[0] == "Success")
                        {
                            await SessionStorage.SetItemAsync("adminName", userResponse[1]);
                            await SessionStorage.SetItemAsync("adminEmail", userResponse[2]);
                            await SessionStorage.SetItemAsync("adminRoles", userResponse[3]);
                            AlertMessage = "Login successful!";
                            NavManager.NavigateTo("/students");
                        }
                        else
                        {
                            AlertMessage = "Invalid response format.";
                            Console.WriteLine($"Invalid response format: {response.Message}");
                        }
                    }
                    else
                    {
                        AlertMessage = "Login response is empty.";
                        Console.WriteLine("Login response is empty.");
                    }
                }
                else
                {
                    AlertMessage = response.Message;
                    Console.WriteLine($"Login failed: {response.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during login: {ex.Message}");
                AlertMessage = "An unexpected error occurred during login.";
            }
        }

    }
}
