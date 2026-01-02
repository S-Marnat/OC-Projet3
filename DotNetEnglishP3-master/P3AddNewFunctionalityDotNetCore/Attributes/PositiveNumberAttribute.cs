using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class PositiveNumberAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // On laisse Required s'occuper du cas d'un champ vide
        if (value != null)
        {
            // Initialisation des variables
            string input = value.ToString();
            double parsedValue;

            // Tentative de conversion
            try
            {
                parsedValue = ParseDoubleWithAutoDecimalSeparator(input);
            }
            catch
            {
                // On laisse RegularExpression s'occuper de ce cas
                return ValidationResult.Success;
            }

            // Vérification du nombre qui doit être supérieur à 0
            if (parsedValue < 0.01)
                return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
        }

        return ValidationResult.Success;
    }

    private static double ParseDoubleWithAutoDecimalSeparator(string input)
    {
        if (input.Contains(','))
        {
            input = input.Replace(',', '.');
        }

        return double.Parse(input, CultureInfo.InvariantCulture);
    }
}