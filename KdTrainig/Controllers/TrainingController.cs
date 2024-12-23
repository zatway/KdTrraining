using KdTrainig.Db;
using KdTrainig.Models;
using KdTrainig.Models.TrainingRequest;
using Microsoft.AspNetCore.Mvc;

namespace KdTrainig.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainingController : ControllerBase
{
     private readonly ApplicationDbContext _context;

    public TrainingController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("add")]
    public IActionResult CreateTraining(TrainingRequest training)
    {
        if (training == null)
        {
            return BadRequest();
        }
        
        var employeeFind = _context.Trainings.SingleOrDefault(e => e.Title == training.Title && e.Description == training.Description);
        
        if (employeeFind != null)
        {
            return BadRequest("Сотрудник существует");
        }        
        
        var newTraining = new Training
        {
            Title = training.Title,
            Description = training.Description,
            Date = training.Date,
        };
        
        _context.Trainings.Add(newTraining);
        _context.SaveChanges();

        return Ok(employeeFind);
    }

    [HttpGet("details/{id}")]
    public IActionResult GetTrainingById(int id)
    {
        var training = _context.Trainings.FirstOrDefault(e => e.Id == id);

        if (training == null)
        {
            return NotFound();
        }

        return Ok(training);
    }

    [HttpGet("list/details")]
    public IActionResult GetAllTraininngs()
    {
        var trainings = _context.Trainings.ToList();
        return Ok(trainings);
    }

    // Обновление информации о сотруднике
    [HttpPut("update/{id}")]
    public IActionResult UpdateEmployee(int id, TrainingRequest trainingEditInfo)
    {
        if (trainingEditInfo == null)
        {
            return BadRequest();
        }

        var training = _context.Trainings.FirstOrDefault(e => e.Id == id);
        if(training != null)
        {
            _context.Trainings.Remove(training);
            _context.SaveChanges();
        }
        else
        {
            return NotFound();
        }

        var user = _context.Trainings.FirstOrDefault(u => u.Title == training.Title && u.Description == training.Description);
        
        training.Title = trainingEditInfo.Title;
        training.Description = trainingEditInfo.Description;
        training.Date = trainingEditInfo.Date;

        // Обновление других полей по мере необходимости
        // Например, обновление коллекций (Maintenances, TrainingEmployees) может требовать дополнительной логики

        _context.Add(training);
        _context.SaveChanges();

        return Ok();
    }

    // Удаление сотрудника
    [HttpDelete("{id}")]
    public IActionResult DeleteEmployee(int id)
    {
        var training = _context.Trainings.FirstOrDefault(e => e.Id == id);

        if (training == null)
        {
            return NotFound();
        }
        
        _context.Trainings.Remove(training);
        _context.SaveChanges();

        return NoContent();
    }
}