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
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

        public async Task<ResponseModel> CreateCourse(Course course)
        {
            var response = new ResponseModel();

            try
            {
                if (course == null)
                {
                    throw new ArgumentNullException(nameof(course));
                }

                _logger.LogInformation("Attempting to create course: {CourseName}", course.Name);

                var existingCourse = await _dbContext.Courses.AnyAsync(c => c.Name == course.Name && c.FieldOfStudy == course.FieldOfStudy);
                if (existingCourse)
                {
                    _logger.LogWarning("Course creation failed - Course already exists: {CourseName}", course.Name);
                    response.Status = false;
                    response.Message = "Course already exists.";
                    return response;
                }

                await _dbContext.Courses.AddAsync(course);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Course created successfully: {CourseName}", course.Name);
                response.Status = true;
                response.Message = "Course created successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during course creation for CourseName: {CourseName}", course.Name);
                response.Status = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        public async Task<ResponseModel> UpdateCourse(Course course)
        {
            var response = new ResponseModel();

            try
            {
                if (course == null)
                {
                    throw new ArgumentNullException(nameof(course));
                }

                _logger.LogInformation("Attempting to update course: {CourseId}", course.Id);

                var existingCourse = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == course.Id);
                if (existingCourse == null)
                {
                    _logger.LogWarning("Course update failed - Course not found: {CourseId}", course.Id);
                    response.Status = false;
                    response.Message = "Course not found.";
                    return response;
                }

                existingCourse.Name = course.Name;
                existingCourse.FieldOfStudy = course.FieldOfStudy;

                _dbContext.Courses.Update(existingCourse);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Course updated successfully: {CourseId}", course.Id);
                response.Status = true;
                response.Message = "Course updated successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during course update for CourseId: {CourseId}", course.Id);
                response.Status = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        public async Task<ResponseModel> DeleteCourse(int id)
        {
            var response = new ResponseModel();

            try
            {
                _logger.LogInformation("Attempting to delete course: {CourseId}", id);

                var existingCourse = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id);
                if (existingCourse == null)
                {
                    _logger.LogWarning("Course deletion failed - Course not found: {CourseId}", id);
                    response.Status = false;
                    response.Message = "Course not found.";
                    return response;
                }

                _dbContext.Courses.Remove(existingCourse);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Course deleted successfully: {CourseId}", id);
                response.Status = true;
                response.Message = "Course deleted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during course deletion for CourseId: {CourseId}", id);
                response.Status = false;
                response.Message = $"An error occurred: {ex.Message}";
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

