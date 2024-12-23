using KdTrainig.Db;
using KdTrainig.Models;
using KdTrainig.Models.TrainingEmployeeRequest;
using Microsoft.AspNetCore.Mvc;

namespace KdTrainig.Controllers;

public class TrainingEmployeeController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TrainingEmployeeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("add")]
    public IActionResult CreateTrainingEmployee(TrainingEmployeeRequest trainingEmployee)
    {
        if (trainingEmployee == null)
        {
            return BadRequest();
        }

        var trainingEmployeeFind =
            _context.TrainingEmployees.SingleOrDefault(e => e.EmployeeId == trainingEmployee.EmployeeId && e.TrainingId == trainingEmployee.TrainingId);

        if (trainingEmployeeFind != null)
        {
            return BadRequest("Сотрудник уже участвует в тренинге");
        }

        var newTrainingEmployee = new TrainingEmployee
        {
            EmployeeId = trainingEmployee.EmployeeId,
            TrainingId = trainingEmployee.TrainingId,
        };

        _context.TrainingEmployees.Add(newTrainingEmployee);
        _context.SaveChanges();

        return Ok(newTrainingEmployee);
    }
    
     // Получение всех сотрудников
        [HttpGet("list/details")]
        public IActionResult GetAllTrainingEmployees()
        {
            var trainingEmployees = _context.TrainingEmployees.ToList();
            return Ok(trainingEmployees);
        }
    
}