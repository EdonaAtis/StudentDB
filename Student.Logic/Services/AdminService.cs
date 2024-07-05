using Microsoft.Extensions.Logging;
using Student.DataModels.CustomModels;
using Student.DataModels.Models;
using Student.Logic.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Student.Logic.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AdminService> _logger;
        private readonly UserManager<IdentityUser> _userManager;


        public AdminService(ApplicationDbContext dbContext, ILogger<AdminService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

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
                    response.Message = $"Success|{userData.Name}|{userData.Email}|Teacher,SuperAdmin";
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

        public async Task<ResponseModel> Register(RegisterModel registerModel)
        {
            var response = new ResponseModel();

            try
            {
                if (registerModel == null)
                {
                    throw new ArgumentNullException(nameof(registerModel));
                }

                if (string.IsNullOrEmpty(registerModel.EmailId) || string.IsNullOrEmpty(registerModel.Password))
                {
                    response.Status = false;
                    response.Message = "Email and Password cannot be empty";
                    return response;
                }

                _logger.LogInformation("Attempting to register with EmailId: {EmailId}", registerModel.EmailId);

                var existingUser = await _dbContext.AdminInfos.AnyAsync(x => x.Email == registerModel.EmailId);
                if (existingUser)
                {
                    _logger.LogWarning("Registration failed for EmailId: {EmailId} - User already exists", registerModel.EmailId);
                    response.Status = false;
                    response.Message = "User already exists.";
                    return response;
                }

                var newUser = new AdminInfo
                {
                    Email = registerModel.EmailId,
                    Password = registerModel.Password
                };

                await _dbContext.AdminInfos.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Registration successful for EmailId: {EmailId}", registerModel.EmailId);
                response.Status = true;
                response.Message = "Registration successful!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration for EmailId: {EmailId}", registerModel.EmailId);
                response.Status = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }


        public async Task<ServiceResponse> CreateCourse(Course course)
        {
            var response = new ServiceResponse();

            try
            {
                _dbContext.Courses.Add(course);
                await _dbContext.SaveChangesAsync();
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = $"Failed to create course: {ex.Message}";
                _logger.LogError(response.Message);
            }

            return response;
        }

        public async Task<ServiceResponse<List<Course>>> GetCourses()
        {
            var response = new ServiceResponse<List<Course>>();
            response.Data = await _dbContext.Courses.ToListAsync();
            response.Status = true;
            return response;
        }

        public async Task<ServiceResponse<List<Course>>> SearchCourses(string searchText)
        {
            var response = new ServiceResponse<List<Course>>();
            response.Data = await _dbContext.Courses
                .Where(c => c.Name.Contains(searchText) || c.FieldOfStudy.Contains(searchText))
                .ToListAsync();
            response.Status = true;
            return response;
        }

        public async Task<ServiceResponse> DeleteCourse(int courseId)
        {
            var response = new ServiceResponse();
            var course = await _dbContext.Courses.FindAsync(courseId);
            if (course != null)
            {
                _dbContext.Courses.Remove(course);
                await _dbContext.SaveChangesAsync();
                response.Status = true;
            }
            else
            {
                response.Status = false;
                response.Message = "Course not found";
            }
            return response;
        }


        public async Task<ResponseModel> AssignRoleToUser(string userId, string role)
        {
            var response = new ResponseModel();
            try
            {
                _logger.LogInformation("AssignRoleToUser called for UserId: {UserId}, Role: {Role}", userId, role);
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User not found for UserId: {UserId}", userId);
                    response.Status = false;
                    response.Message = "User not found.";
                    return response;
                }

                var result = await _userManager.AddToRoleAsync(user, role);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Role {Role} assigned to UserId: {UserId} successfully", role, userId);
                    response.Status = true;
                    response.Message = "Role assigned successfully.";
                }
                else
                {
                    _logger.LogWarning("Failed to assign role {Role} to UserId: {UserId}, Errors: {Errors}", role, userId, string.Join(", ", result.Errors.Select(e => e.Description)));
                    response.Status = false;
                    response.Message = "Failed to assign role: " + string.Join(", ", result.Errors.Select(e => e.Description));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during AssignRoleToUser for UserId: {UserId}, Role: {Role}", userId, role);
                response.Status = false;
                response.Message = $"An error occurred: {ex.Message}";
            }
            return response;
        }
    }
}


