using System;

namespace InventorySystem.Utilities.Data
{
    public static class DateTimeExtensions
    {
        public static string GetRelativeTime(this DateTime dateTime)
        {
            return ToRelativeTime(dateTime);
        }

        private static string ToRelativeTime(this DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan < TimeSpan.FromMinutes(1))
            {
                return timeSpan.Seconds == 1 ? "1 sec ago" : $"{timeSpan.Seconds} secs ago";
            }

            if (timeSpan < TimeSpan.FromHours(1))
            {
                return timeSpan.Minutes == 1 ? "1 min ago" : $"{timeSpan.Minutes} mins ago";
            }

            if (timeSpan < TimeSpan.FromDays(1))
            {
                return timeSpan.Hours == 1 ? "1 hr ago" : $"{timeSpan.Hours} hrs ago";
            }

            if (timeSpan < TimeSpan.FromDays(30))
            {
                return timeSpan.Days == 1 ? "1 day ago" : $"{timeSpan.Days} days ago";
            }

            // Calculate months accurately
            int months = 0;
            DateTime current = dateTime;

            while (current.AddMonths(1) <= DateTime.Now)
            {
                current = current.AddMonths(1);
                months++;
            }

            if (months > 0)
            {
                return months == 1 ? "1 mo ago" : $"{months} mos ago";
            }

            // Calculate years
            int years = (DateTime.Now - dateTime).Days / 365;
            return years == 1 ? "1 yr ago" : $"{years} yrs ago";
        }
    }
}
