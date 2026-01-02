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
        [RegularExpression(@"^(-)?\d+$",
            ErrorMessageResourceType = typeof(P3.Resources.Models.Product),
            ErrorMessageResourceName = "StockNotAnInteger")]
        [PositiveNumber(
            ErrorMessageResourceType = typeof(P3.Resources.Models.Product),
            ErrorMessageResourceName = "StockNotGreaterThanZero")]
        public string Stock { get; set; }

        [DataType(DataType.Currency)]
        [Required(
            ErrorMessageResourceType = typeof(P3.Resources.Models.Product),
            ErrorMessageResourceName = "MissingPrice"
        )]
        [RegularExpression(@"^(-)?\d+([\.,]\d{1,2})?$",
            ErrorMessageResourceType = typeof(P3.Resources.Models.Product),
            ErrorMessageResourceName = "PriceNotANumber")]
        [PositiveNumber(
            ErrorMessageResourceType = typeof(P3.Resources.Models.Product),
            ErrorMessageResourceName = "PriceNotGreaterThanZero")]
        public string Price { get; set; }
    }
}
