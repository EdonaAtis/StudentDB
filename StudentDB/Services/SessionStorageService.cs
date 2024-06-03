using Blazored.SessionStorage;
using System.Threading.Tasks;

namespace StudentDB.Services
{
	public class SessionStorageService : ISessionStorageService
	{
		private readonly ISessionStorageService _sessionStorageService;

		public SessionStorageService(ISessionStorageService sessionStorageService)
		{
			_sessionStorageService = sessionStorageService;
		}

		public async Task SetAsync<T>(string key, T value)
		{
			await _sessionStorageService.SetAsync(key, value);
		}

		public async Task<T> GetAsync<T>(string key)
		{
			return await _sessionStorageService.GetAsync<T>(key);
		}
	}
}
