using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Student.DataModels.Models;


namespace StudentDB.Pages
{
    public class CoursesBase : ComponentBase
    {
        [Inject] protected HttpClient HttpClient { get; set; }
        [Inject] protected NavigationManager NavManager { get; set; }

        protected string searchText = string.Empty;
        protected List<Course> filteredCourses;
        protected bool showDeleteConfirmation = false;
        protected int confirmedCourseId;

        protected override async Task OnInitializedAsync()
        {
            await LoadCourses();
        }

        protected async Task LoadCourses()
        {
            filteredCourses = await HttpClient.GetFromJsonAsync<List<Course>>("api/admin/GetCourses");
        }

        protected async Task Search()
        {
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var courses = await HttpClient.GetFromJsonAsync<List<Course>>($"api/admin/SearchCourses?searchText={searchText}");
                filteredCourses = courses;
            }
            else
            {
                await Reset();
            }
        }

        protected async Task Reset()
        {
            searchText = string.Empty;
            await LoadCourses();
        }

        protected void GoToCreateCoursePage()
        {
            NavManager.NavigateTo("/createcourse");
        }

        protected void ConfirmDeleteCourse(int courseId)
        {
            confirmedCourseId = courseId;
            showDeleteConfirmation = true;
        }

        protected void CancelDelete()
        {
            showDeleteConfirmation = false;
            confirmedCourseId = 0;
        }

        protected async Task DeleteCourse(int courseId)
        {
            var response = await HttpClient.DeleteAsync($"api/admin/DeleteCourse/{courseId}");
            if (response.IsSuccessStatusCode)
            {
                await LoadCourses();
            }
            showDeleteConfirmation = false;
        }
    }
}
