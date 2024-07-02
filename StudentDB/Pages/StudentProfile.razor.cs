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
    public class StudentProfileBase : ComponentBase
    {
        [Parameter] public int id { get; set; }
        [Inject] protected ApplicationDbContext DbContext { get; set; }
        [Inject] protected NavigationManager NavManager { get; set; }

        protected StudentInfo student;
        protected List<Course> allCourses = new List<Course>();
        protected List<Course> enrolledCourses = new List<Course>();
        protected List<Course> availableCourses = new List<Course>();
        protected bool isLoading = true;
        protected string errorMessage;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                student = await DbContext.StudentInfo
                    .Include(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (student != null)
                {
                    allCourses = await DbContext.Courses
                        .Where(c => c.FieldOfStudy == student.FieldOfStudy)
                        .ToListAsync();

                    enrolledCourses = student.StudentCourses.Select(sc => sc.Course).ToList();
                    availableCourses = allCourses.Except(enrolledCourses).ToList();
                }
                else
                {
                    errorMessage = "Student not found.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error loading student profile: {ex.Message}";
            }
            finally
            {
                isLoading = false;
            }
        }

        protected async Task EnrollInCourse(int courseId)
        {
            var studentCourse = new StudentCourse
            {
                StudentId = student.Id,
                CourseId = courseId
            };

            DbContext.StudentCourses.Add(studentCourse);
            await DbContext.SaveChangesAsync();

            await LoadStudentCourses();
        }

        protected async Task RemoveFromCourse(int courseId)
        {
            var studentCourse = await DbContext.StudentCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == student.Id && sc.CourseId == courseId);

            if (studentCourse != null)
            {
                DbContext.StudentCourses.Remove(studentCourse);
                await DbContext.SaveChangesAsync();

                await LoadStudentCourses();
            }
        }

        protected async Task LoadStudentCourses()
        {
            student = await DbContext.StudentInfo
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(s => s.Id == student.Id);

            if (student != null)
            {
                enrolledCourses = student.StudentCourses.Select(sc => sc.Course).ToList();
                availableCourses = allCourses.Except(enrolledCourses).ToList();
            }
        }
    }
}
