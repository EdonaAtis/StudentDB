using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Student.DataModels;
using Student.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentDB.Pages
{
    public class EditStudentBase : ComponentBase
    {
        [Parameter]
        public int id { get; set; }

        protected StudentInfo? student;
        protected bool isLoading = true;
        protected string? errorMessage;
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

        [Inject]
        protected ApplicationDbContext DbContext { get; set; }
        [Inject]
        protected NavigationManager navManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                student = await DbContext.StudentInfo.FindAsync(id);
                if (student == null)
                {
                    errorMessage = "Student not found.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error loading student: {ex.Message}";
            }
            finally
            {
                isLoading = false;
            }
        }

        protected async Task HandleValidSubmit()
        {
            try
            {
                await DbContext.SaveChangesAsync();
                navManager.NavigateTo("/students");
            }
            catch (Exception ex)
            {
                errorMessage = $"Error saving changes: {ex.Message}";
            }
        }
    }
}
