using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Student.DataModels.Models;

public partial class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }
    public DbSet<StudentInfo> StudentInfo { get; set; }
    public DbSet<AdminInfo> AdminInfos { get; set; }
    public DbSet<StudentCourse> StudentCourses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Ensure Identity models are configured

        // Configuring Identity Tables
        modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(x => new { x.LoginProvider, x.ProviderKey });
        modelBuilder.Entity<IdentityUserRole<string>>().HasKey(x => new { x.UserId, x.RoleId });
        modelBuilder.Entity<IdentityUserToken<string>>().HasKey(x => new { x.UserId, x.LoginProvider, x.Name });

        // Configuring StudentCourse
        modelBuilder.Entity<StudentCourse>()
            .HasKey(sc => new { sc.StudentId, sc.CourseId });

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.StudentCourses)
            .HasForeignKey(sc => sc.StudentId);

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.StudentCourses)
            .HasForeignKey(sc => sc.CourseId);

        // Seed data for StudentInfo
        modelBuilder.Entity<StudentInfo>().HasData(
            new StudentInfo { Id = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", PhoneNumber = "123-456-7890", FieldOfStudy = "Computer Science" },
            new StudentInfo { Id = 2, FirstName = "Jane", LastName = "Smith", EmailAddress = "jane.smith@example.com", PhoneNumber = "098-765-4321", FieldOfStudy = "Mathematics" });

        // Seed data for Course
        modelBuilder.Entity<Course>().HasData(
            new Course { Id = 1, Name = "Algorithms", FieldOfStudy = "Computer Science" }
        );

        // Configuring AdminInfo
        modelBuilder.Entity<AdminInfo>().ToTable("AdminInfo")
            .HasKey(a => a.Id);
        modelBuilder.Entity<AdminInfo>()
            .Property(a => a.Email)
            .HasMaxLength(30)
            .IsRequired();
        modelBuilder.Entity<AdminInfo>()
            .Property(a => a.Password)
            .HasMaxLength(6)
            .IsRequired();
        modelBuilder.Entity<AdminInfo>()
            .Property(a => a.Name)
            .HasMaxLength(100);
        modelBuilder.Entity<AdminInfo>()
            .Property(a => a.CreatedOn)
            .HasMaxLength(25);
        modelBuilder.Entity<AdminInfo>()
            .Property(a => a.UpdatedOn)
            .HasMaxLength(25);
        modelBuilder.Entity<AdminInfo>()
            .Property(a => a.LastLogin)
            .HasMaxLength(25);
    }

    public static void SeedData(ApplicationDbContext context)
    {
        if (!context.AdminInfos.Any())
        {
            context.AdminInfos.Add(new AdminInfo
            {
                Email = "superadmin@example.com",
                Password = "superadmin",
                Name = "Super Admin",
                CreatedOn = DateTime.UtcNow.ToString(),
                UpdatedOn = DateTime.UtcNow.ToString(),
                LastLogin = DateTime.UtcNow.ToString()
            });

            context.SaveChanges();
        }
    }
}
