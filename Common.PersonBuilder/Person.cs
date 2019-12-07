using System;
using System.Collections.Generic;
using System.Text;

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
        /// Gets the person's name.
        /// </summary>
        public PersonName Name { get; internal set; }

        /// <summary>
        /// Gets the person's age.
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

        public DateTime? DateOfBirth
        {
            get
            {
                return dateOfBirth;
            }
            set
            {
                if (value == default)
                {
                    age = -1;
                }
                else
                {
                    age = new Age(Maths.Calculate.AgeInYears(value.Value, DateTime.Now));
                }
                dateOfBirth = value;
            }
        }
    }
}
