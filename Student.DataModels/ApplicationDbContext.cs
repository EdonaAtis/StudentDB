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

        public virtual DbSet<StudentInfo> StudentInfo { get; set; }
        public virtual DbSet<AdminInfo> AdminInfos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-D3DKJG2;Database=Students;Integrated Security=True;Encrypt=False;MultipleActiveResultSets=true;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentInfo>().HasData(
                new StudentInfo { Id = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", PhoneNumber = "123-456-7890", FieldOfStudy = "Computer Science" },
                new StudentInfo { Id = 2, FirstName = "Jane", LastName = "Smith", EmailAddress = "jane.smith@example.com", PhoneNumber = "098-765-4321", FieldOfStudy = "Mathematics" });

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



