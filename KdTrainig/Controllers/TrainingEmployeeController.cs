using KdTrainig.Db;
using KdTrainig.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KdTrainig.Controllers;

public class TrainingEmployeeController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TrainingEmployeeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Client")] // Доступ для администраторов и пользователей
    public IActionResult RegisterEmployeeToTraining(int trainingId, int employeeId)
    {
        var trainingEmployee = new TrainingEmployee
        {
            TrainingId = trainingId,
            EmployeeId = employeeId,
            RegistrationDate = DateTime.UtcNow
        };

        _context.TrainingEmployees.Add(trainingEmployee);
        _context.SaveChanges();
        return Ok(trainingEmployee);
    }
}