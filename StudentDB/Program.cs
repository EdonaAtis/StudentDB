using StudentDB.Services;
using Student.DataModels.CustomModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Blazored.SessionStorage;
using Microsoft.EntityFrameworkCore;
using StudentDB.Services.Interfaces;
using Student.DataModels;

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddRazorPages();
		builder.Services.AddServerSideBlazor();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DConnection")));

        // Read ApiBaseAddress from configuration
        var apiBaseAddress = builder.Configuration["ApiBaseAddress"];
		if (string.IsNullOrEmpty(apiBaseAddress))
		{
			throw new InvalidOperationException("ApiBaseAddress configuration is missing.");
		}

        builder.Services.AddHttpClient<IAdminPanelService, AdminPanelService>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:5166/");
        });
        builder.Services.AddScoped<IAdminPanelService, AdminPanelService>();
		builder.Services.AddScoped<INotify, NotifyService>(); 
		builder.Services.AddBlazoredSessionStorage();
		// Configure HttpClient with BaseAddress
		builder.Services.AddHttpClient<IAdminPanelService, AdminPanelService>(client =>
		{
			client.BaseAddress = new Uri(apiBaseAddress);
		});


		var app = builder.Build();


		if (app.Environment.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseExceptionHandler("/Error");
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();
		app.UseRouting();
		app.UseAuthentication();
		app.UseAuthorization();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id}");
		});

		app.MapControllers();
		app.MapBlazorHub();
		app.MapFallbackToPage("/_Host");

		app.Run();
	}
}