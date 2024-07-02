using Microsoft.EntityFrameworkCore;
using Student.DataModels;
using Student.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudentInfo>> GetAllStudentsAsync()
        {
            return await _context.StudentInfo.ToListAsync();
        }

        public async Task<StudentInfo> GetStudentAsync(int id)
        {
            return await _context.StudentInfo.FindAsync(id);
        }

        public async Task<StudentInfo> CreateStudentAsync(StudentInfo student)
        {
            _context.StudentInfo.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task UpdateStudentAsync(int id, StudentInfo student)
        {
            var existingStudent = await _context.StudentInfo.FindAsync(id);
            if (existingStudent == null)
            {
                throw new KeyNotFoundException("Student not found");
            }

            if (id != student.Id)
            {
                throw new ArgumentException("Invalid student ID");
            }

            _context.Entry(existingStudent).CurrentValues.SetValues(student);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    throw new KeyNotFoundException("Student not found");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = await _context.StudentInfo.FindAsync(id);
            if (student == null)
            {
                throw new KeyNotFoundException("Student not found");
            }

            _context.StudentInfo.Remove(student);
            await _context.SaveChangesAsync();
        }

        public bool StudentExists(int id)
        {
            return _context.StudentInfo.Any(e => e.Id == id);
        }
    }
}
