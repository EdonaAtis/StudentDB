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
    [Route("api/[controller]")]
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
            var response = await _adminService.CreateCourse(course);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("UpdateCourse")]
        public async Task<IActionResult> UpdateCourse([FromBody] Course course)
        {
            var response = await _adminService.UpdateCourse(course);
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
