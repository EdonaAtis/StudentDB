using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Student.DataModels.Models;
using Student.Services;
using StudentDB.Services;

namespace Student.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentsController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentInfo>>> GetStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentInfo>> GetStudent(int id)
        {
            var student = await _studentService.GetStudentAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        [HttpPost]
        public async Task<ActionResult<StudentInfo>> PostStudent(StudentInfo student)
        {
            var createdStudent = await _studentService.CreateStudentAsync(student);
            return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.Id }, createdStudent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, StudentInfo student)
        {
            try
            {
                await _studentService.UpdateStudentAsync(id, student);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                await _studentService.DeleteStudentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
