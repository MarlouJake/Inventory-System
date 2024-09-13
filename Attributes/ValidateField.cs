using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Attributes
{
    /// <summary>
    /// A custom validation attribute that ensures a field is not null or an empty string.
    /// </summary>
    public class ValidateField : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateField"/> class.
        /// </summary>
        public ValidateField() : base("{0} is required")
        {
        }

        /// <summary>
        /// Validates that the value is neither null nor an empty string.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context of the validation.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the validation was successful or not.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Check if the value is null or an empty string
            if (value == null || (value is string strValue && string.IsNullOrWhiteSpace(strValue)))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            // If the value is valid (non-null and non-empty)
            return ValidationResult.Success;
        }

        /// <summary>
        /// Formats the error message for the validation attribute.
        /// </summary>
        /// <param name="name">The name of the property or field being validated.</param>
        /// <returns>A formatted error message indicating that the field is required.</returns>
        public override string FormatErrorMessage(string name)
        {
            // 'name' will hold the display name or property name
            return string.Format(ErrorMessageString, name);
        }
    }

    /// <summary>
    /// A custom validation attribute that ensures a string field does not contain spaces.
    /// </summary>
    public class NoSpacesAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoSpacesAttribute"/> class.
        /// </summary>
        public NoSpacesAttribute() : base("{0} must not contain spaces.")
        {
        }

        /// <summary>
        /// Validates that the string does not contain any spaces.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context of the validation.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the validation was successful or not.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string strValue && strValue.Contains(' '))
            {
                // Use validationContext.DisplayName to get the dynamic name of the field.
                var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Formats the error message for the validation attribute.
        /// </summary>
        /// <param name="name">The name of the property or field being validated.</param>
        /// <returns>A formatted error message indicating that the field must not contain spaces.</returns>
        public override string FormatErrorMessage(string name)
        {
            // The 'name' parameter will hold the display name or property name.
            return string.Format(ErrorMessageString, name);
        }
    }
}
