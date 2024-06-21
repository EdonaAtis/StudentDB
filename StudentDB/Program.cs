using StudentDB.Services;
using Blazored.SessionStorage;
using Microsoft.EntityFrameworkCore;
using StudentDB.Services.Interfaces;
using Student.DataModels;


public class Program
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

        // Add HttpClient service for IAdminPanelService
        builder.Services.AddHttpClient<IAdminPanelService, AdminPanelService>(client =>
        {
            client.BaseAddress = new Uri(apiBaseAddress);
        });

        builder.Services.AddScoped<AdminPanelService, AdminPanelService>();
        builder.Services.AddScoped<INotify, NotifyService>();
        builder.Services.AddBlazoredSessionStorage();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts(); // Use HSTS in production
        }

        app.UseHttpsRedirection(); // Ensure HTTPS redirection middleware is used
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        app.MapControllers();
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}
