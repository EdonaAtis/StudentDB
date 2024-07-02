using Blazored.SessionStorage;
using StudentDB.Services.Interfaces;
using System.Threading.Tasks;

namespace StudentDB.Services
{
    public class SessionStorageService 
	{
		private readonly ISessionStorageService _sessionStorageService;

		public SessionStorageService(ISessionStorageService sessionStorageService)
		{
			_sessionStorageService = sessionStorageService;
		}

		public async Task SetAsync<T>(string key, T value)
		{
			await _sessionStorageService.SetItemAsync(key, value);
		}

		public async Task<T> GetAsync<T>(string key)
		{
			return await _sessionStorageService.GetItemAsync<T>(key);
		}
	}
}
