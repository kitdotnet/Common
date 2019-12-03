using System;

namespace Common.Transformations
{
    /// <summary>
    /// Utility class for common manipulations of <see cref="DateTime"/> objects.
    /// </summary>
    public static class TransformDateTime
    {
        /// <summary>
        /// Converts a <see cref="DateTime"/> to the start of its day, preserving the <see cref="DateTime.Kind"/>.
        /// </summary>
        /// <param name="date">The date to transform.</param>
        /// <returns>Start time of the date passed in; the <see cref="DateTime.Kind"/> of date is preserved.</returns>
        public static DateTime StartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0, date.Kind);
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> to the end of its day, preserving the <see cref="DateTime.Kind"/>.
        /// </summary>
        /// <param name="date">The date to transform.</param>
        /// <returns>Ending time of the date passed in; the <see cref="DateTime.Kind"/> of date is preserved.</returns>
        public static DateTime EndOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999, date.Kind);
        }
    }
}
