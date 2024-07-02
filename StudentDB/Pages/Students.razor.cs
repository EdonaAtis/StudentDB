using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Student.DataModels;
using Student.DataModels.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace StudentDB.Pages
{
    public class StudentsBase : ComponentBase
    {
        [Inject] protected ApplicationDbContext DbContext { get; set; }
        [Inject] protected NavigationManager NavManager { get; set; }

        protected List<StudentInfo>? StudentInfo;
        protected List<StudentInfo>? filteredStudents;
        protected string? searchText;
        protected bool showDeleteConfirmation = false;
        protected int confirmedStudentId;

        protected override async Task OnInitializedAsync()
        {
            StudentInfo = await DbContext.StudentInfo.ToListAsync();
            filteredStudents = new List<StudentInfo>(StudentInfo);
        }

        protected void Search()
        {
            filteredStudents = StudentInfo.Where(s =>
                s.FirstName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                s.LastName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                s.EmailAddress.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                s.PhoneNumber.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                s.FieldOfStudy.Contains(searchText, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        protected void Reset()
        {
            searchText = string.Empty;
            filteredStudents = new List<StudentInfo>(StudentInfo);
        }

        protected void GoToCreateStudentPage()
        {
            NavManager.NavigateTo("/createstudent");
        }

        protected void ConfirmDeleteStudent(int id)
        {
            confirmedStudentId = id;
            showDeleteConfirmation = true;
        }

        protected void CancelDelete()
        {
            confirmedStudentId = 0;
            showDeleteConfirmation = false;
        }

        protected async Task DeleteStudent(int id)
        {
            showDeleteConfirmation = false;
            var student = await DbContext.StudentInfo.FindAsync(id);
            if (student != null)
            {
                DbContext.StudentInfo.Remove(student);
                await DbContext.SaveChangesAsync();
                StudentInfo = await DbContext.StudentInfo.ToListAsync();
                filteredStudents = new List<StudentInfo>(StudentInfo);
            }
        }
    }
}