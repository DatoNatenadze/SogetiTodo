namespace SogetiTODO.Models;

/// <summary>
///     Represents the view model for a TODO item.
/// </summary>
public class TodoViewModel
{
    /// <summary>
    ///     The unique identifier of the TODO item.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     The title of the TODO item.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    ///     The description of the TODO item.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    ///     The finish until date of the TODO item.
    /// </summary>
    public DateTime FinishUntil { get; set; }

    /// <summary>
    ///     Indicates whether the TODO item is completed.
    /// </summary>
    public bool IsCompleted { get; set; }
}