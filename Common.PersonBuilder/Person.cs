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
        public Age Age { get; internal set; }
    }
}
