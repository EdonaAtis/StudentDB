using Student.DataModels;
using Student.DataModels.CustomModels;
using Student.DataModels.Models;
using System;
using System.Linq;

namespace Student.Logic.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _dbcontext;

        public AdminService(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public ResponseModel AdminLogin(LoginModel loginModel)
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

                var userData = _dbcontext.AdminInfos
                    .FirstOrDefault(x => x.Email == loginModel.EmailId && x.Password == loginModel.Password);

                if (userData != null)
                {
                    response.Status = true;
                    response.Message = $"{userData.Id}|{userData.Name}|{userData.Email}";
                }
                else
                {
                    response.Status = false;
                    response.Message = "Email or Password is incorrect.";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = $"An error occurred: {ex.Message}";
                // Log the exception
                Console.WriteLine($"Exception during login: {ex}");
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

                var existingUser = _dbcontext.AdminInfos.Any(x => x.Email == registerModel.EmailId);
                if (existingUser)
                {
                    response.Status = false;
                    response.Message = "User already exists.";
                    return response;
                }

                var newUser = new AdminInfo
                {
                    Email = registerModel.EmailId,
                    Password = registerModel.Password // Note: Hash the password in a real-world application
                };

                _dbcontext.AdminInfos.Add(newUser);
                _dbcontext.SaveChanges();

                response.Status = true;
                response.Message = "Registration successful!";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }
    }
}
