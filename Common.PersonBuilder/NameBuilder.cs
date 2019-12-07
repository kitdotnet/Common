using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.PersonBuilder
{
    public partial class PersonBuilder
    {
        protected Person person;

        public PersonBuilder()
        {
            person = new Person();
        }

        public Person WithName(PersonName personName)
        {
            person.Name = personName;
            return person;
        }

        public Person WithName(string firstName,
            string lastName,
            string middleName = default,
            string suffix = default)
        {
            person.Name = new PersonName(firstName: firstName,
                lastName: lastName,
                middleName: middleName,
                suffix: suffix);
            return person;
        }

        public Person WithName(Gender gender, bool includeMiddleName = true)
        {
            if (gender == Gender.Other)
            {
                gender = (Gender)random.Next(0, 2);
            }

            string surname = surnames.ElementAt(random.Next(0, surnameCount));
            string firstName = gender switch
            {
                Gender.Male => maleNames.ElementAt(random.Next(0, maleNameCount)),
                _ => femaleNames.ElementAt(random.Next(0, femaleNameCount))
            };
            string middleName = includeMiddleName ? gender switch
            {
                Gender.Male => maleNames.ElementAt(random.Next(0, maleNameCount)),
                _ => femaleNames.ElementAt(random.Next(0, femaleNameCount))
            } : null;

            return WithName(firstName: firstName, lastName: surname, middleName: middleName);
        }
    }
}
