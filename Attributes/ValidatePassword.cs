using InventorySystem.Utilities.Api;
using System.ComponentModel.DataAnnotations;
namespace InventorySystem.Attributes
{
    /// <summary>
    /// A custom validation attribute that ensures a password meets specific criteria:
    /// length, special characters, uppercase letters, and lowercase letters.
    /// </summary>
    public class ValidatePassword : ValidationAttribute
    {
        private readonly int _minLength;
        private readonly int _maxLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatePassword"/> class.
        /// </summary>
        /// <param name="minLength">The minimum length of the password.</param>
        /// <param name="maxLength">The maximum length of the password.</param>
        /// <exception cref="ArgumentException">Thrown when the minimum length is greater than the maximum length.</exception>
        public ValidatePassword(int minLength, int maxLength)
        {
            if (minLength > maxLength)
            {
                throw new ArgumentException("Minimum length cannot be greater than maximum length.");
            }

            _minLength = minLength;
            _maxLength = maxLength;
        }

        /// <summary>
        /// Formats the error message for the validation attribute.
        /// </summary>
        /// <param name="name">The name of the property or field being validated.</param>
        /// <returns>A formatted error message indicating the length constraints.</returns>
        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be between {_minLength} and {_maxLength} characters.";
        }

        /// <summary>
        /// Validates the password based on the specified criteria.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context of the validation.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the validation was successful or not.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string password)
            {
                // Call the private Validate method
                var result = Validate(password, validationContext.DisplayName);
                if (result != ValidationResult.Success)
                {
                    return result;
                }
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Performs validation on the password, checking length, special characters, uppercase and lowercase letters.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <param name="displayName">The display name of the property or field being validated.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the validation was successful or not.</returns>
        private ValidationResult? Validate(string password, string displayName)
        {
            string validationCase = "";
            var validation = new Validation();

            // Case 1: Length check
            if (!validation.IsValidLength(password, _minLength, _maxLength))
                validationCase = "Length";
            // Case 2: Special character check
            else if (!validation.HasSpecialCharacter(password))
                validationCase = "SpecialCharacter";
            // Case 3: Uppercase letter check
            else if (!validation.HasUppercaseCharacter(password))
                validationCase = "Uppercase";
            // Case 4: Lowercase letter check
            else if (!validation.HasLowercaseCharacter(password))
                validationCase = "Lowercase";

            // Switch statement to handle each validation type
            return validationCase switch
            {
                "Length" =>
                    new ValidationResult(FormatErrorMessage(displayName)),

                "SpecialCharacter" =>
                    new ValidationResult($"{displayName} must contain at least one special character."),

                "Uppercase" =>
                    new ValidationResult($"{displayName} must contain at least one uppercase letter."),

                "Lowercase" =>
                    new ValidationResult($"{displayName} must contain at least one lowercase letter."),

                _ => ValidationResult.Success // All validations passed
            };
        }
    }
}
