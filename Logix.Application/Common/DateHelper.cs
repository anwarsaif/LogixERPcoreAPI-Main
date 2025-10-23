using System.Globalization;
using Logix.Application.Interfaces.IRepositories;
namespace Logix.Application.Common
{
    /// <summary>
    /// Utility helpers for parsing and formatting dates used across the application.
    /// The class contains helpers for converting between string representations and <see cref="DateTime"/>,
    /// basic calendar lookups via the repository, and simple date arithmetic utilities.
    /// </summary>
    public static class DateHelper
    {
        /// <summary>
        /// Repository manager used by methods that require database lookups (e.g. calendar conversions).
        /// This must be initialized by calling <see cref="Initialize(IMainRepositoryManager)"/> before use.
        /// </summary>
        public static IMainRepositoryManager mainRepositoryManager { get; set; }

        /// <summary>
        /// Arabic culture info used for Hijri/Gregorian conversions (default intentionally set for compatibility).
        /// </summary>
        public static CultureInfo arCul { get; set; } = new CultureInfo("en-US");

        /// <summary>
        /// English culture info used for conversions.
        /// </summary>
        public static CultureInfo enCul { get; set; } = new CultureInfo("ar-SA");

        /// <summary>
        /// Initializes the helper with an instance of <see cref="IMainRepositoryManager"/>.
        /// Call this at application startup if methods that access the repository will be used.
        /// </summary>
        /// <param name="_mainRepositoryManager">The repository manager to use for calendar lookups.</param>
        public static void Initialize(IMainRepositoryManager _mainRepositoryManager)
        {
            // Store the provided repository manager for use by async lookup helpers.
            mainRepositoryManager = _mainRepositoryManager;
        }

