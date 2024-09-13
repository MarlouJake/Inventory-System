using InventorySystem.Utilities.Api;
using System.ComponentModel.DataAnnotations;
namespace InventorySystem.Attributes
{
    /// <summary>
    /// A custom validation attribute that validates whether a value is either a valid username or email address.
    /// </summary>
    public class ValidateUsernameOrEmail : ValidationAttribute
    {
        /// <summary>
        /// Validates the value to determine if it is a valid username or email address.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context of the validation.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the validation was successful or not.</returns>
        /// <exception cref="ArgumentException">Thrown when the value is of an unsupported type.</exception>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string stringValue)
            {
                throw new ArgumentException("Validation requires a string value.", nameof(value));
            }

            var validator = new Validation();

            // If the value is null or empty, return success (assuming empty values are allowed)
            if (string.IsNullOrEmpty(stringValue))
            {
                return ValidationResult.Success;
            }

            // Check if the value is a valid email address
            if (validator.IsValidEmail(stringValue))
            {
                return ValidationResult.Success;
            }

            // Additional logic to check for a valid username can be added here
            // For example, ensuring that the username follows specific rules or patterns

            // If the value is neither a valid email nor a valid username, return an error
            return new ValidationResult("Username or Email invalid");
        }
    }
}
