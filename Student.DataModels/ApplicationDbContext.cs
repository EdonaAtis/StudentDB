using System;
using Microsoft.EntityFrameworkCore;
using Student.DataModels.Models;

namespace Student.DataModels
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }

        public virtual DbSet<StudentInfo> StudentInfo { get; set; }
        public virtual DbSet<AdminInfo> AdminInfos { get; set; } = null!;
        public DbSet<StudentCourse> StudentCourses  { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)

            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-D3DKJG2;Database=Students;Integrated Security=True;Encrypt=False;MultipleActiveResultSets=true;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

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

            modelBuilder.Entity<StudentInfo>().HasData(
                new StudentInfo { Id = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", PhoneNumber = "123-456-7890", FieldOfStudy = "Computer Science" },
                new StudentInfo { Id = 2, FirstName = "Jane", LastName = "Smith", EmailAddress = "jane.smith@example.com", PhoneNumber = "098-765-4321", FieldOfStudy = "Mathematics" });



            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "Algorithms", FieldOfStudy = "Computer Science" },
                new Course { Id = 2, Name = "Data Structures", FieldOfStudy = "Computer Science" },
                new Course { Id = 3, Name = "Programming with C#", FieldOfStudy = "Computer Science" },
                new Course { Id = 4, Name = "Calculus", FieldOfStudy = "Mathematics" },
                new Course { Id = 5, Name = "Algorithms", FieldOfStudy = "Mathematics" },
                new Course { Id = 6, Name = "Algebra", FieldOfStudy = "Mathematics" },
                new Course { Id = 7, Name = "Calculus", FieldOfStudy = "Physics" },
                new Course { Id = 8, Name = "Mechanics and Waves", FieldOfStudy = "Physics" },
                new Course { Id = 9, Name = "Organic Chemistry", FieldOfStudy = "Chemistry" },
                new Course { Id = 10, Name = "Chemical Data Analysis", FieldOfStudy = "Chemistry" },
                new Course { Id = 11, Name = "Genetics", FieldOfStudy = "Biology" },
                new Course { Id = 12, Name = "Human anatomy and physiology", FieldOfStudy = "Biology" },
                new Course { Id = 13, Name = "Linear algebra", FieldOfStudy = "Engineering" },
                new Course { Id = 14, Name = "Statistics", FieldOfStudy = "Engineering" },
                new Course { Id = 15, Name = "Introduction to Literary Genres", FieldOfStudy = "Literature" },
                new Course { Id = 16, Name = "Race in Literature.", FieldOfStudy = "Literature" },
                new Course { Id = 17, Name = "American History", FieldOfStudy = "History" },
                new Course { Id = 18, Name = "Non-Western History", FieldOfStudy = "History" },
                new Course { Id = 19, Name = "Modern Philosophy\r\n", FieldOfStudy = "Philosophy" },
                new Course { Id = 20, Name = "Ethics", FieldOfStudy = "Philosophy" });

           

        modelBuilder.Entity<AdminInfo>(entity =>
            {
                entity.ToTable("AdminInfo");

                entity.Property(e => e.Email).HasMaxLength(30).IsRequired();
                entity.Property(e => e.Password).HasMaxLength(6).IsRequired();
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.CreatedOn).HasMaxLength(25);
                entity.Property(e => e.UpdatedOn).HasMaxLength(25);
                entity.Property(e => e.LastLogin).HasMaxLength(25);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
