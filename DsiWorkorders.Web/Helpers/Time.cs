using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DsiWorkorders.Web.Helpers
{
    public static class Time
    {
        public static DateTime ToCentralTime(this DateTime dateTime)
        {
            return GetDateTimeByZone(dateTime, "Central Standard Time");
        }

        public static DateTime GetDateTimeByZone(DateTime datetime, string zone)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(zone);

            return TimeZoneInfo.ConvertTimeFromUtc(datetime, timeZoneInfo);
        }
    }
}