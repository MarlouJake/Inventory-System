using InventorySystem.Models.Accounts;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Attributes
{
    public class PasswordMatch : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var model = (RegisterModel)validationContext.ObjectInstance;

            if (model.Password != model.ConfirmPassword)
            {
                return new ValidationResult("Passwords do not match.");
            }

            return ValidationResult.Success!;
        }
    }
}
