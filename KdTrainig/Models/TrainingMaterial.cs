using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KdTrainig.Models;

public class TrainingMaterial
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string FilePath { get; set; } = null!;
    public int TrainingId { get; set; }
    public Training Training { get; set; } = null!;
}