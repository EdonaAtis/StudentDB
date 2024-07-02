using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Student.DataModels;
using Student.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentDB.Pages
{
    public class CreateStudentBase : ComponentBase
    {
        protected StudentInfo student = new StudentInfo();
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

        protected async Task HandleValidSubmit()
        {
            DbContext.StudentInfo.Add(student);
            await DbContext.SaveChangesAsync();
            navManager.NavigateTo("/students");
        }
    }
}
