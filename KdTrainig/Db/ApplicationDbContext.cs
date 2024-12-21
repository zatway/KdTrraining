using KdTrainig.Models;
using KdTrainig.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace KdTrainig.Db;

public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Equipment> Equipments { get; set; } = null!;
        public DbSet<Maintenance> Maintenances { get; set; } = null!;
        public DbSet<Training> Trainings { get; set; } = null!;
        public DbSet<TrainingEmployee> TrainingEmployees { get; set; } = null!;
        public DbSet<Report> Reports { get; set; } = null!;
        public DbSet<Log> Logs { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<Certificate> Certificates { get; set; } = null!;
        public DbSet<TrainingMaterial> TrainingMaterials { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Связь User -> Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            // Связь Maintenance -> Equipment
            modelBuilder.Entity<Maintenance>()
                .HasOne(m => m.Equipment)
                .WithMany(e => e.Maintenances)
                .HasForeignKey(m => m.EquipmentId);

            // Связь Maintenance -> Employee
            modelBuilder.Entity<Maintenance>()
                .HasOne(m => m.PerformedByEmployee)
                .WithMany(e => e.Maintenances)
                .HasForeignKey(m => m.PerformedBy);

            // Связь TrainingEmployee -> Training
            modelBuilder.Entity<TrainingEmployee>()
                .HasOne(te => te.Training)
                .WithMany(t => t.TrainingEmployees)
                .HasForeignKey(te => te.TrainingId);

            // Связь TrainingEmployee -> Employee
            modelBuilder.Entity<TrainingEmployee>()
                .HasOne(te => te.Employee)
                .WithMany(e => e.TrainingEmployees)
                .HasForeignKey(te => te.EmployeeId);
            
            // Связь Report -> User
            modelBuilder.Entity<Report>()
                .HasOne(r => r.CreatedByUser)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.CreatedBy);

            // Связь Log -> User
            modelBuilder.Entity<Log>()
                .HasOne(l => l.User)
                .WithMany(u => u.Logs)
                .HasForeignKey(l => l.UserId);
            
            // Связь Certificate -> TrainingEmployee
            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.TrainingEmployee)
                .WithOne(te => te.Certificate)
                .HasForeignKey<Certificate>(c => c.TrainingEmployeeId);

            // Связь TrainingMaterial -> Training
            modelBuilder.Entity<TrainingMaterial>()
                .HasOne(tm => tm.Training)
                .WithMany(t => t.TrainingMaterials)
                .HasForeignKey(tm => tm.TrainingId);
            
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }