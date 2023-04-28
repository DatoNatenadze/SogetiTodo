using System.ComponentModel.DataAnnotations;

namespace SogetiTODO.Models.RequestModels;

/// <summary>
///     Represents the request model for updating a TODO item.
/// </summary>
public class UpdateTodoRequestModel
{
    /// <summary>
    ///     The unique identifier of the TODO item.
    /// </summary>
    [Required]
    public string Id { get; set; }

    /// <summary>
    ///     Indicates whether the TODO item is completed.
    /// </summary>
    [Required]
    public bool IsCompleted { get; set; }
}