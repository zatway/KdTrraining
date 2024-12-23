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
            
            base.OnModelCreating(modelBuilder);
        }
        
        public void SeedData(ApplicationDbContext context)
{
    // Создаем роли
    var roles = new List<Role>
    {
        new Role { Name = "Admin" },
        new Role { Name = "Employee" },
        new Role { Name = "Manager" }
    };
    context.Roles.AddRange(roles);
    context.SaveChanges();

    // Создаем пользователей
    var users = new List<User>
    {
        new User { Username = "admin", PasswordHash = "hashedpassword1", RoleId = 1 },
        new User { Username = "employee1", PasswordHash = "hashedpassword2", RoleId = 2 },
        new User { Username = "manager1", PasswordHash = "hashedpassword3", RoleId = 3 }
    };
    context.Users.AddRange(users);
    context.SaveChanges();

    // Создаем сотрудников
    var employees = new List<Employee>
    {
        new Employee { FullName = "Ivan Ivanov", Position = "Engineer", HireDate = DateTime.Now, UserId = users[0].Id },
        new Employee { FullName = "Petr Petrov", Position = "Technician", HireDate = DateTime.Now, UserId = users[1].Id },
        new Employee { FullName = "Sidor Sidorov", Position = "Manager", HireDate = DateTime.Now, UserId = users[2].Id }
    };
    context.Employees.AddRange(employees);
    context.SaveChanges();

    // Создаем оборудование
    var equipments = new List<Equipment>
    {
        new Equipment { Name = "Turbine A", Manufacturer = "CompanyX", Status = "Operational", InstallationDate = DateTime.Now.AddYears(-2) },
        new Equipment { Name = "Pump B", Manufacturer = "CompanyY", Status = "Under Maintenance", InstallationDate = DateTime.Now.AddYears(-1) },
        new Equipment { Name = "Compressor C", Manufacturer = "CompanyZ", Status = "Operational", InstallationDate = DateTime.Now.AddYears(-3) }
    };
    context.Equipments.AddRange(equipments);
    context.SaveChanges();

    // Создаем техническое обслуживание оборудования
    var maintenances = new List<Maintenance>
    {
        new Maintenance { EquipmentId = equipments[0].Id, MaintenanceDate = DateTime.Now.AddMonths(-1), PerformedBy = employees[1].Id, Description = "Scheduled maintenance" },
        new Maintenance { EquipmentId = equipments[1].Id, MaintenanceDate = DateTime.Now.AddMonths(-2), PerformedBy = employees[0].Id, Description = "Replaced valve" },
        new Maintenance { EquipmentId = equipments[2].Id, MaintenanceDate = DateTime.Now.AddMonths(-3), PerformedBy = employees[1].Id, Description = "Repaired compressor" }
    };
    context.Maintenances.AddRange(maintenances);
    context.SaveChanges();

    // Создаем тренинги
    var trainings = new List<Training>
    {
        new Training { Title = "Vibration Monitoring Training", Description = "Training on vibration diagnostics", Date = DateTime.Now.AddMonths(1) },
        new Training { Title = "Automation Systems Training", Description = "Training on automation systems", Date = DateTime.Now.AddMonths(2) }
    };
    context.Trainings.AddRange(trainings);
    context.SaveChanges();

    // Создаем записи о сотрудниках на тренингах
    var trainingEmployees = new List<TrainingEmployee>
    {
        new TrainingEmployee { TrainingId = trainings[0].Id, EmployeeId = employees[0].Id, RegistrationDate = DateTime.Now.AddMonths(-1) },
        new TrainingEmployee { TrainingId = trainings[1].Id, EmployeeId = employees[1].Id, RegistrationDate = DateTime.Now.AddMonths(-2) }
    };
    context.TrainingEmployees.AddRange(trainingEmployees);
    context.SaveChanges();

    // Создаем сертификаты для сотрудников
    var certificates = new List<Certificate>
    {
        new Certificate { TrainingEmployeeId = trainingEmployees[0].Id, CertificateName = "Vibration Monitoring Specialist" },
        new Certificate { TrainingEmployeeId = trainingEmployees[1].Id, CertificateName = "Automation Systems Specialist" }
    };
    context.Certificates.AddRange(certificates);
    context.SaveChanges();

    // Создаем отчеты
    var reports = new List<Report>
    {
        new Report { Title = "Annual Equipment Maintenance Report", FilePath = "/reports/maintenance1.pdf", CreatedBy = users[0].Id },
        new Report { Title = "Employee Training Progress", FilePath = "/reports/training_progress.pdf", CreatedBy = users[1].Id }
    };
    context.Reports.AddRange(reports);
    context.SaveChanges();
}

    }