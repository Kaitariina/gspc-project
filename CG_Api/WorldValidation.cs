using System.ComponentModel.DataAnnotations;

public class WorldValidation : ValidationAttribute
{
    public string GetErrorMessage() => $"A world must exist before beginning a new session";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
            return new ValidationResult(GetErrorMessage());

        return ValidationResult.Success;
    }
}