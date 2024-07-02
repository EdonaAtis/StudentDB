using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student.DataModels.Models;
using Student.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Student.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCoursesController : ControllerBase
    {
        private readonly StudentCourseService _studentCourseService;

        public StudentCoursesController(StudentCourseService studentCourseService)
        {
            _studentCourseService = studentCourseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentCourse>>> GetStudentCourses()
        {
            var studentCourses = await _studentCourseService.GetAllStudentCoursesAsync();
            return Ok(studentCourses);
        }

        [HttpGet("{studentId}/{courseId}")]
        public async Task<ActionResult<StudentCourse>> GetStudentCourse(int studentId, int courseId)
        {
            var studentCourse = await _studentCourseService.GetStudentCourseAsync(studentId, courseId);

            if (studentCourse == null)
            {
                return NotFound();
            }

            return studentCourse;
        }

        [HttpPut("{studentId}/{courseId}")]
        public async Task<IActionResult> PutStudentCourse(int studentId, int courseId, StudentCourse studentCourse)
        {
            if (studentId != studentCourse.StudentId || courseId != studentCourse.CourseId)
            {
                return BadRequest("Invalid studentId or courseId");
            }

            try
            {
                await _studentCourseService.UpdateStudentCourseAsync(studentId, courseId, studentCourse);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_studentCourseService.StudentCourseExists(studentId, courseId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult<StudentCourse>> PostStudentCourse(StudentCourse studentCourse)
        {
            try
            {
                var createdStudentCourse = await _studentCourseService.CreateStudentCourseAsync(studentCourse);
                return CreatedAtAction("GetStudentCourse", new { studentId = createdStudentCourse.StudentId, courseId = createdStudentCourse.CourseId }, createdStudentCourse);
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }
        }

        [HttpDelete("{studentId}/{courseId}")]
        public async Task<IActionResult> DeleteStudentCourse(int studentId, int courseId)
        {
            try
            {
                await _studentCourseService.DeleteStudentCourseAsync(studentId, courseId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
