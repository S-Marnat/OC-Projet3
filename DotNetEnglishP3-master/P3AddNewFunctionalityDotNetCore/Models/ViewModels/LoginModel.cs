using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class LoginModel
    {
        [DataType(DataType.Text)]
        [Required(
            ErrorMessageResourceType = typeof(P3.Resources.Models.Account),
            ErrorMessageResourceName = "ErrorMissingName")]
        public string Name { get; set; }

        [DataType(DataType.Password)]
        [Required(
            ErrorMessageResourceType = typeof(P3.Resources.Models.Account),
            ErrorMessageResourceName = "ErrorMissingPassword")]
        public string Password { get; set; }

        [DataType(DataType.Url)]
        public string ReturnUrl { get; set; } = "/";
    }
}