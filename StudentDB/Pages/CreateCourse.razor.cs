using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Student.DataModels.Models;
using Student.DataModels.CustomModels;

namespace StudentDB.Pages
{
    public class CreateCourseBase : ComponentBase
    {
        [Inject] protected HttpClient HttpClient { get; set; }
        [Inject] protected NavigationManager NavManager { get; set; }

        protected Course course = new Course();
        protected List<string> fieldsOfStudy = new List<string>
        {
            "Computer Science",
            "Mathematics",
            "Physics",
            "Chemistry",
            "Biology",
            "Engineering",
            "Literature",
            "History",
            "IT"
        };

        protected async Task HandleValidSubmit()
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync("api/admin/CreateCourse", course);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadFromJsonAsync<ServiceResponse<List<Course>>>();
                    if (responseBody != null && responseBody.Status)
                    {
                        NavManager.NavigateTo("/courses");
                    }
                    else
                    {
                        // Log or handle unexpected response
                        Console.WriteLine("Unexpected response from API or response status is false");
                    }
                }
                else
                {
                    // Log non-success status code
                    Console.WriteLine($"Failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }
        }
    }
}
