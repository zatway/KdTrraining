using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KdTrainig.Models;

public class Maintenance
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = null!;
    public DateTime MaintenanceDate { get; set; }
    public string? Description { get; set; }
    public int? PerformedBy { get; set; }
    public Employee? PerformedByEmployee { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}