        // Created By Mohammed ALshrik in 2025_07_14
        /// <summary>
        /// Safely parses a date string using a small set of accepted formats. Returns null when parsing fails.
        /// </summary>
        /// <param name="dateString">Input date string (accepted: yyyy/MM/dd, yyyy-MM-dd, dd/MM/yyyy).</param>
        /// <returns>A <see cref="DateTime"/> if parsed successfully; otherwise null.</returns>
        public static DateTime? SafeParseDate(string dateString)
        {
            // Handle null/empty input quickly.
            if (string.IsNullOrWhiteSpace(dateString))
                return null;

            string[] formats = { "yyyy/MM/dd", "yyyy-MM-dd", "dd/MM/yyyy" };

            // Try parse with exact formats using invariant culture.
            if (DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Formats a <see cref="DateTime"/> as yyyy/MM/dd using the current culture.
        /// </summary>
        public static string DateToString(DateTime date)
        {
            return date.ToString("yyyy/MM/dd");
        }

        /// <summary>
        /// Formats a <see cref="DateTime"/> as yyyy/MM/dd using the provided culture info.
        /// </summary>
        public static string DateToString(DateTime date, CultureInfo culture)
        {
            return date.ToString("yyyy/MM/dd", culture);
        }

        /// <summary>
        /// Formats a <see cref="DateTime"/> as yyyy-MM-dd using the provided culture info.
        /// </summary>
        public static string DateDashToString(DateTime date, CultureInfo culture)
        {
            return date.ToString("yyyy-MM-dd", culture);
        }

        /// <summary>
        /// Looks up the Gregorian date that corresponds to the provided Hijri date string using the calendar repository.
        /// </summary>
        /// <param name="DateH">Hijri date string to look up.</param>
        /// <returns>Gregorian date string or empty string when not found or on error.</returns>
        public static async Task<string> DateFormattYYYYMMDD_H_G(string DateH)
        {
            try
            {
                // Query the repository for the Gregorian date matching the provided Hijri date.
                var getDate = await mainRepositoryManager.SysCalendarRepository.GetOne(x => x.GDate, x => x.HDate == DateH);
                return getDate ?? "";
            }
            catch (Exception)
            {
                // Returning a clear error message keeps behavior consistent with original code.
                return "Error In DateFormattYYYYMMDD_H_G Function.";
            }
        }

        /// <summary>
        /// Converts a Hijri date string in yyyy/MM/dd (or similar) to a normalized yyyy/MM/dd Gregorian-like string using adjustment rules.
        /// </summary>
        /// <param name="dateH">Hijri date string to convert.</param>
        /// <returns>Normalized date string in yyyy/MM/dd format.</returns>
        public static string DateFormattYYYYMMDD_H_G2(string dateH)
        {
            // Extract year, month, and day from the input string using substring positions.
            string year = dateH.Substring(0, 4);
            string month = dateH.Substring(5, 2);
            string day = dateH.Substring(8, 2);

            // Apply corrective adjustments for certain day/month combinations.
            if ((day == "30" && month == "06") || (day == "30" && month == "02") || (day == "30" && month == "04") ||
                (day == "30" && month == "08") || (day == "30" && month == "10") || (day == "31" && month == "10") ||
                (day == "30" && month == "12") || (day == "31" && month == "12") || (day == "30" && month == "11"))
            {
                day = "29";
            }

            // Reconstruct and parse the adjusted date; then return in normalized yyyy/MM/dd format.
            string newDateHi = $"{year}/{day}/{month}";
            DateTime newDate = DateTime.ParseExact(newDateHi, "yyyy/dd/MM", CultureInfo.InvariantCulture);

            return $"{newDate.Year}/{newDate.Month.ToString("00")}/{newDate.Day.ToString("00")}";
        }

        /// <summary>
        /// Looks up the Hijri date that corresponds to the provided Gregorian date string using the calendar repository.
        /// </summary>
        /// <param name="DateG">Gregorian date string to look up.</param>
        public static async Task<string> DateFormattYYYYMMDD_G_H(string DateG)
        {
            try
            {
                var getDate = await mainRepositoryManager.SysCalendarRepository.GetOne(x => x.HDate, x => x.GDate == DateG);
                return getDate ?? "";
            }
            catch (Exception)
            {
                return "Error In DateFormattYYYYMMDD_G_H Function.";
            }
        }

        /// <summary>
        /// Returns the Gregorian date for the provided date (searching either GDate or HDate in the calendar table).
        /// </summary>
        public static async Task<string> DateGregorian(string Date)
        {
            try
            {
                var getDate = await mainRepositoryManager.SysCalendarRepository.GetOne(x => x.GDate, x => x.GDate == Date || x.HDate == Date);
                return getDate ?? "";
            }
            catch (Exception)
            {
                return "Error In DateGregorian Function.";
            }
        }

        /// <summary>
        /// Normalizes a supplied Gregorian date string by checking and adjusting certain day/month edge-cases.
        /// </summary>
        public static string DateFormattYYYYMMDD_G_H2(string dateG)
        {
            // Splitting the date string into year, month, and day components
            string[] parts = dateG.Split('/');

            if (parts.Length != 3)
                throw new ArgumentException("Invalid date format");

            string year = parts[0];
            string month = parts[1];
            string day = parts[2];

            // Adjust the day for specific month/day combinations to avoid invalid dates.
            if ((day == "30" && (month == "06" || month == "02" || month == "04" || month == "08" || month == "10" || month == "12")) ||
                (day == "31" && (month == "10" || month == "12")) ||
                (day == "30" && month == "11"))
            {
                day = "29";
            }

            // Return normalized string with zero-padded month/day.
            return $"{year}/{month.PadLeft(2, '0')}/{day.PadLeft(2, '0')}";
        }

        /// <summary>
        /// Calculates the inclusive number of calendar entries (days) between two date strings using the calendar repository.
        /// Requires <see cref="Initialize(IMainRepositoryManager)"/> to be called first.
        /// </summary>
        public async static Task<int> DateDiff_day2(string SDate, string EDate)
        {
            try
            {
                // Retrieve calendar records for the relevant years to limit the dataset.
                var getData = await mainRepositoryManager.SysCalendarRepository.GetAll(x => x.GDate != null && (x.GDate.Contains(SDate.Substring(0, 4)) || x.GDate.Contains(EDate.Substring(0, 4))));

                // Filter the results to the requested range using the helper StringToDate for comparisons.
                var getDatacOUNT = getData.AsEnumerable()
                    .Where(x => x.GDate != null && DateHelper.StringToDate(x.GDate) >= DateHelper.StringToDate(SDate) && DateHelper.StringToDate(x.GDate) <= DateHelper.StringToDate(EDate)
                             );
                return getDatacOUNT.Count();
            }
            catch (Exception)
            {
                // Re-throwing preserves the original behavior; consider logging for diagnostics.
                throw;
            }
        }

        /// <summary>
        /// Removes common bidi/formatting Unicode characters and trims the input string.
        /// Useful when dates are copied from environments that insert directional markers.
        /// </summary>
        public static string CleanDate(string date)
        {
            date = date.Trim();
            // Characters that can appear in strings copied from RTL/Unicode-aware editors.
            char[] bidiCharacters = new char[] { '\u200E', '\u200F', '\u202A', '\u202B', '\u202C', '\u202D', '\u202E', '\u2066', '\u2067', '\u2068', '\u2069' };
            foreach (char bidiChar in bidiCharacters)
            {
                date = date.Replace(bidiChar.ToString(), "");
            }
            return date;
        }

        /// <summary>
        /// Computes a rough day-difference between two dates supplied as yyyy/MM/dd strings using a 360-day/year convention.
        /// </summary>
        public static int DateDiff_day(string oldDate, string newDate)
        {
            // Parse date components and clean bidi characters if present.
            string[] dateParts = oldDate.Split('/');
            string[] newDateParts = newDate.Split('/');

            int d1 = int.Parse(CleanDate(dateParts[2]));
            int m1 = int.Parse(CleanDate(dateParts[1]));
            int y1 = int.Parse(CleanDate(dateParts[0]));
            int d2 = int.Parse(CleanDate(newDateParts[2]));
            int m2 = int.Parse(CleanDate(newDateParts[1]));
            int y2 = int.Parse(CleanDate(newDateParts[0]));

            int years = y2 - y1;

            // The method uses a 360-day-year business convention: months are treated as 30 days.
            if (m2 < m1)
            {
                years--;
                int months = (m2 + 12) - m1;
                if (d2 < d1)
                {
                    months--;
                    int days = (d2 + 30) - d1;
                    return days + (months * 30) + (years * 360);
                }
                else
                {
                    int days = d2 - d1;
                    return days + (months * 30) + (years * 360);
                }
            }
            else
            {
                int months = m2 - m1;
                if (d2 < d1)
                {
                    months--;
                    int days = (d2 + 30) - d1;
                    return days + (months * 30) + (years * 360);
                }
                else
                {
                    int days = d2 - d1;
                    return days + (months * 30) + (years * 360);
                }
            }
        }

        /// <summary>
        /// Validates a date string based on the specified calendar type and basic format checks.
        /// CalendarType "1" expects Gregorian-like years (1900-2100), otherwise expects Hijri-like years (1300-1500).
        /// </summary>
        public static async Task<bool> CheckDate(string curDate, long FacilityId, string CalendarType)
        {
            bool ret = false;
            try
            {
                // Validate year range depending on calendar type.
                int year = int.Parse(curDate.Substring(0, 4));
                if (CalendarType == "1")
                {
                    if (year >= 1900 && year <= 2100)
                        ret = true;
                    else
                        return false;
                }
                else
                {
                    if (year >= 1300 && year <= 1500)
                        ret = true;
                    else
                        return false;
                }

                // Basic month/day validation and slash position checks.
                int month = int.Parse(curDate.Substring(5, 2));
                if (month < 1 || month > 12)
                    return false;

                int day = int.Parse(curDate.Substring(8, 2));
                if (day < 1 || day > 31)
                    return false;

                if (curDate[4] != '/' || curDate[7] != '/')
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid date specified", ex);
            }

            return ret;
        }

        /// <summary>
        /// Validates a Hijri date string with compact checks for year/month/day ranges and slash placement.
        /// </summary>
        public static async Task<bool> CheckDateH(string curDate)
        {
            bool ret = false;
            try
            {
                int year = int.Parse(curDate.Substring(0, 4));
                int month = int.Parse(curDate.Substring(5, 2));
                int day = int.Parse(curDate.Substring(8, 2));
                char slash1 = curDate[4];
                char slash2 = curDate[7];

                if (year >= 1300 && year <= 1500 && month >= 1 && month <= 12 && day >= 1 && day <= 31 && slash1 == '/' && slash2 == '/')
                {
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                ret = false;
                throw new Exception("التاريخ المحدد غير صالح", ex);
            }

            return ret;
        }

        /// <summary>
        /// Attempts to parse various common date formats and returns a normalized yyyy/MM/dd string, or null if parsing fails.
        /// </summary>
        public static string? FixConvertDateFormate(string str_date)
        {
            DateTime date;
            if (
                DateTime.TryParseExact(str_date, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
                || DateTime.TryParseExact(str_date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
                || DateTime.TryParseExact(str_date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
                || DateTime.TryParseExact(str_date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
           )
            {
                // Successfully parsed into 'date'
            }
            else
            {
                return null;
            }

            return date.ToString("yyyy/MM/dd");
        }

        /// <summary>
        /// Returns the English month name for the provided month number (1-12).
        /// </summary>
        public static string GetMonthName(int month)
        {
            try
            {
                if (month < 1 || month > 12)
                {
                    throw new ArgumentException($"Invalid Month Number");
                }
                switch (month)
                {
                    case 1:
                        return "January";
                    case 2:
                        return "February";
                    case 3:
                        return "March";
                    case 4:
                        return "April";
                    case 5:
                        return "May";
                    case 6:
                        return "June";
                    case 7:
                        return "July";
                    case 8:
                        return "August";
                    case 9:
                        return "September";
                    case 10:
                        return "October";
                    case 11:
                        return "November";
                    case 12:
                        return "December";
                    default:
                        throw new ArgumentException($"Invalid Month Number");
                }
            }
            catch (Exception exp)
            {
                throw new ArgumentException($"Exception in : {exp.Message.ToString()}");
            }
        }

        /// <summary>
        /// Returns the Arabic month name for the provided month number (1-12).
        /// </summary>
        public static string GetArMonthName(int month)
        {
            try
            {
                if (month < 1 || month > 12)
                {
                    throw new ArgumentException($"Invalid Month Number");
                }
                switch (month)
                {
                    case 1:
                        return "يناير";
                    case 2:
                        return "فبراير";
                    case 3:
                        return "مارس";
                    case 4:
                        return "أبريل";
                    case 5:
                        return "مايو";
                    case 6:
                        return "يونيو";
                    case 7:
                        return "يوليو";
                    case 8:
                        return "أغسطس";
                    case 9:
                        return "سبتمبر";
                    case 10:
                        return "أكتوبر";
                    case 11:
                        return "نوفمبر";
                    case 12:
                        return "ديسمبر";
                    default:
                        throw new ArgumentException($"Invalid Month Number");
                }
            }
            catch (Exception exp)
            {
                throw new ArgumentException($"Exception in : {exp.Message.ToString()}");
            }
        }

        /// <summary>
        /// Formats a date string xDate (yyyy/MM/dd) and appends the provided time in HH:mm form to produce a timestamp.
        /// </summary>
        public static string ChangeFormatDate(string xDate, string time)
        {
            try
            {
                // Extract year, month, and day from the xDate string
                string year = xDate.Substring(0, 4);
                string month = xDate.Substring(5, 2);
                string day = xDate.Substring(8, 2);

                // Concatenate the date parts with the provided time and milliseconds
                string formattedDateTime = $"{year}-{month}-{day} {time}:00.000";

                return formattedDateTime;
            }
            catch (Exception ex)
            {
                // Preserve original behavior: write to console and rethrow
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Attempts parsing input date into yyyy-MM-dd format; returns null on failure.
        /// </summary>
        public static string? FixConvertDateFormateToDash(string str_date)
        {
            DateTime date;
            if (
                DateTime.TryParseExact(str_date, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
                || DateTime.TryParseExact(str_date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
                || DateTime.TryParseExact(str_date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
                || DateTime.TryParseExact(str_date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
           )
            {
                // parsed to 'date'
            }
            else
            {
                return null;
            }

            return date.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Parses a date string into a <see cref="DateTime"/>. Accepts yyyy/MM/dd or dd/MM/yyyy and also hyphen separators.
        /// Throws <see cref="ArgumentException"/> for invalid inputs.
        /// </summary>
        public static DateTime StringToDate(string? dateString)
        {
            if (string.IsNullOrEmpty(dateString))
            {
                throw new ArgumentException("Date string cannot be null or empty.");
            }

            char[] separators = { '/', '-' };
            string[] dateParts = dateString.Split(separators);

            // Validate the length of the date parts
            if (dateParts.Length != 3)
            {
                throw new ArgumentException($"Invalid date string format: {dateString}");
            }

            int year, month, day;

            // Check if the year part has four digits to determine the format
            if (dateParts[0].Length == 4)
            {
                // yyyy/MM/dd format
                if (!int.TryParse(dateParts[0], out year) ||
                    !int.TryParse(dateParts[1], out month) ||
                    !int.TryParse(dateParts[2], out day))
                {
                    throw new ArgumentException($"Invalid date string format: {dateString}");
                }
            }
            else
            {
                // dd/MM/yyyy format
                if (!int.TryParse(dateParts[2], out year) ||
                    !int.TryParse(dateParts[1], out month) ||
                    !int.TryParse(dateParts[0], out day))
                {
                    throw new ArgumentException($"Invalid date string format: {dateString}");
                }
            }

            // Validate month and day ranges
            if (month < 1 || month > 12 || day < 1 || day > DateTime.DaysInMonth(year, month))
            {
                throw new ArgumentException($"Invalid date values in date string format: {dateString}");
            }

            return new DateTime(year, month, day);
        }

        /// <summary>
        /// Returns the difference in minutes between two times in HH:mm format. Returns 0 on parse error.
        /// </summary>
        public static int CalculateMinutesDifference(string timeIn, string timeOut)
        {
            try
            {
                var timeInDate = DateTime.ParseExact(timeIn, "HH:mm", CultureInfo.InvariantCulture);
                var timeOutDate = DateTime.ParseExact(timeOut, "HH:mm", CultureInfo.InvariantCulture);

                return (int)(timeOutDate - timeInDate).TotalMinutes;
            }
            catch (Exception)
            {
                // Log the exception details and return 0 as a safe default
                return 0;
            }
        }

        /// <summary>
        /// Returns the current UTC date in yyyy/MM/dd format.
        /// </summary>
        public static string GetDateGregorianDotNow()
        {
            return DateTime.UtcNow.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the current local <see cref="DateTime"/>.
        /// </summary>
        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// Placeholder for Hijri-to-Gregorian conversion. Current implementation returns input unchanged.
        /// </summary>
        public static string? HijriToGreg(string? hijri)
        {
            return hijri;
        }

        /// <summary>
        /// Calculates whole years between two dates using month/day adjustments.
        /// </summary>
        public static int GetCountYears(DateTime startDate, DateTime endDate)
        {
            try
            {
                int months = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month;

                if (endDate.Day < startDate.Day)
                {
                    months -= 1;
                }

                int years = months / 12;
                return years;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"An error occurred while calculating years: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Calculates the remaining months (0-11) between two dates after subtracting whole years.
        /// </summary>
        public static int GetCountMonths(DateTime startDate, DateTime endDate)
        {
            try
            {
                int months = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month;

                if (endDate.Day < startDate.Day)
                {
                    months -= 1;
                }

                months = months % 12;
                return months;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while calculating months: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Calculates day difference (0-30) between two dates' day-of-month values with borrowing from months when needed.
        /// </summary>
        public static int GetCountDays(DateTime startDate, DateTime endDate)
        {
            try
            {
                int days;

                if (endDate.Day < startDate.Day)
                {
                    int daysInEndMonth = DateTime.DaysInMonth(endDate.Year, endDate.Month);
                    days = daysInEndMonth - startDate.Day + endDate.Day;
                }
                else
                {
                    days = endDate.Day - startDate.Day;
                }

                return days;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while calculating days: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Returns the Hijri or Gregorian year depending on CalendarType. Default CalendarType "1" uses Arabic culture.
        /// </summary>
        public static int YearHijri(string CalendarType = "1")
        {
            try
            {
                string res = "";
                if (CalendarType == "2")
                    res = DateTime.Now.AddDays(-1).ToString("yyyy", enCul.DateTimeFormat);
                else
                    res = DateTime.Now.AddDays(-1).ToString("yyyy", arCul.DateTimeFormat);

                return Convert.ToInt32(res);
            }
            catch
            {
                return 0;
            }
        }

    }
}

