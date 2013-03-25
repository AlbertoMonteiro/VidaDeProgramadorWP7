using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Phone.Controls.LocalizedResources;

namespace VidaDeProgramador.Converters
{
    /// <summary>
    ///     Time converter to display elapsed time relatively to the present.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    public class RelativeTimeConverter : IValueConverter
    {
        /// <summary>
        ///     A minute defined in seconds.
        /// </summary>
        private const double Minute = 60.0;

        /// <summary>
        ///     An hour defined in seconds.
        /// </summary>
        private const double Hour = 60.0 * Minute;

        /// <summary>
        ///     A day defined in seconds.
        /// </summary>
        private const double Day = 24 * Hour;

        /// <summary>
        ///     A week defined in seconds.
        /// </summary>
        private const double Week = 7 * Day;

        /// <summary>
        ///     A month defined in seconds.
        /// </summary>
        private const double Month = 30.5 * Day;

        /// <summary>
        ///     A year defined in seconds.
        /// </summary>
        private const double Year = 365 * Day;

        /// <summary>
        ///     Abbreviation for the default culture used by resources files.
        /// </summary>
        private const string DefaultCulture = "pt-BR";

        /// <summary>
        ///     Four different strings to express hours in plural.
        /// </summary>
        private string[] PluralHourStrings;

        /// <summary>
        ///     Four different strings to express minutes in plural.
        /// </summary>
        private string[] PluralMinuteStrings;

        /// <summary>
        ///     Four different strings to express seconds in plural.
        /// </summary>
        private string[] PluralSecondStrings;

        /// <summary>
        ///     Converts a
        ///     <see cref="T:System.DateTime" />
        ///     object into a string the represents the elapsed time
        ///     relatively to the present.
        /// </summary>
        /// <param name="value">The given date and time.</param>
        /// <param name="targetType">
        ///     The type corresponding to the binding property, which must be of
        ///     <see cref="T:System.String" />.
        /// </param>
        /// <param name="parameter">(Not used).</param>
        /// <param name="culture">
        ///     The culture to use in the converter.
        ///     When not specified, the converter uses the current culture
        ///     as specified by the system locale.
        /// </param>
        /// <returns>The given date and time as a string.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Target value must be a System.DateTime object.
            if (!(value is DateTime))
            {
                throw new ArgumentException(Resource.InvalidDateTimeArgument);
            }

            string result;

            DateTime given = ((DateTime)value).ToLocalTime();

            DateTime current = DateTime.Now;

            TimeSpan difference = current - given;

            SetLocalizationCulture(new CultureInfo("pt-BR"));

            if (DateTimeFormatHelper.IsFutureDateTime(current, given))
            {
                // Future dates and times are not supported, but to prevent crashing an app
                // if the time they receive from a server is slightly ahead of the phone's clock
                // we'll just default to the minimum, which is "2 seconds ago".
                result = GetPluralTimeUnits(2, PluralSecondStrings);
            }

            if (difference.TotalSeconds > Year)
            {
                // "over a year ago"
                result = "a mais de um ano atrás";
            }
            else if (difference.TotalSeconds > (1.5 * Month))
            {
                // "x months ago"
                var nMonths = (int)((difference.TotalSeconds + Month / 2) / Month);
                result = GetPluralMonth(nMonths);
            }
            else if (difference.TotalSeconds >= (3.5 * Week))
            {
                // "about a month ago"
                result = "a cerca de um mês atrás";
            }
            else if (difference.TotalSeconds >= Week)
            {
                var nWeeks = (int)(difference.TotalSeconds / Week);
                if (nWeeks > 1)
                {
                    // "x weeks ago"
                    result = string.Format(CultureInfo.CurrentUICulture, "a {0} semanas atrás", nWeeks.ToString(ControlResources.Culture));
                }
                else
                {
                    // "about a week ago"
                    result = "a cerca de uma semana atrás";
                }
            }
            else if (difference.TotalSeconds >= (5 * Day))
            {
                // "last <dayofweek>"    
                result = string.Format(CultureInfo.CurrentUICulture, "na {0} passada", GetDayOfWeek(given.DayOfWeek));
            }
            else if (difference.TotalSeconds >= Day)
            {
                // "on <dayofweek>"
                result = GetOnDayOfWeek(given.DayOfWeek);
            }
            else if (difference.TotalSeconds >= (2 * Hour))
            {
                // "x hours ago"
                var nHours = (int)(difference.TotalSeconds / Hour);
                result = GetPluralTimeUnits(nHours, PluralHourStrings);
            }
            else if (difference.TotalSeconds >= Hour)
            {
                // "about an hour ago"
                result = "a cerca de uma hora atrás";
            }
            else if (difference.TotalSeconds >= (2 * Minute))
            {
                // "x minutes ago"
                var nMinutes = (int)(difference.TotalSeconds / Minute);
                result = GetPluralTimeUnits(nMinutes, PluralMinuteStrings);
            }
            else if (difference.TotalSeconds >= Minute)
            {
                // "about a minute ago"
                result = "a cerca de um minuto atrás";
            }
            else
            {
                // "x seconds ago" or default to "2 seconds ago" if less than two seconds.
                int nSeconds = ((int)difference.TotalSeconds > 1.0) ? (int)difference.TotalSeconds : 2;
                result = GetPluralTimeUnits(nSeconds, PluralSecondStrings);
            }

            return result;
        }

