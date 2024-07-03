using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Student.DataModels;
using Student.DataModels.Models;
using System;
using System.Threading.Tasks;

namespace StudentDB.Pages
{
    public class CreateCourseBase : ComponentBase
    {
        protected Course course = new Course();

        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected NavigationManager navManager { get; set; }

        protected async Task HandleValidSubmit()
        {
            var response = await HttpClient.PostAsJsonAsync("api/admin/course", course);

            if (response.IsSuccessStatusCode)
            {
                navManager.NavigateTo("/courses");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {errorMessage}");
            }
        }
    }
}
