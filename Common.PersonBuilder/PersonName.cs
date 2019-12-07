﻿using System;
using System.Collections.Generic;

namespace Common.PersonBuilder
{
    /// <summary>
    /// Represents a person's name.
    /// </summary>
    public readonly struct PersonName : IEquatable<PersonName>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="PersonName"/> struct.
        /// </summary>
        /// <param name="firstName">The person's first name.</param>
        /// <param name="lastName">The person's last name.</param>
        /// <param name="middleName">The person's middle name.</param>
        /// <param name="suffix">The suffix of the person's name.</param>
        public PersonName(string firstName,
            string lastName,
            string middleName = default,
            string suffix = default)
        {
            FirstName = string.IsNullOrWhiteSpace(firstName) ? throw new ArgumentNullException(nameof(firstName)) : firstName.Trim();
            LastName = string.IsNullOrWhiteSpace(lastName) ? throw new ArgumentNullException(nameof(lastName)) : lastName.Trim();
            MiddleName = middleName?.Trim();
            Suffix = suffix?.Trim();
        }

        /// <summary>
        /// Gets the person's first name.
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// Gets the person's last name.
        /// </summary>
        public string LastName { get; }

        /// <summary>
        /// Gets the person's middle name.
        /// </summary>
        public string MiddleName { get; }

        /// <summary>
        /// Gets the person's middle initial.
        /// </summary>
        public string MiddleInitial => string.IsNullOrWhiteSpace(MiddleName)
                ? null
                : MiddleName.Substring(0, 1);

        /// <summary>
        /// Get the suffix of the person's name.
        /// </summary>
        public string Suffix { get; }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj is PersonName name && Equals(name);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(PersonName other)
        {
            return FirstName == other.FirstName &&
                   LastName == other.LastName &&
                   MiddleName == other.MiddleName &&
                   Suffix == other.Suffix;
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = -2016661541;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LastName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MiddleName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Suffix);
            return hashCode;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return ToString(PersonNameFormat.Full);
        }

        /// <summary>
        /// Returns a formatted string that represents the person's name.
        /// </summary>
        /// <param name="format">A <see cref="PersonNameFormat"/> value to dictate the name's format.</param>
        /// <param name="includeSuffix">If true, the person's suffix (if any) will be added to the end of the name.</param>
        /// <returns>A string that represents the person's name.</returns>
        public string ToString(PersonNameFormat format, bool includeSuffix = false)
        {
            string middleInitial = string.IsNullOrWhiteSpace(MiddleName) 
                ? string.Empty
                : MiddleName.Substring(0, 1);

            var name = format switch
            {
                PersonNameFormat.FirstMiddleInitialLast => $"{FirstName} {middleInitial} {LastName}",
                PersonNameFormat.FirstLast => $"{FirstName} {LastName}",
                PersonNameFormat.LastFirst => $"{LastName}, {FirstName} ",
                PersonNameFormat.LastFirstMiddle => $"{LastName}, {FirstName} {MiddleName}",
                PersonNameFormat.LastFirstMiddleInitial => $"{LastName}, {FirstName} {MiddleName}",
                _ => $"{FirstName} {MiddleName} {LastName}",
            };

            if (includeSuffix && !string.IsNullOrWhiteSpace(Suffix))
            {
                name = $"{name} {Suffix}";
            }

            return Transformations.TransformRawText.CondenseSpacingAndTrim(name);
        }
    }
}
