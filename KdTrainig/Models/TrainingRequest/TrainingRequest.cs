namespace KdTrainig.Models.TrainingRequest;

public class TrainingRequest
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime Date { get; set; }
}