        /// <summary>
        ///     Not implemented.
        /// </summary>
        /// <param name="value">(Not used).</param>
        /// <param name="targetType">(Not used).</param>
        /// <param name="parameter">(Not used).</param>
        /// <param name="culture">(Not used).</param>
        /// <returns>null</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Resources use the culture in the system locale by default.
        ///     The converter must use the culture specified the ConverterCulture.
        ///     The ConverterCulture defaults to en-US when not specified.
        ///     Thus, change the resources culture only if ConverterCulture is set.
        /// </summary>
        /// <param name="culture">The culture to use in the converter.</param>
        private void SetLocalizationCulture(CultureInfo culture)
        {
            if (!culture.Name.Equals(DefaultCulture, StringComparison.Ordinal))
            {
                ControlResources.Culture = culture;
            }

            PluralHourStrings = new string[4]
            {
                "a {0} horas atrás ",
                "a {0} horas atrás ",
                "a {0} horas atrás ",
                "a {0} horas atrás "
            };

            PluralMinuteStrings = new string[4]
            {
                "a {0} minutos atrás ",
                "a {0} minutos atrás ",
                "a {0} minutos atrás ",
                "a {0} minutos atrás "
            };

            PluralSecondStrings = new string[4]
            {
                "a {0} segundos atrás",
                "a {0} segundos atrás",
                "a {0} segundos atrás",
                "a {0} segundos atrás"
            };
        }

        /// <summary>
        ///     Returns a localized text string to express months in plural.
        /// </summary>
        /// <param name="month">Number of months.</param>
        /// <returns>Localized text string.</returns>
        private static string GetPluralMonth(int month)
        {
            if (month >= 2 && month <= 4)
            {
                return string.Format(CultureInfo.CurrentUICulture, ControlResources.XMonthsAgo_2To4, month.ToString(ControlResources.Culture));
            }
            else if (month >= 5 && month <= 12)
            {
                return string.Format(CultureInfo.CurrentUICulture, ControlResources.XMonthsAgo_5To12, month.ToString(ControlResources.Culture));
            }
            else
            {
                throw new ArgumentException(Resource.InvalidNumberOfMonths);
            }
        }

        /// <summary>
        ///     Returns a localized text string to express time units in plural.
        /// </summary>
        /// <param name="units">
        ///     Number of time units, e.g. 5 for five months.
        /// </param>
        /// <param name="resources">
        ///     Resources related to the specified time unit.
        /// </param>
        /// <returns>Localized text string.</returns>
        private static string GetPluralTimeUnits(int units, string[] resources)
        {
            int modTen = units % 10;
            int modHundred = units % 100;

            if (units <= 1)
            {
                throw new ArgumentException(Resource.InvalidNumberOfTimeUnits);
            }
            else if (units >= 2 && units <= 4)
            {
                return string.Format(CultureInfo.CurrentUICulture, resources[0], units.ToString(ControlResources.Culture));
            }
            else if (modTen == 1 && modHundred != 11)
            {
                return string.Format(CultureInfo.CurrentUICulture, resources[1], units.ToString(ControlResources.Culture));
            }
            else if ((modTen >= 2 && modTen <= 4) && !(modHundred >= 12 && modHundred <= 14))
            {
                return string.Format(CultureInfo.CurrentUICulture, resources[2], units.ToString(ControlResources.Culture));
            }
            else
            {
                return string.Format(CultureInfo.CurrentUICulture, resources[3], units.ToString(ControlResources.Culture));
            }
        }

        /// <summary>
        ///     Returns a localized text string for the day of week.
        /// </summary>
        /// <param name="dow">Day of week.</param>
        /// <returns>Localized text string.</returns>
        private static string GetDayOfWeek(DayOfWeek dow)
        {
            string result;

            switch (dow)
            {
                case DayOfWeek.Monday:
                    result = "Segunda";
                    break;
                case DayOfWeek.Tuesday:
                    result = "Terça";
                    break;
                case DayOfWeek.Wednesday:
                    result = "Quarta";
                    break;
                case DayOfWeek.Thursday:
                    result = "Quinta";
                    break;
                case DayOfWeek.Friday:
                    result = "Sexta";
                    break;
                case DayOfWeek.Saturday:
                    result = "Sabado";
                    break;
                default:
                    result = "Domingo";
                    break;
            }

            return result;
        }

        /// <summary>
        ///     Returns a localized text string to express "on {0}"
        ///     where {0} is a day of the week, e.g. Sunday.
        /// </summary>
        /// <param name="dow">Day of week.</param>
        /// <returns>Localized text string.</returns>
        private static string GetOnDayOfWeek(DayOfWeek dow)
        {
            if (dow == DayOfWeek.Tuesday)
            {
                return string.Format(CultureInfo.CurrentUICulture, "na {0}", GetDayOfWeek(dow));
            }
            else
            {
                return string.Format(CultureInfo.CurrentUICulture, "na {0}", GetDayOfWeek(dow));
            }
        }
    }
}