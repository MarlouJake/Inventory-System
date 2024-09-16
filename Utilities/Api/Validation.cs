using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Utilities.Api
{
    /// <summary>
    /// Provides utility methods for various validation checks, such as null checks, space checks,
    /// length validation, and format validation.
    /// </summary>
    public class Validation
    {
        /// <summary>
        /// Checks if a string is null or empty.
        /// </summary>
        /// <param name="validate">The string to check.</param>
        /// <returns>True if the string is null or empty; otherwise, false.</returns>
        public bool IfNull(string validate)
        {
            bool isNull = string.IsNullOrEmpty(validate);
            return isNull;
        }

        /// <summary>
        /// Checks if a string contains a specific value.
        /// </summary>
        /// <param name="validate">The string to check.</param>
        /// <param name="value">The value to look for.</param>
        /// <returns>True if the string contains the specified value; otherwise, false.</returns>
        public bool HasSpace(string validate, string value)
        {
            bool hasSpace = validate.Contains(value);
            return hasSpace;
        }

        /// <summary>
        /// Validates if any of the provided strings are null or empty.
        /// </summary>
        /// <param name="validate1">The first string to check.</param>
        /// <param name="validate2">The second string to check.</param>
        /// <param name="validate3">The third string to check.</param>
        /// <returns>True if any of the strings are null or empty; otherwise, false.</returns>
        public bool ValidateNull(string validate1, string validate2, string validate3)
        {
            bool valid1 = IfNull(validate1);
            bool valid2 = IfNull(validate2);
            bool valid3 = IfNull(validate3);
            return valid1 || valid2 || valid3;
        }

        /// <summary>
        /// Validates if any of the provided strings contain a specific value.
        /// </summary>
        /// <param name="validate1">The first string to check.</param>
        /// <param name="validate2">The second string to check.</param>
        /// <param name="validate3">The third string to check.</param>
        /// <param name="contains">The value to look for.</param>
        /// <returns>True if any of the strings contain the specified value; otherwise, false.</returns>
        public bool ValidateSpaces(string validate1, string validate2, string validate3, string contains)
        {
            bool valid1 = HasSpace(validate1, contains);
            bool valid2 = HasSpace(validate2, contains);
            bool valid3 = HasSpace(validate3, contains);
            return valid1 || valid2 || valid3;
        }

        /// <summary>
        /// Checks if a string's length is within the specified range.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <param name="min">The minimum length.</param>
        /// <param name="max">The maximum length.</param>
        /// <returns>True if the string length is within the range; otherwise, false.</returns>
        public bool IsValidLength(string str, int min, int max)
        {
            return str.Length >= min && str.Length <= max;
        }

        /// <summary>
        /// Checks if a string contains any special characters.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>True if the string contains special characters; otherwise, false.</returns>
        public bool HasSpecialCharacter(string str)
        {
            return str.Any(ch => !char.IsLetterOrDigit(ch));
        }

        /// <summary>
        /// Checks if a string contains any uppercase characters.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>True if the string contains uppercase characters; otherwise, false.</returns>
        public bool HasUppercaseCharacter(string str)
        {
            return str.Any(char.IsUpper);
        }

        /// <summary>
        /// Checks if a string contains any lowercase characters.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>True if the string contains lowercase characters; otherwise, false.</returns>
        public bool HasLowercaseCharacter(string str)
        {
            return str.Any(char.IsLower);
        }

        /// <summary>
        /// Validates if an email address is in a correct format.
        /// </summary>
        /// <param name="value">The email address to validate.</param>
        /// <returns>True if the email address is valid; otherwise, false.</returns>
        public bool IsValidEmail(string value)
        {
            // Simple email validation
            return new EmailAddressAttribute().IsValid(value);
        }

        /// <summary>
        /// Checks if a number is within a specified range.
        /// </summary>
        /// <param name="value">The number to check.</param>
        /// <param name="minvalue">The minimum allowed value.</param>
        /// <param name="maxvalue">The maximum allowed value.</param>
        /// <returns>True if the number is within the range; otherwise, false.</returns>
        public bool IsValidNumberRange(int value, int minValue, int maxValue)
        {
            return value >= minValue && value <= maxValue;
        }

    }
}
