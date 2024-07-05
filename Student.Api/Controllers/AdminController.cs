using Microsoft.AspNetCore.Mvc;
using Student.DataModels;
using Student.DataModels.CustomModels;
using Student.DataModels.Models;
using Student.Logic.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Student.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        [HttpPost("AdminLogin")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var response = await _adminService.AdminLogin(loginModel);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            var response = await _adminService.Register(registerModel);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("CreateCourse")]
        public async Task<IActionResult> CreateCourse([FromBody] Course course)
        {
            try
            {
                var response = await _adminService.CreateCourse(course);
                if (response.Status)
                {
                    return Ok(response);
                }
                else
                {
                    _logger.LogError($"Failed to create course: {response.Message}");
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred while creating course: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    

    [HttpGet("GetCourses")]
        public async Task<IActionResult> GetCourses()
        {
            var response = await _adminService.GetCourses();
            return Ok(response);
        }

        [HttpGet("SearchCourses")]
        public async Task<IActionResult> SearchCourses([FromQuery] string searchText)
        {
            var response = await _adminService.SearchCourses(searchText);
            return Ok(response);
        }

        [HttpDelete("DeleteCourse/{courseId}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            var response = await _adminService.DeleteCourse(courseId);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleModel model)
        { 
            var response = await _adminService.AssignRoleToUser(model.UserId, model.Role);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
