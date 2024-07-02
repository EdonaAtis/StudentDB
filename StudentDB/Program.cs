using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentDB.Services.Interfaces;
using StudentDB.Services;
using Student.Logic.Services.Interfaces;
using Student.Logic.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DConnection")));

        // Add Identity services
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // Configure authentication and authorization
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("SuperAdminPolicy", policy =>
                policy.RequireRole("SuperAdmin"));
        });

        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

        // Add HttpClient service for IAdminPanelService
        var apiBaseAddress = builder.Configuration["ApiBaseAddress"];
        if (string.IsNullOrEmpty(apiBaseAddress))
        {
            throw new InvalidOperationException("ApiBaseAddress configuration is missing.");
        }
        builder.Services.AddHttpClient<IAdminPanelService, AdminPanelService>(client =>
        {
            client.BaseAddress = new Uri(apiBaseAddress);
        });

        // Register HttpClient for IAdminService
        builder.Services.AddHttpClient<IAdminService, AdminService>();

        // Register application services
        builder.Services.AddScoped<IAdminService, AdminService>();
        builder.Services.AddScoped<INotify, NotifyService>();

        // Add Blazored.SessionStorage
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

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });

        app.Run();
    }
}
