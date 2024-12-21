using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KdTrainig.Models;

public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string? Position { get; set; }
    public DateTime? HireDate { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public ICollection<Maintenance>? Maintenances { get; set; }
    public ICollection<TrainingEmployee>? TrainingEmployees { get; set; }
}