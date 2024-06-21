using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student.DataModels;
using Student.DataModels.CustomModels;
using Student.DataModels.Models;
using System.Linq;
using Student.Logic.Services;
using Microsoft.Extensions.Logging;


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
        public IActionResult AdminLogin([FromBody] LoginModel loginModel)
        {
            try
            {
                _logger.LogInformation("AdminLogin called with EmailId: {EmailId}", loginModel.EmailId);
                var response = _adminService.AdminLogin(loginModel);
                if (response.Status)
                {
                    _logger.LogInformation("Login successful for EmailId: {EmailId}", loginModel.EmailId);
                    return Ok(response);
                }
                else
                {
                    _logger.LogWarning("Login failed for EmailId: {EmailId}, Reason: {Reason}", loginModel.EmailId, response.Message);
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during AdminLogin for EmailId: {EmailId}", loginModel.EmailId);
                return StatusCode(500, new ResponseModel { Status = false, Message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
