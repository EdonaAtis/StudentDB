using Microsoft.EntityFrameworkCore;
using Student.DataModels;
using Student.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student.Services
{
    public class StudentCourseService : IStudentCourseService
    {
        private readonly ApplicationDbContext _context;

        public StudentCourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudentCourse>> GetAllStudentCoursesAsync()
        {
            return await _context.StudentCourses.ToListAsync();
        }

        public async Task<StudentCourse> GetStudentCourseAsync(int studentId, int courseId)
        {
            return await _context.StudentCourses.FindAsync(studentId, courseId);
        }

        public async Task UpdateStudentCourseAsync(int studentId, int courseId, StudentCourse studentCourse)
        {
            var existingStudentCourse = await _context.StudentCourses.FindAsync(studentId, courseId);
            if (existingStudentCourse == null)
            {
                throw new KeyNotFoundException("Student course not found");
            }

            if (studentId != studentCourse.StudentId || courseId != studentCourse.CourseId)
            {
                throw new ArgumentException("Invalid student ID or course ID");
            }

            existingStudentCourse.StudentId = studentCourse.StudentId;
            existingStudentCourse.CourseId = studentCourse.CourseId;

            await _context.SaveChangesAsync();
        }

        public async Task<StudentCourse> CreateStudentCourseAsync(StudentCourse studentCourse)
        {
            _context.StudentCourses.Add(studentCourse);
            await _context.SaveChangesAsync();
            return studentCourse;
        }

        public async Task DeleteStudentCourseAsync(int studentId, int courseId)
        {
            var studentCourse = await _context.StudentCourses.FindAsync(studentId, courseId);
            if (studentCourse == null)
            {
                throw new KeyNotFoundException("Student course not found");
            }

            _context.StudentCourses.Remove(studentCourse);
            await _context.SaveChangesAsync();
        }

        public bool StudentCourseExists(int studentId, int courseId)
        {
            return _context.StudentCourses.Any(e => e.StudentId == studentId && e.CourseId == courseId);
        }
    }
}
