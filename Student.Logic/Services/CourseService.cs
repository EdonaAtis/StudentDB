using Microsoft.EntityFrameworkCore;
using Student.DataModels;
using Student.DataModels.Models;
using Student.Logic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student.Services
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course> GetCourseAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task UpdateCourseAsync(int id, Course course)
        {
            if (id != course.Id)
            {
                throw new ArgumentException("Invalid course ID");
            }

            _context.Entry(course).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Course> CreateCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                throw new KeyNotFoundException("Course not found");
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }

        public bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
