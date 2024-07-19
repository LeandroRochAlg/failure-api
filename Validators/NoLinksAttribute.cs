using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace failure_api.Validators
{
    public partial class NoLinksAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success!;
            }

            string stringValue = value.ToString() ?? "";
            
            if (MyRegex().IsMatch(stringValue))
            {
                return new ValidationResult("The field cannot contain links.");
            }

            return ValidationResult.Success!;
        }

        [GeneratedRegex(@"http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?")]
        private static partial Regex MyRegex();
    }
}