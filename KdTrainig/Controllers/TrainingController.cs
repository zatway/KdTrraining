namespace KdTrainig.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using KdTrainig.Db;
using KdTrainig.Models;

[ApiController]
[Route("api/[controller]")]
public class TrainingController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TrainingController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize] // Доступ для всех авторизованных пользователей
    public IActionResult GetTrainings()
    {
        return Ok(_context.Trainings.ToList());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // Доступ только для администраторов
    public IActionResult CreateTraining(Training training)
    {
        _context.Trainings.Add(training);
        _context.SaveChanges();
        return Ok(training);
    }
}
