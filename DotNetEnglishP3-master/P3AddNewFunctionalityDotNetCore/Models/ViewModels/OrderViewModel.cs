using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class OrderViewModel
    {
        [BindNever]
        public int OrderId { get; set; }

        [BindNever]
        public ICollection<CartLine> Lines { get; set; }

        [DataType(DataType.Text)]
        [Required(
            ErrorMessageResourceType = typeof(P3.Resources.Models.Order),
            ErrorMessageResourceName = "ErrorMissingName"
        )]
        [RegularExpression(@"^[\p{L}][\p{L} \-']*[\p{L}]$",
            ErrorMessageResourceType = typeof(P3.Resources.Models.Order),
            ErrorMessageResourceName = "ErrorNameInvalid")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Required(
            ErrorMessageResourceType = typeof(P3.Resources.Models.Order),
            ErrorMessageResourceName = "ErrorMissingAddress"
        )]
        [RegularExpression(@"^[\p{L}\d][\p{L}\d \-',]*[\p{L}\d]$",
            ErrorMessageResourceType = typeof(P3.Resources.Models.Order),
            ErrorMessageResourceName = "ErrorAdressInvalid")]
        public string Address { get; set; }

        [DataType(DataType.Text)]
        [Required(
            ErrorMessageResourceType = typeof(P3.Resources.Models.Order),
            ErrorMessageResourceName = "ErrorMissingCity"
        )]
        [RegularExpression(@"^[\p{L}][\p{L} \-']*[\p{L}]$",
            ErrorMessageResourceType = typeof(P3.Resources.Models.Order),
            ErrorMessageResourceName = "ErrorCityInvalid")]
        public string City { get; set; }

        [DataType(DataType.PostalCode)]
        [Required(
            ErrorMessageResourceType = typeof(P3.Resources.Models.Order),
            ErrorMessageResourceName = "ErrorMissingZipCode"
        )]
        // Dans le cadre de l'exercice, je ne prends en compte que le code postal français
        [RegularExpression(@"^\d{5}$",
            ErrorMessageResourceType = typeof(P3.Resources.Models.Order),
            ErrorMessageResourceName = "ErrorZipCodeInvalid")]
        public string Zip { get; set; }

        [DataType(DataType.Text)]
        [Required(
            ErrorMessageResourceType = typeof(P3.Resources.Models.Order),
            ErrorMessageResourceName = "ErrorMissingCountry"
        )]
        [RegularExpression(@"^[\p{L}][\p{L} \-']*[\p{L}]$",
            ErrorMessageResourceType = typeof(P3.Resources.Models.Order),
            ErrorMessageResourceName = "ErrorCountryInvalid")]
        public string Country { get; set; }

        [BindNever]
        public DateTime Date { get; set; }
    }
}
