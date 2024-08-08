using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Attributes
{
    public class ConditionalRequiredAttribute(string conditionPropertyName) : ValidationAttribute
    {
        private readonly string _conditionPropertyName = conditionPropertyName;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_conditionPropertyName);
            if (property == null)
            {
                return new ValidationResult($"Unknown property: {_conditionPropertyName}");
            }

            var conditionValue = property.GetValue(validationContext.ObjectInstance) as bool?;
            if (conditionValue == true && (value == null || string.IsNullOrWhiteSpace(value.ToString())))
            {
                return new ValidationResult(ErrorMessage ?? "This field is required.");
            }
            if (conditionValue == false && (value == null || string.IsNullOrWhiteSpace(value.ToString())))
            {
                return ValidationResult.Success;
            }

            // Ensure that a valid ValidationResult is always returned.
            return ValidationResult.Success;
        }
    }


    public class NoSpacesAttribute : ValidationAttribute
    {
        public NoSpacesAttribute() : base("The field cannot contain spaces.")
        {
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string strValue && strValue.Contains(' '))
            {
                return new ValidationResult(ErrorMessage);
            }

#pragma warning disable CS8603 // Possible null reference return.
            return ValidationResult.Success;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
