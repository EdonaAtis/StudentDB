using Microsoft.Extensions.Logging;
using Student.DataModels.CustomModels;
using Student.DataModels.Models;
using Student.Logic.Services.Interfaces;
using System.Net.Http.Json;
using Blazored.SessionStorage;
using Microsoft.EntityFrameworkCore;



namespace Student.Logic.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AdminService> _logger;

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
    

    public ResponseModel Register(RegisterModel registerModel)
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

                var existingUser = _dbContext.AdminInfos.Any(x => x.Email == registerModel.EmailId);
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

                _dbContext.AdminInfos.Add(newUser);
                _dbContext.SaveChanges();

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

        public ResponseModel CreateCourse(Course course)
        {
            var response = new ResponseModel();

            try
            {
                if (course == null)
                {
                    throw new ArgumentNullException(nameof(course));
                }

                _logger.LogInformation("Attempting to create course: {CourseName}", course.Name);

                var existingCourse = _dbContext.Courses.Any(c => c.Name == course.Name && c.FieldOfStudy == course.FieldOfStudy);
                if (existingCourse)
                {
                    _logger.LogWarning("Course creation failed - Course already exists: {CourseName}", course.Name);
                    response.Status = false;
                    response.Message = "Course already exists.";
                    return response;
                }

                _dbContext.Courses.Add(course);
                _dbContext.SaveChanges();

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

        public ResponseModel UpdateCourse(Course course)
        {
            var response = new ResponseModel();

            try
            {
                if (course == null)
                {
                    throw new ArgumentNullException(nameof(course));
                }

                _logger.LogInformation("Attempting to update course: {CourseId}", course.Id);

                var existingCourse = _dbContext.Courses.FirstOrDefault(c => c.Id == course.Id);
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
                _dbContext.SaveChanges();

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

        public ResponseModel DeleteCourse(int id)
        {
            var response = new ResponseModel();

            try
            {
                _logger.LogInformation("Attempting to delete course: {CourseId}", id);

                var existingCourse = _dbContext.Courses.FirstOrDefault(c => c.Id == id);
                if (existingCourse == null)
                {
                    _logger.LogWarning("Course deletion failed - Course not found: {CourseId}", id);
                    response.Status = false;
                    response.Message = "Course not found.";
                    return response;
                }

                _dbContext.Courses.Remove(existingCourse);
                _dbContext.SaveChanges();

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

        Task<ResponseModel> IAdminService.Register(RegisterModel registerModel)
        {
            throw new NotImplementedException();
        }

        Task<ResponseModel> IAdminService.CreateCourse(Course course)
        {
            throw new NotImplementedException();
        }

        Task<ResponseModel> IAdminService.UpdateCourse(Course course)
        {
            throw new NotImplementedException();
        }

        Task<ResponseModel> IAdminService.DeleteCourse(int courseId)
        {
            throw new NotImplementedException();
        }
    }
}
