﻿namespace StudentDB.Services
{
	public interface ISessionStorageService
	{
		Task SetAsync<T>(string key, T value);
		Task<T> GetAsync<T>(string key);
	}
}