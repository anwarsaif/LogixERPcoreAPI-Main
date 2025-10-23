using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Logix.Application.Common
{
    /// <summary>
    /// Helper utilities used across the application (string/list conversions and Arabic number-to-words routines).
    /// </summary>
    public static class MethodsHelper {

        /// <summary>
        /// Joins a list of values into a single delimited string using each item's <c>ToString()</c>.
        /// </summary>
        /// <typeparam name="T">Item type of the list.</typeparam>
        /// <param name="numbers">List of items to join.</param>
        /// <param name="delimiter">Delimiter character to use (defaults to comma).</param>
        /// <returns>Delimited string or an empty string when the input list is null/empty.</returns>
        public static string ConvertListToString<T>(List<T> numbers, char delimiter = ',')

        {
            // Guard clause: return empty string for null or empty collections.
            if (numbers == null || numbers.Count == 0)
            {
                return string.Empty;
            }

            // Join elements using the delimiter. Each element is converted with ToString().
            return string.Join(delimiter.ToString(), numbers.Select(x => x.ToString()));
        }

        /// <summary>
        /// Parses a delimited string into a list of integers.
        /// </summary>
        /// <param name="str">Delimited input string (example: "1,2,3").</param>
        /// <param name="delimiter">Delimiter character (default is ',').</param>
        /// <returns>List of parsed integers; returns an empty list for null/empty input.</returns>
        public static List<int> ConvertStringToIntList(string str, char delimiter = ',')
        {
            // Return an empty list for null or empty input to avoid exceptions.
            if (string.IsNullOrEmpty(str))
            {
                return new List<int>(); 
            }

            // Split and parse each segment using invariant culture to avoid locale-specific parsing differences.
            return str.Split(delimiter)
                      .Select(x => int.Parse(x, CultureInfo.InvariantCulture)) // Use InvariantCulture for parsing consistency
                      .ToList();
        }  
        /// <summary>
        /// Parses a delimited string into a list of long integers.
        /// </summary>
        /// <param name="str">Delimited input string.</param>
        /// <param name="delimiter">Delimiter character.</param>
        /// <returns>List of parsed long values; empty when input is null or empty.</returns>
        public static List<long> ConvertStringToLongList(string str, char delimiter = ',')
        {
            if (string.IsNullOrEmpty(str))
            {
                return new List<long>(); 
            }

            // Parse using InvariantCulture for consistent numeric parsing across locales.
            return str.Split(delimiter)
                      .Select(x => long.Parse(x, CultureInfo.InvariantCulture)) // Use InvariantCulture for parsing consistency
                      .ToList();
        }
        /// <summary>
        /// Converts a small integer group (0..999) into its Arabic words representation.
        /// This is an internal helper used by the higher-level number-to-words routines.
        /// </summary>
        /// <param name="x">Integer value in the 0..999 range.</param>
        /// <returns>Arabic phrase representing the number group.</returns>
        private static string SHorof(int x)
        {
            // Prepare working variables for units/tens/hundreds and larger group concatenation.
            string letter1 = "", letter2 = "", letter3 = "", letter4 = "", letter5 = "", letter6 = "";
            int n = x;
            // Pad the number to 12 digits to consistently extract groups (billions, millions, thousands, units).
            string c = n.ToString("000000000000");
            int c1 = int.Parse(c.Substring(11, 1));
            switch (c1)
            {
                case 1: letter1 = "واحد"; break;
                case 2: letter1 = "اثنان"; break;
                case 3: letter1 = "ثلاثة"; break;
                case 4: letter1 = "اربعة"; break;
                case 5: letter1 = "خمسة"; break;
                case 6: letter1 = "ستة"; break;
                case 7: letter1 = "سبعة"; break;
                case 8: letter1 = "ثمانية"; break;
                case 9: letter1 = "تسعة"; break;
            }
            int c2 = int.Parse(c.Substring(10, 1));
            switch (c2)
            {
                case 1: letter2 = "عشر"; break;
                case 2: letter2 = "عشرون"; break;
                case 3: letter2 = "ثلاثون"; break;
                case 4: letter2 = "اربعون"; break;
                case 5: letter2 = "خمسون"; break;
                case 6: letter2 = "ستون"; break;
                case 7: letter2 = "سبعون"; break;
                case 8: letter2 = "ثمانون"; break;
                case 9: letter2 = "تسعون"; break;
            }
            // Combine unit and tens into the correct Arabic phrase using conjunctions and special cases.
            if (!string.IsNullOrEmpty(letter1) && c2 > 1) letter2 = letter1 + " و" + letter2;
            if (string.IsNullOrEmpty(letter2)) letter2 = letter1;
            if (c1 == 0 && c2 == 1) letter2 += "ة";
            if (c1 == 1 && c2 == 1) letter2 = "احدى عشر";
            if (c1 == 2 && c2 == 1) letter2 = "اثنى عشر";
            if (c1 > 2 && c2 == 1) letter2 = letter1 + " " + letter2;

            int c3 = int.Parse(c.Substring(9, 1));
            switch (c3)
            {
                case 1: letter3 = "مائة"; break;
                case 2: letter3 = "مئتان"; break;
                case > 2: letter3 = SHorof(c3) + "مائة"; break;
            }
            // Attach tens/units to hundreds when both exist.
            if (!string.IsNullOrEmpty(letter3) && !string.IsNullOrEmpty(letter2)) letter3 += " و" + letter2;
            if (string.IsNullOrEmpty(letter3)) letter3 = letter2;

            int c4 = int.Parse(c.Substring(6, 3));
            switch (c4)
            {
                case 1: letter4 = "الف"; break;
                case 2: letter4 = "الفان"; break;
                case >= 3 and <= 10: letter4 = SHorof(c4) + " آلاف"; break;
                case > 10: letter4 = SHorof(c4) + " الف"; break;
            }
            // Thousands group handling and concatenation with lower groups.
            if (!string.IsNullOrEmpty(letter4) && !string.IsNullOrEmpty(letter3)) letter4 += " و" + letter3;
            if (string.IsNullOrEmpty(letter4)) letter4 = letter3;

            int c5 = int.Parse(c.Substring(3, 3));
            switch (c5)
            {
                case 1: letter5 = "مليون"; break;
                case 2: letter5 = "مليونان"; break;
                case >= 3 and <= 10: letter5 = SHorof(c5) + " ملايين"; break;
                case > 10: letter5 = SHorof(c5) + " مليون"; break;
            }
            // Millions group handling.
            if (!string.IsNullOrEmpty(letter5) && !string.IsNullOrEmpty(letter4)) letter5 += " و" + letter4;
            if (string.IsNullOrEmpty(letter5)) letter5 = letter4;

            int c6 = int.Parse(c.Substring(0, 3));
            switch (c6)
            {
                case 1: letter6 = "مليار"; break;
                case 2: letter6 = "ملياران"; break;
                case > 2: letter6 = SHorof(c6) + " مليار"; break;
            }
            // Billions group handling and final composition.
            if (!string.IsNullOrEmpty(letter6) && !string.IsNullOrEmpty(letter5)) letter6 += " و" + letter5;
            if (string.IsNullOrEmpty(letter6)) letter6 = letter5;

            return letter6;
        }
        /// <summary>
        /// Builds the full Arabic phrase for a decimal value by combining the integer and fractional parts with currency unit names.
        /// </summary>
        /// <param name="x">The decimal number to convert (e.g., invoice amount).</param>
        /// <param name="ma1">Main currency unit name (e.g., "ريال").</param>
        /// <param name="mi1">Minor currency unit name (e.g., "هللة").</param>
        /// <returns>Localized Arabic textual representation of the amount.</returns>
        public static string Horof(decimal x, string ma1, string mi1)
        {
            string result = "";
            string ma = ma1;
            string mi = mi1;
            // Separate integer and fractional parts
            int n = (int)Math.Floor(x);
            int b = (int)(x * 100) % 100;
            string r = SHorof(n);
            string h = SHorof(b);

            // Combine integer and fractional parts depending on presence of each.
            if (!string.IsNullOrEmpty(r) && b > 0)
            {
                result = r + " " + ma + " و " + h + " " + mi;
            }
            else if (!string.IsNullOrEmpty(r) && b == 0)
            {
                result = r + " " + ma;
            }
            else if (string.IsNullOrEmpty(r) && b != 0)
            {
                result = h + " " + mi;
            }

            return result;
        }

        /// <summary>
        /// Converts a decimal number into its Arabic textual representation with currency words.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        /// <returns>Arabic textual representation of the number with currency units.</returns>
        public static string Tafkeet(decimal number)
        {
            // Handle zero and negative special cases explicitly.
            if (number == 0)
                return "صفر";

            if (number < 0)
                return "رقم غير صحيح";

            string result = "فقط ";

            long integerPart = (long)Math.Floor(number);
            long fractionalPart = (long)((number - integerPart) * 100);

            // Convert integer part and append the main currency word.
            result += ConvertToWords(integerPart) + " ريال";

            // If fractional part exists, convert and append the minor currency word.
            if (fractionalPart > 0)
            {
                result += " و " + ConvertToWords(fractionalPart) + " هللة";
            }

            result += " سعودي لا غير";

            return result.Trim();
        }
        /// <summary>
        /// Arabic word mappings for base numbers used by the recursive <see cref="ConvertToWords"/> routine.
        /// Keys include units, teens, tens and common hundreds.
        /// </summary>
        private static readonly Dictionary<int, string> NumberNames = new Dictionary<int, string>
    {
        { 0, "" }, { 1, "واحد" }, { 2, "اثنان" }, { 3, "ثلاثة" }, { 4, "أربعة" },
        { 5, "خمسة" }, { 6, "ستة" }, { 7, "سبعة" }, { 8, "ثمانية" }, { 9, "تسعة" },
        { 10, "عشرة" }, { 11, "إحدى عشر" }, { 12, "اثنى عشر" }, { 13, "ثلاثة عشر" },
        { 14, "أربعة عشر" }, { 15, "خمسة عشر" }, { 16, "ستة عشر" }, { 17, "سبعة عشر" },
        { 18, "ثمانية عشر" }, { 19, "تسعة عشر" }, { 20, "عشرون" }, { 30, "ثلاثون" },
        { 40, "أربعون" }, { 50, "خمسون" }, { 60, "ستون" }, { 70, "سبعون" },
        { 80, "ثمانون" }, { 90, "تسعون" }, { 100, "مائة" }, { 200, "مائتان" },
        { 300, "ثلاثمائة" }, { 400, "أربعمائة" }, { 500, "خمسمائة" }, { 600, "ستمائة" },
        { 700, "سبعمائة" }, { 800, "ثمانمائة" }, { 900, "تسعمائة" }
    };
        /// <summary>
        /// Converts an integer into its Arabic words representation (supports up to trillions in the current logic).
        /// </summary>
        /// <param name="number">The positive integer to convert.</param>
        /// <returns>Arabic words for the provided number.</returns>
        private static string ConvertToWords(long number)
        {
            // Handle edge cases and progressively larger magnitudes by recursion.
            if (number == 0)
                return string.Empty;

            if (number <= 19)
                return NumberNames[(int)number];

            if (number <= 99)
                return NumberNames[(int)(number - number % 10)] + (number % 10 != 0 ? " و " + ConvertToWords(number % 10) : "");

            if (number <= 999)
                return NumberNames[(int)(number - number % 100)] + (number % 100 != 0 ? " و " + ConvertToWords(number % 100) : "");

            if (number <= 999999)
            {
                return ConvertToWords(number / 1000) + " ألف" + (number % 1000 != 0 ? " و " + ConvertToWords(number % 1000) : "");
            }

            if (number <= 999999999)
            {
                return ConvertToWords(number / 1000000) + " مليون" + (number % 1000000 != 0 ? " و " + ConvertToWords(number % 1000000) : "");
            }

            if (number <= 999999999999)
            {
                return ConvertToWords(number / 1000000000) + " مليار" + (number % 1000000000 != 0 ? " و " + ConvertToWords(number % 1000000000) : "");
            }

            return ConvertToWords(number / 1000000000000) + " ترليون" + (number % 1000000000000 != 0 ? " و " + ConvertToWords(number % 1000000000000) : "");
        }
    }
}

