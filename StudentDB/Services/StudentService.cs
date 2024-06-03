using Student.DataModels.Models;

namespace StudentDB.Services
{
	public class StudentService
	{
		private readonly ApplicationDbContext _applicationDbContext;
		public StudentService(ApplicationDbContext applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
		}

		public ApplicationDbContext Get_applicationDbContext()
		{
			return _applicationDbContext;
		}

	}
}
