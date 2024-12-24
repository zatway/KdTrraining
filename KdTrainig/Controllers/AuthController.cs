using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KdTrainig.Db;
using KdTrainig.Models;
using KdTrainig.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        // Проверка на существование пользователя
        if (_context.Users.Any(u => u.Username == request.Username))
        {
            return BadRequest("Пользователь уже существует.");
        }
        
        var role = _context.Roles.FirstOrDefault(r => r.Name == request.RoleName);
        
        if (role == null)
        { 
            return BadRequest("Такой роли не существуе");
        }
        
        // Создание нового пользователя
        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            RoleId = role.Id
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        // Генерация JWT токена
        var token = GenerateJwtToken(user);
        return Ok(new { Token = token });
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        // Поиск пользователя
        var user = _context.Users.SingleOrDefault(u => u.Username == request.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized("Неверное имя пользователя или пароль.");
        }

        // Генерация JWT токена
        var token = GenerateJwtToken(user);
        
        return Ok(new { Token = token });
    }
    
    [HttpGet("roles/list")]
    public IActionResult GetRoles()
    {
        // Поиск пользователя
        var roles = _context.Roles.ToList();
        
        return Ok(roles);
    }
    
    [HttpPost("roles/add")]
    public IActionResult AddRoles(string Name)
    {
        // Создаем роли
        var role = new Role()
        {
            Name = Name
        };
        
        _context.Roles.AddRange(role);
        _context.SaveChanges();
        
        return Ok();
    }

    private string GenerateJwtToken(User user)
    {
        var role = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId)?.Name;
        if (role != null)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            if (key.KeySize < 256)
            {
                throw new InvalidOperationException("Key size is too short. Use a key with at least 32 characters.");
            }

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        return null;
    }
}