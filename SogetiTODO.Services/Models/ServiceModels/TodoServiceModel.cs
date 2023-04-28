namespace SogetiTODO.Services.Models.ServiceModels;

public class TodoServiceModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime FinishUntil { get; set; }
}