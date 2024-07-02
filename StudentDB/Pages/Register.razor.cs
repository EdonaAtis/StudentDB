using Microsoft.AspNetCore.Components;
using Student.DataModels.CustomModels;
using StudentDB.Services.Interfaces;
using System;
using System.Threading.Tasks;


namespace StudentDB.Pages
{
    public class RegisterBase : ComponentBase
    {
        [Inject] protected IAdminPanelService adminPanelService { get; set; }
        [Inject] protected NavigationManager navManager { get; set; }

        protected RegisterModel registerModel = new RegisterModel();
        protected string? alertMessage;

        protected async Task Register_Click()
        {
            if (registerModel == null || string.IsNullOrEmpty(registerModel.EmailId) || string.IsNullOrEmpty(registerModel.Password))
            {
                alertMessage = "Please provide all required fields.";
                return;
            }

            try
            {
                Console.WriteLine($"Registering user: {registerModel.EmailId}");
                var response = await adminPanelService.AdminRegister(registerModel);

                if (response.Status)
                {
                    alertMessage = "Registration successful!";
                    navManager.NavigateTo("/login");
                }
                else
                {
                    alertMessage = response.Message;
                    Console.WriteLine($"Registration failed: {response.Message}");
                }
            }
            catch (Exception ex)
            {
                alertMessage = $"An error occurred: {ex.Message}";
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}
