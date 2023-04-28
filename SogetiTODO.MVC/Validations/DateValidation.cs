using System.ComponentModel.DataAnnotations;

namespace SogetiTODO.Validations;

public class DateValidation : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        DateTime date;
        if (!DateTime.TryParse(value.ToString(), out date)) return false;

        if (date <= DateTime.Now || date >= DateTime.Now.AddYears(2)) return false;

        return true;
    }
}