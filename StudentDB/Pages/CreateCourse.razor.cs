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
        protected ApplicationDbContext DbContext { get; set; }
        [Inject]
        protected NavigationManager navManager { get; set; }

        protected async Task HandleValidSubmit()
        {
            DbContext.Courses.Add(course);
            await DbContext.SaveChangesAsync();
            navManager.NavigateTo("/courses");
        }
    }
}
