namespace StudentDB.Services
{
	using Microsoft.AspNetCore.Components;
	using System.Threading.Tasks;

	public class NotifyService : INotify
	{
		private readonly NavigationManager _navigationManager;

		public NotifyService(NavigationManager navigationManager)
		{
			_navigationManager = navigationManager;
		}

		public Task InvokeAsync()
		{
			Console.WriteLine("Notification invoked");

			// Redirect to the students page
			_navigationManager.NavigateTo("/students");

			return Task.CompletedTask;
		}
	}
}
