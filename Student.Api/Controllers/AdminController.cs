using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student.DataModels;
using Student.DataModels.CustomModels;
using Student.DataModels.Models;
using System.Linq;

namespace Student.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext;

        public AdminController(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpPost("AdminLogin")]
        public IActionResult AdminLogin([FromBody] LoginModel loginModel)
        {
            if (loginModel == null || string.IsNullOrEmpty(loginModel.EmailId) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest("Invalid login data.");
            }

            try
            {
                var userData = _dbcontext.AdminInfos
                    .Where(x => x.Email == loginModel.EmailId && x.Password == loginModel.Password)
                    .FirstOrDefault();

                if (userData != null)
                {
                    var response = new ResponseModel
                    {
                        Status = true,
                        Message = $"{userData.Id}|{userData.Name}|{userData.Email}"
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest(new ResponseModel { Status = false, Message = "Email or Password is incorrect." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel { Status = false, Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {
            if (registerModel == null || string.IsNullOrEmpty(registerModel.EmailId) || string.IsNullOrEmpty(registerModel.Password))
            {
                return BadRequest("Invalid registration data.");
            }

            try
            {
                Console.WriteLine($"Received registration request for: {registerModel.EmailId}");

                var existingUser = _dbcontext.AdminInfos.Any(x => x.Email == registerModel.EmailId);
                if (existingUser)
                {
                    return BadRequest(new ResponseModel { Status = false, Message = "User already exists." });
                }

                var newUser = new AdminInfo
                {
                    Email = registerModel.EmailId,
                    Password = registerModel.Password // Note: Hash the password in a real-world application
                };

                _dbcontext.AdminInfos.Add(newUser);
                _dbcontext.SaveChanges();

                var response = new ResponseModel
                {
                    Status = true,
                    Message = "Registration successful!"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during registration: {ex.Message}");
                return StatusCode(500, new ResponseModel { Status = false, Message = $"An error occurred: {ex.Message}" });
            }
        }

    }
}