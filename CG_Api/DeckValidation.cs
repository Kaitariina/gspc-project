using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class DeckValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var list = value; // as Deck ?

        try
        {
            if (list != null)
            {
                return ValidationResult.Success;

            }
        }
        catch
        {
            return new ValidationResult(GetErrorMessage());
        }

        return ValidationResult.Success;

    }
    public string GetErrorMessage()
    {
        return $"Player must have at least one deck.";
    }
}