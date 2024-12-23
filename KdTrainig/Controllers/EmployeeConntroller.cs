using KdTrainig.Db;
using KdTrainig.EmployeeRequest.Employee;
using KdTrainig.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KdTrainig.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EmployeeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Создание нового сотрудника
    [HttpPost("add")]
    public IActionResult CreateEmployee(EmployeeReqeust employee)
    {
        if (employee == null)
        {
            return BadRequest();
        }

        var user = _context.Users.FirstOrDefault(u => u.Username == employee.UserName);
        if (user == null)
        {
            return BadRequest("Неверное имя пользователя");
        }
        var employeeFind = _context.Employees.SingleOrDefault(e => e.FullName == employee.FullName && e.Id == user.Id);
        
        if (employeeFind != null)
        {
            return BadRequest("Сотрудник существует");
        }        
        
        var newEmployee = new Employee
        {
            FullName = employee.FullName,
            HireDate = employee.HireDate,
            Position = employee.Position,
            UserId = user.Id
        };
        
        _context.Employees.Add(newEmployee);
        _context.SaveChanges();

        return Ok(employeeFind);
    }

    // Получение информации о сотруднике по ID
    [HttpGet("details/{id}")]
    public IActionResult GetEmployeeById(int id)
    {
        var employee = _context.Employees.FirstOrDefault(e => e.Id == id);

        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    // Получение всех сотрудников
    [HttpGet("list/details")]
    public IActionResult GetAllEmployees()
    {
        var employees = _context.Employees.ToList();
        return Ok(employees);
    }

    // Обновление информации о сотруднике
    [HttpPut("update/{id}")]
    public IActionResult UpdateEmployee(int id, EmployeeReqeust employeeEditInfo)
    {
        if (employeeEditInfo == null)
        {
            return BadRequest();
        }

        var employee = _context.Employees.FirstOrDefault(e => e.Id == id);
        if(employee != null)
        {
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }
        
        else
        {
            return NotFound();
        }

        var user = _context.Users.FirstOrDefault(u => u.Username == employeeEditInfo.UserName);
        
        employee.FullName = employeeEditInfo.FullName;
        employee.Position = employeeEditInfo.Position;
        employee.HireDate = employeeEditInfo.HireDate;
        employee.UserId = user.Id;

        // Обновление других полей по мере необходимости
        // Например, обновление коллекций (Maintenances, TrainingEmployees) может требовать дополнительной логики

        _context.Add(employee);
        _context.SaveChanges();

        return Ok();
    }

    // Удаление сотрудника
    [HttpDelete("{id}")]
    public IActionResult DeleteEmployee(int id)
    {
        var employee = _context.Employees.FirstOrDefault(e => e.Id == id);

        if (employee == null)
        {
            return NotFound();
        }
        
        _context.Employees.Remove(employee);
        _context.SaveChanges();

        return NoContent();
    }
}
