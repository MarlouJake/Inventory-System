namespace InventorySystem.Utilities
{
    public class Validation
    {

        //public static IActionResult CheckNull(object model, IActionResult result)
        //{
        //    if (model != null)
        //    {
        //        return new OkResult();
        //    }
        //    else
        //    {
        //        return result;
        //    }
        //}

        public static bool IfNull(string validate)
        {
            bool isNull = string.IsNullOrEmpty(validate);
            return isNull;
        }

        public static bool HasSpace(string validate, string value)
        {
            bool hasSpace = validate.Contains(value);
            return hasSpace;
        }

        public static bool ValidateNull(string validate1, string validate2, string validate3)
        {
            bool valid1 = IfNull(validate1);
            bool valid2 = IfNull(validate2);
            bool valid3 = IfNull(validate3);
            return valid1 || valid2 || valid3;
        }

        public static bool ValidateSpaces(string validate1, string validate2, string validate3, string contains)
        {
            bool valid1 = HasSpace(validate1, contains);
            bool valid2 = HasSpace(validate2, contains);
            bool valid3 = HasSpace(validate3, contains);
            return valid1 || valid2 || valid3;
        }
    }

}