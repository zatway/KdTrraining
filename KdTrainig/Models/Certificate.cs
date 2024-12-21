using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KdTrainig.Models;

public class Certificate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int TrainingEmployeeId { get; set; }
    public TrainingEmployee TrainingEmployee { get; set; } = null!;
    public string CertificateName { get; set; } = null!;
    public DateTime IssuedOn { get; set; } = DateTime.UtcNow;
}