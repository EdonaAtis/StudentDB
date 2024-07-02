using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Student.DataModels;
using Student.DataModels.CustomModels;
using Student.DataModels.Models;
using Student.Logic.Services.Interfaces;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Student.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public AdminController(IAdminService adminService, ILogger<AdminController> logger, UserManager<IdentityUser> userManager, ApplicationDbContext dbContext)
        {
            _adminService = adminService;
            _logger = logger;
            _userManager = userManager;
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpPost("AdminLogin")]
        public async Task<ResponseModel> AdminLogin(LoginModel loginModel)
        {
            var response = new ResponseModel();

            try
            {
                if (loginModel == null)
                {
                    throw new ArgumentNullException(nameof(loginModel));
                }

                if (string.IsNullOrEmpty(loginModel.EmailId) || string.IsNullOrEmpty(loginModel.Password))
                {
                    response.Status = false;
                    response.Message = "Email and Password cannot be empty";
                    return response;
                }

                _logger.LogInformation("Attempting to login with EmailId: {EmailId}", loginModel.EmailId);

                var userData = await _dbContext.AdminInfos
                    .FirstOrDefaultAsync(x => x.Email == loginModel.EmailId && x.Password == loginModel.Password);

                if (userData != null)
                {
                    _logger.LogInformation("Login successful for EmailId: {EmailId}", loginModel.EmailId);
                    response.Status = true;

                    // Get the user from Identity
                    var user = await _userManager.FindByEmailAsync(loginModel.EmailId);
                    if (user != null)
                    {
                        // Get the roles for the user
                        var roles = await _userManager.GetRolesAsync(user);
                        var rolesString = string.Join(",", roles);

                        response.Message = $"Success|{userData.Name}|{userData.Email}|{rolesString}";
                    }
                    else
                    {
                        response.Status = false;
                        response.Message = "User not found in identity.";
                    }
                }
                else
                {
                    _logger.LogWarning("Login failed for EmailId: {EmailId} - Email or Password is incorrect", loginModel.EmailId);
                    response.Status = false;
                    response.Message = "Email or Password is incorrect.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during AdminLogin for EmailId: {EmailId}", loginModel.EmailId);
                response.Status = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }



        [HttpPost("CreateCourse")]
        public async Task<IActionResult> CreateCourse([FromBody] Course course)
        {
            try
            {
                _logger.LogInformation("CreateCourse called for CourseName: {CourseName}", course.Name);
                var response = await _adminService.CreateCourse(course); // Await the asynchronous method
                if (response.Status)
                {
                    _logger.LogInformation("Course created successfully for CourseName: {CourseName}", course.Name);
                    return Ok(response);
                }
                else
                {
                    _logger.LogWarning("Failed to create course for CourseName: {CourseName}, Reason: {Reason}", course.Name, response.Message);
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during CreateCourse for CourseName: {CourseName}", course.Name);
                return StatusCode(500, new ResponseModel { Status = false, Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPut("UpdateCourse")]
        public async Task<IActionResult> UpdateCourse([FromBody] Course course)
        {
            try
            {
                _logger.LogInformation("UpdateCourse called for CourseId: {CourseId}", course.Id);
                var response = await _adminService.UpdateCourse(course); // Await the asynchronous method
                if (response.Status)
                {
                    _logger.LogInformation("Course updated successfully for CourseId: {CourseId}", course.Id);
                    return Ok(response);
                }
                else
                {
                    _logger.LogWarning("Failed to update course for CourseId: {CourseId}, Reason: {Reason}", course.Id, response.Message);
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during UpdateCourse for CourseId: {CourseId}", course.Id);
                return StatusCode(500, new ResponseModel { Status = false, Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpDelete("DeleteCourse/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                _logger.LogInformation("DeleteCourse called for CourseId: {CourseId}", id);
                var response = await _adminService.DeleteCourse(id); // Await the asynchronous method
                if (response.Status)
                {
                    _logger.LogInformation("Course deleted successfully for CourseId: {CourseId}", id);
                    return Ok(response);
                }
                else
                {
                    _logger.LogWarning("Failed to delete course for CourseId: {CourseId}, Reason: {Reason}", id, response.Message);
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during DeleteCourse for CourseId: {CourseId}", id);
                return StatusCode(500, new ResponseModel { Status = false, Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleModel model)
        {
            try
            {
                _logger.LogInformation("AssignRoleToUser called for UserId: {UserId}, Role: {Role}", model.UserId, model.Role);
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    _logger.LogWarning("User not found for UserId: {UserId}", model.UserId);
                    return NotFound(new ResponseModel { Status = false, Message = "User not found." });
                }

                var result = await _userManager.AddToRoleAsync(user, model.Role);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Role {Role} assigned to UserId: {UserId} successfully", model.Role, model.UserId);
                    return Ok(new ResponseModel { Status = true, Message = "Role assigned successfully." });
                }
                else
                {
                    _logger.LogWarning("Failed to assign role {Role} to UserId: {UserId}, Errors: {Errors}", model.Role, model.UserId, string.Join(", ", result.Errors.Select(e => e.Description)));
                    return BadRequest(new ResponseModel { Status = false, Message = "Failed to assign role: " + string.Join(", ", result.Errors.Select(e => e.Description)) });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during AssignRoleToUser for UserId: {UserId}, Role: {Role}", model.UserId, model.Role);
                return StatusCode(500, new ResponseModel { Status = false, Message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
