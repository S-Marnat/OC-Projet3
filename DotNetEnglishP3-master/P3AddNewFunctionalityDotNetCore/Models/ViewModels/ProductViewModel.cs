using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Required(
            ErrorMessageResourceType = typeof(P3.Resources.Models.Product),
            ErrorMessageResourceName = "MissingName"
        )]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

        [DataType(DataType.Text)]
        public string Details { get; set; }

        [DataType(DataType.Text)]
        [Required(
            ErrorMessageResourceType = typeof(P3.Resources.Models.Product),
            ErrorMessageResourceName = "MissingStock"
        )]
        [RegularExpression(@"^\d+$",
        ErrorMessageResourceType = typeof(P3.Resources.Models.Product),
        ErrorMessageResourceName = "StockNotAnInteger")]
        [Range(1, int.MaxValue,
        ErrorMessageResourceType = typeof(P3.Resources.Models.Product),
        ErrorMessageResourceName = "StockNotGreaterThanZero")]
        public string Stock { get; set; }

        [DataType(DataType.Currency)]
        [Required(
            ErrorMessageResourceType = typeof(P3.Resources.Models.Product),
            ErrorMessageResourceName = "MissingPrice"
        )]
        // A revoir, ne fonctionne pas pour les nombres décimaux
        [RegularExpression(@"^\d+([\.,]\d{1,2})?$",
        ErrorMessageResourceType = typeof(P3.Resources.Models.Product),
        ErrorMessageResourceName = "PriceNotANumber")]
        [Range(0.01, double.MaxValue,
        ErrorMessageResourceType = typeof(P3.Resources.Models.Product),
        ErrorMessageResourceName = "PriceNotGreaterThanZero")]
        public string Price { get; set; }
    }
}
