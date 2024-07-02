using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Student.DataModels;
using Student.DataModels.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentDB.Pages
{
    public class MajorsBase : ComponentBase
    {
        [Inject] protected ApplicationDbContext DbContext { get; set; }
        [Inject] protected NavigationManager navManager { get; set; }
        [Inject] protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

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
            "Philosophy"
        };

        protected List<StudentInfo> studentsInField;
        protected bool showDeleteConfirmation;
        protected int confirmedStudentId;
        protected bool isSuperAdmin;

        protected async Task ShowStudentsInField(string fieldOfStudy)
        {
            studentsInField = await DbContext.StudentInfo
                .Where(s => s.FieldOfStudy == fieldOfStudy)
                .ToListAsync();
        }

        protected void ConfirmDeleteStudent(int studentId)
        {
            confirmedStudentId = studentId;
            showDeleteConfirmation = true;
        }

        protected void CancelDelete()
        {
            showDeleteConfirmation = false;
            confirmedStudentId = 0;
        }

        protected async Task DeleteStudent(int studentId)
        {
            var student = await DbContext.StudentInfo.FindAsync(studentId);
            if (student != null)
            {
                DbContext.StudentInfo.Remove(student);
                await DbContext.SaveChangesAsync();
                studentsInField.Remove(student);
            }
            showDeleteConfirmation = false;
        }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            isSuperAdmin = user.Identity.IsAuthenticated && user.IsInRole("SuperAdmin");
        }
    }
}
