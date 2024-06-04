using Student.DataModels;
using Student.DataModels.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Student.Logic.Services
{
    public class AdminService : IAdminService
	{

		private readonly ApplicationDbContext _dbcontext;

		public AdminService(ApplicationDbContext appDbcontext)
		{
			this._dbcontext = appDbcontext;
		}

		public ResponseModel AdminLogin(LoginModel loginModel)
		{
			ResponseModel response = new ResponseModel();

			try
			{
				
					if (_dbcontext == null)
					{
						throw new Exception("_dbcontext is null");
					}

					if (_dbcontext.AdminInfos == null)
					{
						throw new Exception("_dbcontext.AdminInfos is null");
					}

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


					var userData = _dbcontext.AdminInfos.Where(x => x.Email == loginModel.EmailId && x.Password == loginModel.Password).FirstOrDefault();

				if (userData != null)
				{
					if (userData.Password == loginModel.Password)
					{
						response.Status = true;
						response.Message = $"{userData.Id}|{userData.Name}|{userData.Email}";
					}

					else
					{
						response.Status = false;
						response.Message = "Your Password is Incorrect";
					}
				}
				else
				{
					response.Status = false;
					response.Message = "Email is not registered. Please check your Email Id";
				}

				return response;
			}

			catch (Exception)
			{
				response.Status = false;
				response.Message = "An Error has occured. Please try again!";
				return response;




			}
		}
	}
}

