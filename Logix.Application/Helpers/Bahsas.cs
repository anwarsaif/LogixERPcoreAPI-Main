using System;
using System.Globalization;
using Logix.Application.Common;
using Microsoft.AspNetCore.Http;

namespace Logix.Application.Helpers
{

    //  CalendarType == "1" it mean Gregorian ,,CalendarType == "2" it mean Hijri
    public static class Bahsas
    {
        private const int startGreg = 1900;
        private const int endGreg = 2100;
        private static readonly string[] allFormats = { "yyyy/MM/dd", "yyyy/M/d", "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy",
        "yyyy-MM-dd", "yyyy-M-d", "dd-MM-yyyy", "d-M-yyyy", "dd-M-yyyy", "d-MM-yyyy",
        "yyyy MM dd", "yyyy M d", "dd MM yyyy", "d M yyyy", "dd M yyyy", "d MM yyyy" };

        public static string HDateNow3(ICurrentData session)
        {
            try
            {
                if (session.CalendarType != null)
                {
                    if (session.CalendarType == "1")
                    {
                        // Gregorian Calendar with the desired format
                        var gregorianCulture = GetEnCul();
                        gregorianCulture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
                        return DateTime.Now.ToString("yyyy/MM/dd", gregorianCulture);
                    }
                    else
                    {
                        // Hijri Calendar with the desired format
                        CultureInfo hijriCulture = GetHijriCulture();
                        return DateTime.Now.ToString("yyyy/MM/dd", hijriCulture);
                    }
                }
                else
                {
                    // Default to invariant culture with desired format
                    return DateTime.Now.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                }
            }
            catch (Exception)
            {
                // Handle the exception appropriately
                return "";
            }
        }

        public static CultureInfo GetHijriCulture()
        {
            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = new UmAlQuraCalendar();
            hijriCulture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            hijriCulture.DateTimeFormat.FullDateTimePattern = "yyyy/MM/dd";
            hijriCulture.DateTimeFormat.AbbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            hijriCulture.DateTimeFormat.DayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            hijriCulture.DateTimeFormat.AbbreviatedMonthNames = new string[] { "Muh", "Saf", "Rab1", "Rab2", "Jum1", "Jum2", "Raj", "Sha", "Ram", "Shaw", "Dhu1", "Dhu2", "" };
            hijriCulture.DateTimeFormat.MonthNames = new string[] { "Muharram", "Safar", "Rabi' al-awwal", "Rabi' al-thani", "Jumada al-awwal", "Jumada al-thani", "Rajab", "Sha'ban", "Ramadan", "Shawwal", "Dhu al-Qi'dah", "Dhu al-Hijjah", "" };
            hijriCulture.DateTimeFormat.AMDesignator = "AM";
            hijriCulture.DateTimeFormat.PMDesignator = "PM";
            hijriCulture.DateTimeFormat.ShortestDayNames = new string[] { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };
            hijriCulture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Saturday;

            return hijriCulture;
        }




