using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KdTrainig.Models;

public class Equipment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Manufacturer { get; set; }
    public string Status { get; set; } = null!;
    public DateTime? InstallationDate { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public ICollection<Maintenance>? Maintenances { get; set; }
}