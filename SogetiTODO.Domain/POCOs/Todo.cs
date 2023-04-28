namespace SogetiTODO.Domain.POCOs;

public class Todo
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime FinishUntil { get; set; }
}