        public static bool IsHijri(string hijri, ICurrentData session)
        {
            if (string.IsNullOrEmpty(hijri))
            {
                return false;
            }
            try
            {
                DateTime tempDate;
                if (session.CalendarType != "1")
                    tempDate = DateTime.ParseExact(hijri, allFormats, GetArCul().DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                else
                    tempDate = DateTime.ParseExact(hijri, allFormats, GetEnCul().DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);

                return tempDate.Year >= startGreg && tempDate.Year <= endGreg;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string HijriToGreg(string hijri)
        {
            if (string.IsNullOrEmpty(hijri))
            {
                throw new ArgumentException("Invalid date format");
            }

            try
            {
                DateTime tempDate = DateTime.ParseExact(hijri, allFormats, GetArCul().DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                return tempDate.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string GregorianToHijri(string gregorian)
        {
            if (string.IsNullOrEmpty(gregorian))
            {
                throw new ArgumentException("Invalid date format");
            }

            try
            {
                DateTime gregDate = DateTime.Parse(gregorian, CultureInfo.InvariantCulture);
                int year, month, day;
                HijriCalendar hijriCalendar = new HijriCalendar();
                year = hijriCalendar.GetYear(gregDate);
                month = hijriCalendar.GetMonth(gregDate);
                day = hijriCalendar.GetDayOfMonth(gregDate);

                return $"{year}/{month.ToString("D2")}/{day.ToString("D2")}";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static CultureInfo GetArCul()
        {
            return new CultureInfo("ar-SA");
        }

        public static CultureInfo GetEnCul()
        {
            return new CultureInfo("en-US");
        }

        public static DateTime ConvertHijriStringToDate(string hijriDateStr)
        {
            UmAlQuraCalendar umAlQuraCalendar = new UmAlQuraCalendar();
            DateTimeFormatInfo dtfi = new CultureInfo("ar-SA", false).DateTimeFormat;
            dtfi.Calendar = umAlQuraCalendar;
            dtfi.ShortDatePattern = "yyyy/MM/dd";

            DateTime hijriDate = DateTime.ParseExact(hijriDateStr, "yyyy/MM/dd", dtfi);
            return hijriDate;

        }
        public static string ConvertDateToHijriString(DateTime gregorianDate)
        {
            // Create an instance of the UmAlQuraCalendar
            var umAlQuraCalendar = new System.Globalization.UmAlQuraCalendar();

            // Get the Hijri year, month, and day
            int hijriYear = umAlQuraCalendar.GetYear(gregorianDate);
            int hijriMonth = umAlQuraCalendar.GetMonth(gregorianDate);
            int hijriDay = umAlQuraCalendar.GetDayOfMonth(gregorianDate);

            // Format the Hijri date as "yyyy/MM/dd"
            return $"{hijriYear:0000}/{hijriMonth:00}/{hijriDay:00}";
        }
        public static string YearHijri(ICurrentData session)
        {
            try
            {
                // الوصول إلى الـ Session من خلال HttpContextAccessor

                var calendarType = session.CalendarType;

                // تحديد الثقافة المطلوبة
                var enCul = new CultureInfo("en-US");
                var arCul = new CultureInfo("ar-SA") { DateTimeFormat = { Calendar = new HijriCalendar() } };

                // التحقق من نوع التقويم
                if (calendarType == "1")
                {
                    return DateTime.Now.AddDays(-1).ToString("yyyy", enCul.DateTimeFormat);
                }
                else
                {
                    return DateTime.Now.AddDays(-1).ToString("yyyy", arCul.DateTimeFormat);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HDateNow: {ex.Message}");
                return "";
            }
        }
        public static long DateDiff_Day(string Date1, string Date2)
        {
            int Day1, Month1, Year1;
            int Day2, Month2, Year2;
            int DD = 0, MM, YY;

            string[] Xdate1 = Date1.Split("/");
            string[] Xdate2 = Date2.Split("/");

            Day1 = int.Parse(Xdate1[2]);
            Month1 = int.Parse(Xdate1[1]);
            Year1 = int.Parse(Xdate1[0]);

            Day2 = int.Parse(Xdate2[2]);
            Month2 = int.Parse(Xdate2[1]);
            Year2 = int.Parse(Xdate2[0]);

            YY = Year2 - Year1;

            if (Month2 < Month1)
            {
                YY--;
                MM = Month2 + 12 - Month1;
            }
            else if (Month2 == Month1 && Day2 < Day1)
            {
                YY--;
                MM = Month2 + 12 - Month1;
            }
            else
            {
                MM = Month2 - Month1;
            }

            if (Day2 < Day1)
            {
                MM--;
                DD = Day2 + 30 - Day1;
            }
            else
            {
                DD = Day2 - Day1;
            }

            return YY * 360 + MM * 30 + DD;
        }

    }
}
