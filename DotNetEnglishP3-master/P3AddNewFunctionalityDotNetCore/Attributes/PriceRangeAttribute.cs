using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

public class PriceRangeAttributes : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // Vérification de la présence d'un champ vide
        if (value == null)
            return new ValidationResult(ErrorMessage);

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
            // Renvoyer une erreur en cas d'échec
            return new ValidationResult(ErrorMessage);
        }

        // Vérification du nombre qui doit être supérieur à 0
        if (parsedValue < 0.01)
            return new ValidationResult(ErrorMessage);

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