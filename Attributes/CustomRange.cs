using InventorySystem.Utilities.Api;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Attributes
{
    /// <summary>
    /// A custom validation attribute that ensures a numeric value is within a specified range.
    /// </summary>
    public class CustomRange : ValidationAttribute
    {
        /// <summary>
        /// Gets the minimum value of the range.
        /// </summary>
        public int MinValue { get; }

        /// <summary>
        /// Gets the maximum value of the range.
        /// </summary>
        public int MaxValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomRange"/> class.
        /// </summary>
        /// <param name="minValue">The minimum value allowed in the range.</param>
        /// <param name="maxValue">The maximum value allowed in the range.</param>
        /// <exception cref="ArgumentException">Thrown when the minimum value is greater than the maximum value.</exception>
        public CustomRange(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentException("Minimum value cannot be greater than maximum value.");
            }

            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// Validates the value to ensure it falls within the specified range.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context of the validation.</param>
        /// <returns>A <see cref="ValidationResult"/> that indicates whether the validation was successful or not.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var validator = new Validation();
            if (value is int number)
            {
                var result = validator.IsValidNumberRange(number, MinValue, MaxValue);
                if (!result)
                    return new ValidationResult($"{validationContext.DisplayName} must be between {MinValue} and {MaxValue}");

                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid input, expected a numeric value.");
        }
    }
}
