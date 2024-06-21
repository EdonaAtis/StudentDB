using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Student.DataModels.CustomModels;
using System.Threading.Tasks;

namespace Student.Logic.Services
{
	public interface IAdminService
	{
		ResponseModel AdminLogin(LoginModel loginModel);
        ResponseModel Register(RegisterModel registerModel);

    }
}
 