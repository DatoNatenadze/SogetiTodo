using System.ComponentModel.DataAnnotations;
using SogetiTODO.Validations;

namespace SogetiTODO.Models.RequestModels;

/// <summary>
///     Represents the request model for creating a new TODO item.
/// </summary>
public class CreateTodoRequestModel
{
    /// <summary>
    ///     The title of the TODO item.
    /// </summary>

    [Required(ErrorMessageResourceType = typeof(ValidationMessages),
        ErrorMessageResourceName = "RequiredValidationError")]
    public string Title { get; set; }

    /// <summary>
    ///     The description of the TODO item.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    ///     The finish until date of the TODO item.
    /// </summary>
    [DateValidation(ErrorMessageResourceType = typeof(ValidationMessages),
        ErrorMessageResourceName = "DateValidationError")]
    public DateTime FinishUntil { get; set; }
}