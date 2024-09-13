using InventorySystem.Utilities.Api;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Attributes
{
    /// <summary>
    /// A custom validation attribute that ensures a string value is within a specified length range.
    /// </summary>
    public class CharacterLength : ValidationAttribute
    {
        private readonly int _minLength;
        private readonly int _maxLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterLength"/> class.
        /// </summary>
        /// <param name="minLength">The minimum length of the string.</param>
        /// <param name="maxLength">The maximum length of the string.</param>
        /// <exception cref="ArgumentException">Thrown when the minimum length is greater than the maximum length.</exception>
        public CharacterLength(int minLength, int maxLength)
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
        /// Validates the value of the property or field to ensure it meets the length constraints.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context of the validation.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the validation was successful or not.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string str)
            {
                // Call the private Validate method
                var result = Validate(str, validationContext.DisplayName);
                if (result != ValidationResult.Success)
                {
                    return result;
                }
            }
            return ValidationResult.Success;
        }

        /// <summary>
        /// Performs the length validation on the string.
        /// </summary>
        /// <param name="str">The string to validate.</param>
        /// <param name="displayName">The display name of the property or field being validated.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the validation was successful or not.</returns>
        private ValidationResult? Validate(string str, string displayName)
        {
            string validationCase = "";

            var validation = new Validation();

            // Case 1: Length check
            if (!validation.IsValidLength(str, _minLength, _maxLength))
                validationCase = "Length";

            // Switch statement to handle each validation type
            return validationCase switch
            {
                "Length" =>
                    new ValidationResult(FormatErrorMessage(displayName)),
                _ => ValidationResult.Success // All validations passed
            };
        }
    }
}
