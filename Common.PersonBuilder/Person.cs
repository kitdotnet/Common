using System;
using System.Collections.Generic;

namespace Common.PersonBuilder
{
    /// <summary>
    /// Represents a person.
    /// </summary>
    public class Person
    {
        private DateTime? dateOfBirth = default;
        private Age age = -1;
        private Random random = new Random();

        /// <summary>
        /// Creates a new instance of the <see cref="Person"/> class.
        /// </summary>
        public Person()
        {
        }

        /// <summary>
        /// Gets this person's name.
        /// </summary>
        public PersonName Name { get; internal set; }

        /// <summary>
        /// Gets this person's gender.
        /// </summary>
        public Gender Gender { get; internal set; } = Gender.Other;

        /// <summary>
        /// Gets this person's age.
        /// </summary>
        public Age Age
        {
            get
            {
                return age;
            }
            internal set
            {
                age = value;

                dateOfBirth = DateTime.Now.AddYears(-value).AddDays(-1 * random.Next(1, 365));
            }
        }

        /// <summary>
        /// Gets this person's date of birth.
        /// </summary>
        public DateTime? DateOfBirth
        {
            get
            {
                return dateOfBirth;
            }
            internal set
            {
                if (value == default)
                {
                    age = default;
                }
                else
                {
                    age = new Age(Maths.Calculate.AgeInYears(value.Value, DateTime.Now));
                }
                dateOfBirth = value;
            }
        }

        public IEnumerable<string> Races { get; internal set; } = new List<string>();

        public string Ethnicity { get; internal set; }
    }
}
