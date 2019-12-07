using System;
using System.Collections.Generic;
using System.Text;

namespace Common.PersonBuilder
{
    public partial class PersonBuilder
    {
        public PersonBuilder WithAge(int age)
        {
            person.Age = age;
            return this;
        }

        public PersonBuilder WithDateOfBirth(DateTime dateOfBirth)
        {
            person.DateOfBirth = dateOfBirth;
            return this;
        }
    }
}
