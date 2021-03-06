﻿using System.Linq;

namespace Common.Validators
{
    /// <summary>
    /// Utility for validating a phone number.
    /// </summary>
    public static class UnitedStatesPhoneNumber
    {
        /// <summary>
        /// Determines if the structure of the phone number is valid.
        /// </summary>
        /// <param name="phoneNumber">The phone number to validate.</param>
        /// <param name="validCounts">An array of counts that are valid (e.g., 4, 7, 10).</param>
        /// <returns>An indicator of whether the structure of the phone number is valid.</returns>
        public static bool IsValidStructure(string phoneNumber, params int[] validCounts)
        {
            bool isValid = false;
            string numbersOnly = new string(phoneNumber.ToCharArray().Where(c => char.IsDigit(c)).ToArray());
            int length = numbersOnly.Length;

            if (validCounts != null && validCounts.Any())
            {
                isValid = validCounts.Contains(length);
            }
            else
            {
                isValid = length >= 10 && length <= 11;
            }

            return isValid;
        }
    }
}
