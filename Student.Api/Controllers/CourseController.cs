using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student.DataModels.Models;
using Student.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Student.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly CourseService _courseService;

        public CoursesController(CourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _courseService.GetCourseAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        [Authorize(Roles = "CourseManager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            try
            {
                await _courseService.UpdateCourseAsync(id, course);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid course ID");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_courseService.CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [Authorize(Roles = "CourseManager")]
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            var createdCourse = await _courseService.CreateCourseAsync(course);
            return CreatedAtAction("GetCourse", new { id = createdCourse.Id }, createdCourse);
        }

        [Authorize(Roles = "CourseManager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                await _courseService.DeleteCourseAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
