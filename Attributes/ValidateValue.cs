using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.Json;

namespace InventorySystem.Attributes
{
    public class ValidateValue(string section) : ValidationAttribute
    {
        private readonly string _jsonFilePath = GetJsonFilePath();
        private readonly string _section = section.ToLower();

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string selectedOption || string.IsNullOrEmpty(selectedOption))
            {
                return new ValidationResult($"{validationContext.DisplayName} value is invalid");
            }

            if (IfNotSelected(selectedOption))
            {
                return new ValidationResult($"Please select {validationContext.DisplayName}");
            }

            try
            {
                if (!IsValueInSection(selectedOption, _section))
                {
                    return new ValidationResult($"The value '{selectedOption}' is not in the '{_section}' section of the JSON file.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (for debugging purposes)
                Debug.WriteLine($"Error in {nameof(ValidateValue)}: {ex.Message}");

                // Return a general error message to the user
                return new ValidationResult("An error occurred while validating the value.");
            }

            return ValidationResult.Success;
        }

        private static string GetJsonFilePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "validValues.json");
        }

        private static bool IfNotSelected(string selectedOption)
        {
            return selectedOption.Contains("--Select Status--");
        }

        private bool IsValueInSection(string value, string sectionname)
        {
            try
            {
                var jsonData = File.ReadAllText(_jsonFilePath);
                var jsonDoc = JsonDocument.Parse(jsonData);

                if (!jsonDoc.RootElement.TryGetProperty(_section, out var section))
                {
                    throw new ArgumentException($"The section '{sectionname}' does not exist in the JSON file.", nameof(sectionname));
                }

                return section.EnumerateObject().Any(option =>
                    option.Value.GetProperty("Value").GetString() == value
                );
            }
            catch (FileNotFoundException ex)
            {
                // Log and handle file not found exception
                Debug.WriteLine($"File not found: {ex.Message}");
                throw new FileNotFoundException("The JSON configuration file is missing.", ex);
            }
            catch (JsonException ex)
            {
                // Log and handle JSON parsing exception
                Debug.WriteLine($"JSON parsing error: {ex.Message}");
                throw new JsonException("Error parsing the JSON file.", ex);
            }
            catch (ArgumentException ex)
            {
                // Provide a more specific error for argument exceptions
                Debug.WriteLine($"Argument error: {ex.Message}");
                throw; // Re-throw the original exception
            }
            catch (Exception ex)
            {
                // Log and handle any other exceptions
                Debug.WriteLine($"Unexpected error: {ex.Message}");
                throw new InvalidOperationException("An unexpected error occurred.", ex);
            }
        }
    }
}
