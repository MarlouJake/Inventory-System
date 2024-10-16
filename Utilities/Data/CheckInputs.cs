namespace InventorySystem.Utilities.Data
{
    public class CheckInputs
    {
        public void CheckId(int? id)
        {
            bool isPositive = (id < 0);
            if (isPositive)
            {
                throw new ArgumentOutOfRangeException("Id must be a positive integer.");
            }

            if (id == null)
            {
                throw new ArgumentNullException("Id cannot be null.");
            }
        }

        public void CheckPage(int? page)
        {
            bool isPositive = (page < 0);
            if (isPositive)
            {
                throw new ArgumentOutOfRangeException("Page must be a positive integer.");
            }

            if (page == null)
            {
                throw new ArgumentNullException("Page cannot be null.");
            }

        }


    }
}
